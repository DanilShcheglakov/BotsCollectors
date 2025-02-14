using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(CollisionHandler))]
public class Unit : MonoBehaviour
{
	[SerializeField] private BackPack _backPack;

	private Base _base;
	private UnitMover _mover;
	private CollisionHandler _collisionHandler;
	private Transform _target;

	public bool IsBusy { get; private set; }
	public bool IsEmptyBackPack => _backPack.IsThereResources == false;

	private void Awake()
	{
		_mover = GetComponent<UnitMover>();
		_collisionHandler = GetComponent<CollisionHandler>();

	}

	private void OnEnable()
	{
		_collisionHandler.CollisionDetected += HandleCollisionEnter;
		_backPack.Filled += ComeBackToBase;
		_backPack.Devastated += SetFree;
	}

	private void OnDisable()
	{
		_collisionHandler.CollisionDetected -= HandleCollisionEnter;
		_backPack.Filled -= ComeBackToBase;
		_backPack.Devastated -= SetFree;
	}

	private void Update()
	{
		_mover.Move(_target);
	}

	public void SetDefaultSettings(Base unitsBase, Transform defaultTransform)
	{
		_base = unitsBase;
		_target = unitsBase.CollectionPoint;
		transform.position = defaultTransform.position;
		IsBusy = false;
	}

	public void SetNewTarget(Transform newTarget, bool isItResource)
	{
		_target = newTarget;
		IsBusy = isItResource;
	}

	private void PickUpResource(GameResource resource)
	{
		_backPack.AddResource(resource);
		IsBusy = !IsEmptyBackPack;
	}

	private void HandleCollisionEnter(Collider collision)
	{
		if (collision.TryGetComponent<GameResource>(out GameResource gameResource))
		{
			if (_backPack.IsFilled)
				return;

			PickUpResource(gameResource);
		}
		else if (collision.gameObject.TryGetComponent<Base>(out Base unitBase))
		{
			if (unitBase == _base)
			{
				while (_backPack.IsThereResources)
				{
					_base.CollectResource(_backPack.GiveItem());
				}

				SetNewTarget(_base.CollectionPoint, false);
			}
		}
	}

	private void ComeBackToBase()
	{
		SetNewTarget(_base.transform, true);
	}

	private void SetFree()
	{
		IsBusy = false;
	}
}
