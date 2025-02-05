using UnityEngine;

public class UnitSpawner : Spawner<Unit>
{
	[SerializeField] private Base _base;

	public override void SetDefaultSettings(Unit prefab)
	{
		prefab.SetDefaultSettings(_base, _base.InstantiatingPoint);
	}
}
