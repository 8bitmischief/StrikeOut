using System;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class Hurtbox : EntityComponent
	{
		private IHurtable _hurtableEntity = null;

		public event Action<Entity, Hitbox, Hurtbox> onHurt;

		private void Start()
		{
			if (entity is IHurtable)
				_hurtableEntity = entity as IHurtable;
		}

		public override void UpdateState()
		{
			
		}

		public void OnHurt(Hitbox hitbox)
		{
			if (_hurtableEntity != null)
				_hurtableEntity.OnHurt(entity, hitbox, this);
			onHurt?.Invoke(entity, hitbox, this);
		}
	}
}