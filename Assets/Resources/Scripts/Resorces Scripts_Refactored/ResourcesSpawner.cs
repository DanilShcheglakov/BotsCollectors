using System.Collections;
using UnityEngine;

public class ResourcesSpawner : Spawner<GameResource>
{
	[SerializeField] private int _resourcesCount;

	[SerializeField] private Transform _maxPointForSpawn;
	[SerializeField] private Transform _minPointForSpawn;

	private float _maxX;
	private float _minX;

	private float _maxZ;
	private float _minZ;

	private float _delayForSpawn;

	private WaitForSeconds _spawnDelay;

	private void OnValidate()
	{
		if (_maxPointForSpawn.position.x < _minPointForSpawn.position.x)
		{
			_maxX = _minPointForSpawn.position.x;
			_minX = _maxPointForSpawn.position.x;
		}
		else
		{
			_maxX = _maxPointForSpawn.position.x;
			_minX = _minPointForSpawn.position.x;
		}

		if (_maxPointForSpawn.position.z < _minPointForSpawn.position.z)
		{
			_maxZ = _minPointForSpawn.position.z;
			_minZ = _maxPointForSpawn.position.z;
		}
		else
		{
			_maxZ = _maxPointForSpawn.position.z;
			_minZ = _minPointForSpawn.position.z;
		}

		_delayForSpawn = 1f;
		_spawnDelay = new WaitForSeconds(_delayForSpawn);
	}

	public override void SetDefaultSettings(GameResource prefab)
	{
		prefab.SetDefault();
		prefab.transform.position = GetPosition();
	}

	public IEnumerator StartSpawnResources()
	{
		while (enabled)
		{
			if (_resourcesCount > ActiveObjects)
				GetPrefab();

			yield return _spawnDelay;
		}
	}

	private Vector3 GetPosition()
	{
		Vector3 spawnPoint = GetRandomVector();

		bool isVectorFinding = false;

		while (isVectorFinding == false)
		{
			Collider[] colliders = Physics.OverlapSphere(spawnPoint, 1.5f);

			foreach (var collider in colliders)
			{
				if (collider.gameObject.TryGetComponent(out Ground _))
				{
					isVectorFinding = true;
				}
				else
				{
					isVectorFinding = false;
					spawnPoint = GetRandomVector();
					break;
				}
			}
		}

		return spawnPoint;
	}

	private Vector3 GetRandomVector()
	{
		return new Vector3(Random.Range(_maxX, _minX), 0f, Random.Range(_maxZ, _minZ));
	}
}
