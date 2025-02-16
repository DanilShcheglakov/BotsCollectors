using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitSpawner), typeof(Rigidbody), typeof(Scanner))]
[RequireComponent(typeof(Coffers))]
public class Base : MonoBehaviour
{
	private static List<GameResource> FreeResources;
	private static List<GameResource> BookedResources;
	private static int CountOfBases = 0;

	[SerializeField] private Transform _instantiatingPoint;
	[SerializeField] private Transform _collectionPoint;
	[SerializeField] private Flag _flag;
	[SerializeField] private int _startUnitsCount = 3;

	private Scanner _scaner;
	private UnitSpawner _unitSpawner;
	private Coffers _coffers;
	private List<Unit> _units;

	private int _numberOfBase;
	private int _unitPrice = 3;
	private int _BasePrice = 5;

	private bool _isUnitSendingToBuild;

	public event Action<int> UnitsChanged;

	public Transform InstantiatingPoint => _instantiatingPoint;
	public Transform CollectionPoint => _collectionPoint;
	public int StartUnitCount => _startUnitsCount;

	private void Awake()
	{
		_unitSpawner = GetComponent<UnitSpawner>();
		_coffers = GetComponent<Coffers>();
		_scaner = GetComponent<Scanner>();

		_units = new List<Unit>();
		FreeResources = new List<GameResource>();
		BookedResources = new List<GameResource>();

		_flag.gameObject.SetActive(false);

		_numberOfBase = CountOfBases;
		CountOfBases++;
	}

	private void OnEnable()
	{
		_scaner.AreaScanned += SendUnitForCollectResource;
		_coffers.CountChanged += BuyUnit;
	}

	private void OnDisable()
	{
		_scaner.AreaScanned -= SendUnitForCollectResource;
		_coffers.CountChanged -= BuyUnit;
	}

	private void Start()
	{
		for (int i = 0; i < _startUnitsCount; i++)
			AttachUnit(_unitSpawner.GetUnit());

		UnitsChanged?.Invoke(_units.Count);
	}

	public void AttachUnit(Unit unit)
	{
		_units.Add(unit);
		UnitsChanged?.Invoke(_units.Count);
	}

	public void CollectResource(GameResource resource)
	{
		if (resource is Gold)
			_coffers.AddCoin();

		BookedResources.Remove(resource);
		resource.Delete();
	}

	public void SetFlag(Vector3 positionForFlag)
	{
		if (_units.Count < 2)
			return;

		_flag.gameObject.SetActive(true);
		_flag.transform.position = positionForFlag;
	}

	private void SendUnitForCollectResource(List<GameResource> findingResources)
	{
		SortFindingResources(findingResources);

		if (FreeResources.Count > _numberOfBase)
		{
			if (TrySetTargetForUnit(FreeResources[_numberOfBase].transform))
			{
				BookedResources.Add(FreeResources[_numberOfBase]);
				FreeResources.RemoveAt(_numberOfBase);
				return;
			}
			else
			{
				return;
			}
		}
		else
		{
			CollectUnitsAtTheBase();
		}
	}

	private bool TrySetTargetForUnit(Transform target)
	{
		bool isSendUnitForBuild = false;

		if (_flag.isActiveAndEnabled && _isUnitSendingToBuild == false && _coffers.TrySpendCoin(_BasePrice))
		{
			target = _flag.transform;
			isSendUnitForBuild = true;
		}

		foreach (var unit in _units)
		{
			if (!unit.IsBusy)
			{
				unit.SetTarget(target, true);

				if (isSendUnitForBuild)
				{
					unit.SetBuilder();
					unit.Builded += UntieUnit;
					_isUnitSendingToBuild = true;
				}

				return true;
			}
		}

		return false;
	}

	private void SortFindingResources(List<GameResource> findingResources)
	{
		if (findingResources.Count == 0)
			return;

		bool isResourceBooked = false;

		FreeResources.Clear();

		for (int i = 0; i < findingResources.Count; i++)
		{
			for (int j = 0; j < BookedResources.Count; j++)
			{
				if (findingResources[i] == BookedResources[j])
				{
					isResourceBooked = true;
					break;
				}
				else
				{
					isResourceBooked = false;
				}
			}

			if (isResourceBooked == false)
			{
				FreeResources.Add(findingResources[i]);
			}
		}
	}

	private void UntieUnit(Unit unit)
	{
		_flag.gameObject.SetActive(false);

		unit.Builded -= UntieUnit;
		_units.Remove(unit);

		_isUnitSendingToBuild = false;
		UnitsChanged?.Invoke(_units.Count);
	}

	private void CollectUnitsAtTheBase()
	{
		foreach (var unit in _units)
			if (unit.IsBusy == false)
				if (unit.IsEmptyBackPack)
					unit.SetTarget(CollectionPoint, false);
				else
					unit.SetTarget(transform, false);
	}

	private void BuyUnit()
	{
		if (_flag.isActiveAndEnabled)
		{
			return;
		}

		if (_coffers.TrySpendCoin(_unitPrice))
		{
			_units.Add(_unitSpawner.GetUnit());

			UnitsChanged?.Invoke(_units.Count);
		}
	}
}
