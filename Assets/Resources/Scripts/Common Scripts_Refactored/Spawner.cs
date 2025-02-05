using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable<T>
{
	[SerializeField] protected T _prefab;

	protected List<T> _allPrefabs;
	protected Queue<T> _prefabs;

	public int ActiveObjects { get; private set; }

	private void Awake()
	{
		ActiveObjects = 0;

		_allPrefabs = new List<T>();
		_prefabs = new Queue<T>();
	}

	public T GetPrefab()
	{
		T newPrefab;

		if (_prefabs.Count == 0)
		{
			newPrefab = (Instantiate(_prefab));
			SetDefaultSettings(newPrefab);

			_allPrefabs.Add(newPrefab);
			ActiveObjects++;

			newPrefab.WorkEnding += PutPrefab;

			return newPrefab;
		}

		newPrefab = _prefabs.Dequeue();
		newPrefab.WorkEnding += PutPrefab;

		SetDefaultSettings(newPrefab);
		ActiveObjects++;

		return newPrefab;
	}

	public void PutPrefab(T prefab)
	{
		_prefabs.Enqueue(prefab);
		ActiveObjects--;

		prefab.WorkEnding -= PutPrefab;
		prefab.gameObject.SetActive(false);
	}

	public abstract void SetDefaultSettings(T prefab);
}
