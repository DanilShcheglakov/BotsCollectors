using System.Collections.Generic;
using UnityEngine;

public class BuldPeview : MonoBehaviour
{
	[SerializeField] private Material _allowedMaterial;
	[SerializeField] private Material _prohibitedMaterial;

	private List<Collider> _conflictingCollising;
	private MeshRenderer _meshRenderer;

	public bool IsBuildAgreed => _conflictingCollising.Count == 0;

	private void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
		_conflictingCollising = new List<Collider>();

		SetActualMaterial();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<Ground>(out _) == false || other.gameObject.TryGetComponent<GameResource>(out _) == false)
			_conflictingCollising.Add(other);

		SetActualMaterial();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.TryGetComponent<Ground>(out _) == false || other.gameObject.TryGetComponent<GameResource>(out _) == false)
			_conflictingCollising.Remove(other);

		SetActualMaterial();
	}

	public void Hide()
	{
		_conflictingCollising.Clear();
		gameObject.SetActive(false);
	}

	private void SetActualMaterial()
	{
		if (IsBuildAgreed)
			_meshRenderer.material = _allowedMaterial;
		else
			_meshRenderer.material = _prohibitedMaterial;
	}
}
