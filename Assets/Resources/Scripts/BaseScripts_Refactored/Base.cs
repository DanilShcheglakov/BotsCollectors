using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitSpawner), typeof(Rigidbody), typeof(Scaner))]
public class Base : MonoBehaviour
{
	[SerializeField] private Transform _instantiatingPoint;
	[SerializeField] private Transform _collectionPoint;

	private Scaner _scaner;
	private UnitSpawner _unitSpawner;

	private List<Unit> _units;

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
		_scaner = gameObject.GetComponent<Scaner>();

		_units = new List<Unit>();

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
		SendUnitForCollectResource();
	}

	public void CollectResource(GameResource resource)
	{
		if (resource is Gold)
			_goldCount++;

		_bookedResources.Remove(resource);
		resource.Delete();

		GoldChanged?.Invoke(_goldCount);
	}

	private void SendUnitForCollectResource()
	{
		SortFindingResources(_scaner.ScanArea());

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

	private void SortFindingResources(List<GameResource> findingResources)
	{
		bool isResourceBooked = false;

		if (findingResources.Count == 0)
			return;

		_freeResources.Clear();

		for (int i = 0; i < findingResources.Count; i++)
		{
			for (int j = 0; j < _bookedResources.Count; j++)
			{
				if (findingResources[i] == _bookedResources[j])
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
				_freeResources.Add(findingResources[i]);
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
