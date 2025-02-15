using UnityEngine;

public class UnitMover : MonoBehaviour
{
	[SerializeField] private float _speed;

	public void Move(Transform target)
	{
		float sqrGoalReachingDistance = 0.1f;
		float rotationSpeed = 10f;

		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = new Vector3(target.position.x, currentPosition.y, target.position.z);

		if ((targetPosition - currentPosition).sqrMagnitude > sqrGoalReachingDistance)
		{
			transform.position = Vector3.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);

			Quaternion targetRotation = Quaternion.LookRotation(targetPosition - currentPosition);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}
	}
}