using System;
using UnityEngine;

public class Coffers : MonoBehaviour
{
	private int _goldCount;

	public event Action CountChanged;

	public int GoldCount => _goldCount;

	private void Awake()
	{
		_goldCount = 0;
	}

	public void AddCoin()
	{
		_goldCount++;
		CountChanged?.Invoke();
	}

	public bool TrySpendCoin(int value)
	{
		if (value <= _goldCount)
			_goldCount -= value;
		else
			return false;

		CountChanged?.Invoke();

		return true;
	}
}
