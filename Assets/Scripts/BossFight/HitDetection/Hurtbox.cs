using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public abstract class Hurtbox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;

		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		public override void UpdateState()
		{
			_collider.size = new Vector3(
				Mathf.Sign(transform.lossyScale.x),
				Mathf.Sign(transform.lossyScale.y),
				Mathf.Sign(transform.lossyScale.z));
		}
	}
}