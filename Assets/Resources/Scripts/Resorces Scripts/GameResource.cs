using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class GameResource : MonoBehaviour, ISpawnable<GameResource>
{
	public event Action<GameResource> WorkEnded;

	public void SetDefault()
	{
		gameObject.SetActive(true);
		gameObject.GetComponent<Collider>().enabled = true;

		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	public void Delete()
	{
		WorkEnded?.Invoke(this);
	}
}
