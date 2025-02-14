using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
	[SerializeField] private Base _base;
	[SerializeField] protected Unit _unitPrefab;

	public Unit GetUnit()
	{
		Unit newUnit;

		newUnit = (Instantiate(_unitPrefab));
		newUnit.SetDefaultSettings(_base, _base.InstantiatingPoint);

		return newUnit;
	}
}
