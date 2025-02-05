using UnityEngine;

public class Game : MonoBehaviour
{
	[SerializeField] ResourcesSpawner _resourcesSpawner;

	private void Start()
	{
		StartCoroutine(_resourcesSpawner.StartSpawnResources());
	}
}
