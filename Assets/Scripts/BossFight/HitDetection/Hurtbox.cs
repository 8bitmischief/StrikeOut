using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public abstract class Hurtbox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;

		[Header("Hurtbox Config")]
		[SerializeField] private HitChannel _channel;

		public HitChannel channel => _channel;
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