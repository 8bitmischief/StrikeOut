using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public abstract class Hitbox : HitDetectionBox
	{
		[Header("Hitbox Config")]
		[SerializeField] private bool _requireOverlap = false;
		[SerializeField] private HitBehaviour _hitBehaviour = HitBehaviour.OneHitPerEntity;
		private HashSet<Entity> _hitEntities = new HashSet<Entity>();
		private HashSet<Hurtbox> _overlappingHurtboxes = new HashSet<Hurtbox>();

		public override void UpdateState()
		{
			base.UpdateState();
			_overlappingHurtboxes.Clear();
		}

		public virtual bool IsHitting(Hurtbox hurtbox)
		{
			// Check if the hitbox is overlapping the hurtbox
			if (_requireOverlap && !IsOverlapping(hurtbox))
				return false;
			// Check if the entity has already been hit by this hitbox
			else if (_hitBehaviour == HitBehaviour.OneHitPerEntity && HasAlreadyHit(hurtbox.entity))
				return false;
			else
				return true;
		}

		protected override void Register()
		{
			_hitEntities.Clear();
			_overlappingHurtboxes.Clear();
		}

		protected void OnHit(Hurtbox hurtbox)
		{
			_hitEntities.Add(hurtbox.entity);
		}

		private bool IsOverlapping(Hurtbox hurtbox)
		{
			return _overlappingHurtboxes.Contains(hurtbox);
		}

		private bool HasAlreadyHit(Entity entity)
		{
			return _hitEntities.Contains(entity);
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out Hurtbox hurtbox))
				_overlappingHurtboxes.Add(hurtbox);
		}

		private enum HitBehaviour
		{
			None = 0,
			Default = 1,
			OneHitPerEntity = 2
		}
	}
}