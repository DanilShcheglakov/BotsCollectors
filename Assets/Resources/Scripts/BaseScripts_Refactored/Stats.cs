using UnityEngine;
using TMPro;
using System;

public class Stats : MonoBehaviour
{
	[SerializeField] private Base _base;

	[SerializeField] private TextMeshProUGUI _goldCount;
	[SerializeField] private TextMeshProUGUI _unitsCount;

	private void Start()
	{
		_goldCount.text = "0";
		_unitsCount.text = $"{_base.StartUnitCount}";
	}

	private void OnEnable()
	{
		_base.GoldChanged += ChangeGoldCount;
		_base.UnitsChanged += ChangeUnitCount;
	}

	private void OnDisable()
	{
		_base.GoldChanged -= ChangeGoldCount;
		_base.UnitsChanged -= ChangeUnitCount;
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
