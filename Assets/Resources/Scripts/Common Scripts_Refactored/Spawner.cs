using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable<T>
{
	[SerializeField] protected T Prefab;

	protected List<T> AllPrefabs;
	protected Queue<T> Prefabs;

	public int ActiveObjects { get; private set; }

	private void Awake()
	{
		ActiveObjects = 0;

		AllPrefabs = new List<T>();
		Prefabs = new Queue<T>();
	}

	public T GetPrefab()
	{
		T prefab;

		if (Prefabs.Count == 0)
		{
			prefab = (Instantiate(Prefab));
			SetDefaultSettings(prefab);

			AllPrefabs.Add(prefab);
			ActiveObjects++;

			prefab.WorkEnded += PutPrefab;

			return prefab;
		}

		prefab = Prefabs.Dequeue();
		prefab.WorkEnded += PutPrefab;

		SetDefaultSettings(prefab);
		ActiveObjects++;

		return prefab;
	}

	public void PutPrefab(T prefab)
	{
		Prefabs.Enqueue(prefab);
		ActiveObjects--;

		prefab.WorkEnded -= PutPrefab;
		prefab.gameObject.SetActive(false);
	}

	public abstract void SetDefaultSettings(T prefab);
}
