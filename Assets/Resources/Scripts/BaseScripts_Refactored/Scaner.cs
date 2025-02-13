using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
	private Collider[] _collidersBuffer;

	private void Awake()
	{
		_collidersBuffer = new Collider[50];
	}

	public List<GameResource> ScanArea()
	{
		List<GameResource> _findingResources = new List<GameResource>();

		int count = Physics.OverlapSphereNonAlloc(Vector3.zero, 20f, _collidersBuffer);

		for (int i = 0; i < count; i++)
			if (_collidersBuffer[i].gameObject.TryGetComponent<GameResource>(out GameResource resource))
				_findingResources.Add(resource);

		return _findingResources;
	}
}
