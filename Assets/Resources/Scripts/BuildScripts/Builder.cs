using UnityEngine;

public class Builder : MonoBehaviour
{
	[SerializeField] private BuldPeview _preview;
	[SerializeField] private InputHandler _input;

	private Base _selectedBase;

	private bool _isBuildModeEnable = false;

	private void OnEnable()
	{
		_input.OnBaseClicked += EnableBuildMode;
		_input.EscButtonDowned += ExitBuildMode;
	}

	private void OnDisable()
	{
		_input.OnBaseClicked -= EnableBuildMode;
		_input.EscButtonDowned -= ExitBuildMode;
	}

	private void FixedUpdate()
	{
		if (_isBuildModeEnable)
			BuildMode();
	}

	private void BuildMode()
	{
		if (_input.IsRaycastHit)
		{
			_preview.gameObject.SetActive(true);
			_preview.transform.position = new Vector3(_input.HitPointPosition.x, _preview.transform.position.y, _input.HitPointPosition.z);
		}
		else
		{
			_preview.Hide();
		}
	}

	private void EnableBuildMode(Base selectedBase)
	{
		if (_selectedBase != null)
		{
			_input.OnGroundClicked -= SetFlagForBuild;
		}

		_input.OnGroundClicked += SetFlagForBuild;
		_selectedBase = selectedBase;

		_isBuildModeEnable = true;
	}

	private void ExitBuildMode()
	{
		_input.OnGroundClicked -= SetFlagForBuild;
		_preview.Hide();
		_isBuildModeEnable = false;

		_selectedBase = null;
	}

	private void SetFlagForBuild(Vector3 positionForBuild)
	{
		if (_preview.enabled && _preview.IsBuildAgreed)
		{
			_selectedBase.SetFlag(positionForBuild);
			ExitBuildMode();
		}
	}
}
