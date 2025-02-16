using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	private Ray _ray;
	private RaycastHit _raycastHit;

	public event Action<Base> OnBaseClicked;
	public event Action<Vector3> OnGroundClicked;
	public event Action EscButtonDowned;

	public Vector3 HitPointPosition => _raycastHit.point;
	public bool IsRaycastHit => Physics.Raycast(_ray, out _raycastHit);

	private void LateUpdate()
	{
		_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _raycastHit))
			if (_raycastHit.collider.gameObject.TryGetComponent<Base>(out Base clickedBase))
				OnBaseClicked?.Invoke(clickedBase);
			else if (_raycastHit.collider.gameObject.TryGetComponent<Ground>(out _))
				OnGroundClicked?.Invoke(_raycastHit.point);

		if (Input.GetKeyDown(KeyCode.Escape))
			EscButtonDowned?.Invoke();
	}
}
