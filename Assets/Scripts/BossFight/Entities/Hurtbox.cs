using System;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoxCollider))]
	public class Hurtbox : EntityComponent
	{
		private BoxCollider _collider;
		private IHurtable _hurtableEntity = null;

		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		public event Action<Entity, Hitbox, Hurtbox> onHurt;

		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
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