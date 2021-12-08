using System;
using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class Hitbox : EntityComponent
	{
		private IHittable _hittableEntity = null;
		private List<Hurtbox> _touchedHurtboxes = new List<Hurtbox>();

		public event Action<Entity, Hitbox, Hurtbox> onHit;

		private void Start()
		{
			if (entity is IHittable)
				_hittableEntity = entity as IHittable;
		}

		public override void CheckInteractions()
		{
			foreach (Hurtbox hurtbox in _touchedHurtboxes)
			{
				OnHit(hurtbox);
				hurtbox.OnHurt(this);
			}
			_touchedHurtboxes.Clear();
		}

		public void OnHit(Hurtbox hurtbox)
		{
			if (_hittableEntity != null)
				_hittableEntity.OnHit(entity, this, hurtbox);
			onHit?.Invoke(entity, this, hurtbox);
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out Hurtbox hurtbox))
			{
				_touchedHurtboxes.Add(hurtbox);
			}
		}
	}
}