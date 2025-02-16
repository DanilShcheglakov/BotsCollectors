using System;
using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(CollisionHandler))]
public class Unit : MonoBehaviour
{
	[SerializeField] private BackPack _backPack;
	[SerializeField] private Base _basePrefab;

	private Base _base;
	private UnitMover _mover;
	private CollisionHandler _collisionHandler;
	private Transform _target;

	private bool _isThisBuilder;

	public event Action<Unit> Builded;

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

	public void SetTarget(Transform newTarget, bool isItResource)
	{
		_target = newTarget;
		IsBusy = isItResource;
	}

	public void SetBuilder()
	{
		_isThisBuilder = true;
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
			if (_backPack.IsFilled == false)
				return;

			if (unitBase == _base)
			{
				while (_backPack.IsThereResources)
				{
					_base.CollectResource(_backPack.GiveItem());
				}

				SetTarget(_base.CollectionPoint, false);
			}
		}
		else if (collision.gameObject.TryGetComponent<Flag>(out Flag flag) && _isThisBuilder)
		{
			Vector3 buildPosition = new Vector3(flag.transform.position.x, _basePrefab.transform.position.y, flag.transform.position.z);

			Builded?.Invoke(this);
			SetNewBase(Instantiate(_basePrefab, buildPosition, Quaternion.identity, null));
		}
	}

	private void ComeBackToBase()
	{
		SetTarget(_base.transform, true);
	}

	private void SetFree()
	{
		IsBusy = false;
		_isThisBuilder = false;
	}

	private void SetNewBase(Base newBase)
	{
		_base = newBase;
		SetFree();
		_base.AttachUnit(this);
	}
}
