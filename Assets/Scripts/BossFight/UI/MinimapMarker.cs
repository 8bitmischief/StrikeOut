using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class MinimapMarker : Entity
	{
		private static readonly Vector3 Offset = new Vector3(0f, 0f, -2f);
		private static readonly float Scale = 0.1f;
		private static readonly float RotationSpeed = 60f;

		[SerializeField] private Entity _target;

		public override void Render()
		{
			if (_target != null)
				transform.localPosition = _target.transform.position * Scale + Offset;
			transform.rotation *= Quaternion.Euler(Vector3.up * RotationSpeed * Time.deltaTime);
		}
	}
}
