using System;
using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class Hurtbox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;

		[Header("Hurtbox Config")]
		[SerializeField] private HitChannel _channel;
		private IHurtable _hurtableEntity = null;

		public HitChannel channel => _channel;
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		public event Action<Entity, Hitbox, Hurtbox> onHurt;

		private void OnEnable()
		{
			Scene.I.updateLoop.RegisterHurtbox(this);
		}

		private void OnDisable()
		{
			if (Scene.hasInstance)
				Scene.I.updateLoop.UnregisterHurtbox(this);
		}

		private void Start()
		{
			if (entity is IHurtable)
				_hurtableEntity = entity as IHurtable;
		}

		public override void UpdateState()
		{
			_collider.size = new Vector3(
				Mathf.Sign(transform.lossyScale.x),
				Mathf.Sign(transform.lossyScale.y),
				Mathf.Sign(transform.lossyScale.z));
		}

		public void OnHurt(Hitbox hitbox)
		{
			if (_hurtableEntity != null)
				_hurtableEntity.OnHurt(hitbox.entity, hitbox, this);
			onHurt?.Invoke(hitbox.entity, hitbox, this);
		}
	}
}