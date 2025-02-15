using System;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour
{
	[SerializeField] private int _count;

	private Vector3 _contentPosition;
	private Queue<GameResource> _resources;

	private float _resourceShift = 0.25f;

	public event Action Filled;
	public event Action Devastated;

	public bool IsThereResources => _resources.Count > 0;
	public bool IsFilled => _resources.Count == _count;

	private void Awake()
	{
		_resources = new Queue<GameResource>();
		_contentPosition = transform.position;
	}

	public void AddResource(GameResource newResource)
	{
		newResource.transform.position = _contentPosition;
		newResource.transform.SetParent(transform, false);
		newResource.gameObject.GetComponent<Collider>().enabled = false;

		_resources.Enqueue(newResource);

		if (_resources.Count == _count)
			Filled?.Invoke();

		_contentPosition.y += _resourceShift;
	}

	public GameResource GiveItem()
	{
		if (_resources.Count == 0)
		{
			Devastated?.Invoke();
			return null;
		}

		_contentPosition.y -= _resourceShift;

		GameResource gameResource = _resources.Dequeue();
		gameResource.transform.parent = null;

		return gameResource;
	}
}
