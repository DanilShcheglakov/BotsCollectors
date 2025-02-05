using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
	public event Action<Collider> CollisionDetected;

	private void OnTriggerEnter(Collider other)
	{
		CollisionDetected?.Invoke(other);
	}
}
