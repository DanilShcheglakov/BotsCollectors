using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitSpawner), typeof(Rigidbody))]
public class Base : MonoBehaviour
{
	[SerializeField] private Transform _instantiatingPoint;
	[SerializeField] private Transform _collectionPoint;

	private UnitSpawner _unitSpawner;

	private List<GameResource> _findingResources;
	private List<Unit> _units;
	private Collider[] _collidersBuffer;

	private int _goldCount = 0;
	private int _starUnitsCount = 3;
	private int _unitPrice = 3;

	public Transform InstantiatingPoint => _instantiatingPoint;
	public Transform CollectionPoint => _collectionPoint;
	public int StartUnitCount => _starUnitsCount;

	public event Action<int> GoldChanges;
	public event Action<int> UnitsChanges;

	private void Awake()
	{
		_unitSpawner = gameObject.GetComponent<UnitSpawner>();

		_units = new List<Unit>();
		_findingResources = new List<GameResource>();
		_collidersBuffer = new Collider[50];
	}

	private void Start()
	{
		for (int i = 0; i < _starUnitsCount; i++)
			_units.Add(_unitSpawner.GetPrefab());

		UnitsChanges?.Invoke(_units.Count);
	}

	private void Update()
	{
		BuyUnit();
		ScanArea();
	}

	public void CollectResource(GameResource resource)
	{
		if (resource is Gold)
			_goldCount++;

		resource.Delete();

		GoldChanges?.Invoke(_goldCount);
	}

	private void ScanArea()
	{
		_findingResources.Clear();
		int count = Physics.OverlapSphereNonAlloc(Vector3.zero, 20f, _collidersBuffer);

		for (int i = 0; i < count; i++)
			if (_collidersBuffer[i].gameObject.TryGetComponent<GameResource>(out GameResource resource) && !resource.IsBooked)
				_findingResources.Add(resource);

		SendUnitForCollectResource();
	}

	private void SendUnitForCollectResource()
	{
		if (_findingResources.Count > 0)
		{
			for (int i = 0; i < _findingResources.Count; i++)
			{
				foreach (var unit in _units)
				{
					if (!unit.IsBusy)
					{
						unit.SetNewTarget(_findingResources[i].transform, true);
						_findingResources[i].Booking();
						return;
					}
				}
			}
		}
		else
		{
			CollectUnitsAtTheBase();
		}
	}

	private void CollectUnitsAtTheBase()
	{
		foreach (var unit in _units)
		{
			if (!unit.IsBusy)
			{
				if (unit.IsEmptyBackPack)
				{
					unit.SetNewTarget(CollectionPoint, false);
				}
				else
				{
					unit.SetNewTarget(transform, false);
				}
			}
		}
	}

	private void BuyUnit()
	{
		if (_goldCount >= _unitPrice)
		{
			_units.Add(_unitSpawner.GetPrefab());
			_goldCount -= _unitPrice;

			UnitsChanges?.Invoke(_units.Count);
			GoldChanges?.Invoke(_goldCount);
		}
	}
}
