using UnityEngine;

public class Game : MonoBehaviour
{
	[SerializeField] private ResourcesSpawner _resourcesSpawner;

	private void Start()
	{
		StartCoroutine(_resourcesSpawner.StartSpawnResources());
	}
}
