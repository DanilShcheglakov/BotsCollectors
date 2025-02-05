using System;
using UnityEngine;

public abstract class GameResource : MonoBehaviour, ISpawnable<GameResource>
{
	public bool IsBooked { get; private set; }

	public event Action<GameResource> WorkEnding;

	public void SetDefault()
	{
		gameObject.SetActive(true);
		gameObject.GetComponent<Collider>().enabled = true;

		IsBooked = false;

		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	public void Delete()
	{
		WorkEnding?.Invoke(this);
	}

	public void Booking()
	{
		IsBooked = true;
	}
}
