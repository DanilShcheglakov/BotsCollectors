using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable<T>
{
	[SerializeField] private T Prefab;

	private List<T> AllObjects;
	private Queue<T> PulledObjects;

	public int ActiveObjects { get; private set; }

	private void Awake()
	{
		ActiveObjects = 0;

		AllObjects = new List<T>();
		PulledObjects = new Queue<T>();
	}

	public T GetPrefab()
	{
		T prefab;

		if (PulledObjects.Count == 0)
		{
			prefab = (Instantiate(Prefab));
			SetDefaultSettings(prefab);

			AllObjects.Add(prefab);
			ActiveObjects++;

			prefab.WorkEnded += PutPrefab;

			return prefab;
		}

		prefab = PulledObjects.Dequeue();
		prefab.WorkEnded += PutPrefab;

		SetDefaultSettings(prefab);
		ActiveObjects++;

		return prefab;
	}

	public void PutPrefab(T prefab)
	{
		PulledObjects.Enqueue(prefab);
		ActiveObjects--;

		prefab.WorkEnded -= PutPrefab;
		prefab.gameObject.SetActive(false);
	}

	public abstract void SetDefaultSettings(T prefab);
}
