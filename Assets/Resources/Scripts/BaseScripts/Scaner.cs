using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Scaner : MonoBehaviour
{
	private Collider[] _collidersBuffer;
	private float _scanRadius = 20f;
	private float _scanDelay = 1;
	private WaitForSeconds _delay;

	public event Action<List<GameResource>> AreaScanned;

	private void Awake()
	{
		_collidersBuffer = new Collider[50];
		_delay = new WaitForSeconds(_scanDelay);
	}

	private void Start()
	{
		StartCoroutine(ScanArea());
	}

	private List<GameResource> GetResourcesList()
	{
		List<GameResource> findingResources = new List<GameResource>();

		int count = Physics.OverlapSphereNonAlloc(Vector3.zero, _scanRadius, _collidersBuffer);

		for (int i = 0; i < count; i++)
			if (_collidersBuffer[i].gameObject.TryGetComponent<GameResource>(out GameResource resource))
				findingResources.Add(resource);

		return findingResources;
	}

	private IEnumerator ScanArea()
	{
		while (enabled)
		{
			AreaScanned?.Invoke(GetResourcesList());
			yield return _delay;
		}
	}
}
