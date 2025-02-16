using UnityEngine;
using TMPro;
using System;

public class Stats : MonoBehaviour
{
	[SerializeField] private Base _base;
	[SerializeField] private Coffers _coffers;

	[SerializeField] private TextMeshProUGUI _goldCount;
	[SerializeField] private TextMeshProUGUI _unitsCount;

	private void Start()
	{
		_goldCount.text = "0";
		_unitsCount.text = $"{_base.StartUnitCount}";

		Vector3 flipVector = new Vector3(0f, 180f, 0f);
		transform.LookAt(Camera.main.transform);
		transform.Rotate(flipVector);
	}

	private void OnEnable()
	{
		_coffers.CountChanged += ChangeGoldCount;
		_base.UnitsChanged += ChangeUnitCount;
	}

	private void OnDisable()
	{
		_coffers.CountChanged -= ChangeGoldCount;
		_base.UnitsChanged -= ChangeUnitCount;
	}

	private void ChangeGoldCount()
	{
		ShowCurrentInfo(_coffers.GoldCount, _goldCount);
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
