using System;
using UnityEngine;

public class Coffers : MonoBehaviour
{
	private int _goldCount;

	public event Action CountChangeng;

	public int GoldCount => _goldCount;

	private void Awake()
	{
		_goldCount = 0;
	}

	public void AddCoin()
	{
		_goldCount++;
		CountChangeng?.Invoke();
	}

	public bool TrySpendCoin(int value)
	{
		if (value <= _goldCount)
			_goldCount -= value;
		else
			return false;

		CountChangeng?.Invoke();
		return true;
	}
}
