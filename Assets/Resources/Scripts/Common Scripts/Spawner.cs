using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable<T>
{
	[SerializeField] private T _prefab;

	private List<T> _allObjects;
	private Queue<T> _pulledObjects;

	public int ActiveObjects { get; private set; }

	private void Awake()
	{
		ActiveObjects = 0;

		_allObjects = new List<T>();
		_pulledObjects = new Queue<T>();
	}

	public T GetPrefab()
	{
		T prefab;

		if (_pulledObjects.Count == 0)
		{
			prefab = (Instantiate(_prefab));
			SetDefaultSettings(prefab);

			_allObjects.Add(prefab);
			ActiveObjects++;

			prefab.WorkEnded += PutPrefab;

			return prefab;
		}

		prefab = _pulledObjects.Dequeue();
		prefab.WorkEnded += PutPrefab;

		SetDefaultSettings(prefab);
		ActiveObjects++;

		return prefab;
	}

	public void PutPrefab(T prefab)
	{
		_pulledObjects.Enqueue(prefab);
		ActiveObjects--;

		prefab.WorkEnded -= PutPrefab;
		prefab.gameObject.SetActive(false);
	}

	public abstract void SetDefaultSettings(T prefab);
}
