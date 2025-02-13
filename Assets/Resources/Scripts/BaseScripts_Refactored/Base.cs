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

	private List<GameResource> _freeResources;
	private List<GameResource> _bookedResources;

	private int _goldCount = 0;
	private int _starUnitsCount = 3;
	private int _unitPrice = 3;

	public Transform InstantiatingPoint => _instantiatingPoint;
	public Transform CollectionPoint => _collectionPoint;
	public int StartUnitCount => _starUnitsCount;

	public event Action<int> GoldChanged;
	public event Action<int> UnitsChanged;

	private void Awake()
	{
		_unitSpawner = gameObject.GetComponent<UnitSpawner>();

		_units = new List<Unit>();
		_findingResources = new List<GameResource>();
		_collidersBuffer = new Collider[50];

		_freeResources = new List<GameResource>();
		_bookedResources = new List<GameResource>();
	}

	private void Start()
	{
		for (int i = 0; i < _starUnitsCount; i++)
			_units.Add(_unitSpawner.GetPrefab());

		UnitsChanged?.Invoke(_units.Count);
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

		_bookedResources.Remove(resource);
		resource.Delete();

		GoldChanged?.Invoke(_goldCount);
	}

	private void ScanArea()
	{
		_findingResources.Clear();
		int count = Physics.OverlapSphereNonAlloc(Vector3.zero, 20f, _collidersBuffer);

		for (int i = 0; i < count; i++)
			if (_collidersBuffer[i].gameObject.TryGetComponent<GameResource>(out GameResource resource))
				_findingResources.Add(resource);

		SortFindingResources();
		SendUnitForCollectResource();
	}

	private void SendUnitForCollectResource()
	{
		if (_freeResources.Count > 0)
		{
			for (int i = 0; i < _freeResources.Count; i++)
			{
				foreach (var unit in _units)
				{
					if (!unit.IsBusy)
					{
						unit.SetNewTarget(_freeResources[i].transform, true);

						_bookedResources.Add(_freeResources[i]);
						_freeResources.RemoveAt(i);

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

	private void SortFindingResources()
	{
		bool isResourceBooked = false;

		if (_findingResources.Count == 0)
			return;

		_freeResources.Clear();

		for (int i = 0; i < _findingResources.Count; i++)
		{
			for (int j = 0; j < _bookedResources.Count; j++)
			{
				if (_findingResources[i] == _bookedResources[j])
				{
					isResourceBooked = true;
					break;
				}
				else
				{
					isResourceBooked = false;
				}
			}

			if (!isResourceBooked)
			{
				_freeResources.Add(_findingResources[i]);
			}
		}
	}

	private void CollectUnitsAtTheBase()
	{
		foreach (var unit in _units)
			if (!unit.IsBusy)
				if (unit.IsEmptyBackPack)
					unit.SetNewTarget(CollectionPoint, false);
				else
					unit.SetNewTarget(transform, false);
	}

	private void BuyUnit()
	{
		if (_goldCount >= _unitPrice)
		{
			_units.Add(_unitSpawner.GetPrefab());
			_goldCount -= _unitPrice;

			UnitsChanged?.Invoke(_units.Count);
			GoldChanged?.Invoke(_goldCount);
		}
	}
}
