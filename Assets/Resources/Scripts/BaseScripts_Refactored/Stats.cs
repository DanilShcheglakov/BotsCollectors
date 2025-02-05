using UnityEngine;
using TMPro;
using System;

public class Stats : MonoBehaviour
{
	[SerializeField] Base _base;

	[SerializeField] TextMeshProUGUI _goldCount;
	[SerializeField] TextMeshProUGUI _unitsCount;

	private void Start()
	{
		_goldCount.text = "0";
		_unitsCount.text = $"{_base.StartUnitCount}";
	}

	private void OnEnable()
	{
		_base.GoldChanges += ChangeGoldCount;
		_base.UnitsChanges += ChangeUnitCount;
	}

	private void OnDisable()
	{
		_base.GoldChanges -= ChangeGoldCount;
		_base.UnitsChanges -= ChangeUnitCount;
	}

	private void ChangeGoldCount(int currentCount)
	{
		ShowCurrentInfo(currentCount, _goldCount);
	}

	private void ChangeUnitCount(int currentCount)
	{
		ShowCurrentInfo(currentCount, _unitsCount);
	}

	private void ShowCurrentInfo(int currentCount, TextMeshProUGUI text)
	{
		text.text = Convert.ToString(currentCount);
	}
}
