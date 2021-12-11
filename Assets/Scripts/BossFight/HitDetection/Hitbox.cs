using System;
using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoxCollider))]
	public class Hitbox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;

		[Header("Hitbox Config")]
		[SerializeField] private HitBehaviour _hitBehaviour = HitBehaviour.OneHitPerEntity; 
		private IHittable _hittableEntity = null;
		private HashSet<Entity> _hitEntities = new HashSet<Entity>();
		private List<Hurtbox> _touchedHurtboxes = new List<Hurtbox>();

		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		public event Action<Entity, Hitbox, Hurtbox> onHit;

		private void Start()
		{
			if (entity is IHittable)
				_hittableEntity = entity as IHittable;
		}

		private void OnEnable()
		{
			_hitEntities.Clear();
			_touchedHurtboxes.Clear();
		}

		public override void UpdateState()
		{
			_collider.size = new Vector3(
				Mathf.Sign(transform.lossyScale.x),
				Mathf.Sign(transform.lossyScale.y),
				Mathf.Sign(transform.lossyScale.z));
		}

		public override void CheckInteractions()
		{
			foreach (Hurtbox hurtbox in _touchedHurtboxes)
			{
				if (CanHit(hurtbox))
				{
					if (!_hitEntities.Contains(hurtbox.entity))
						_hitEntities.Add(hurtbox.entity);
					OnHit(hurtbox);
					hurtbox.OnHurt(this);
				}
			}
			_touchedHurtboxes.Clear();
		}

		public void OnHit(Hurtbox hurtbox)
		{
			if (_hittableEntity != null)
				_hittableEntity.OnHit(hurtbox.entity, this, hurtbox);
			onHit?.Invoke(hurtbox.entity, this, hurtbox);
		}

		protected virtual bool CanHit(Hurtbox hurtbox)
		{
			return _hitBehaviour != HitBehaviour.OneHitPerEntity || !_hitEntities.Contains(hurtbox.entity);
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out Hurtbox hurtbox))
			{
				_touchedHurtboxes.Add(hurtbox);
			}
		}

		private enum HitBehaviour
		{
			None = 0,
			Default = 1,
			OneHitPerEntity = 2
		}
	}
}