using System;
using UnityEngine;

public interface ISpawnable<T> where T : MonoBehaviour
{
	public event Action<T> WorkEnded;
}
