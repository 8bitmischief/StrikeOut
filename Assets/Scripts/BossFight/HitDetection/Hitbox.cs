using System;
using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class Hitbox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;

		[Header("Hitbox Config")]
		[SerializeField] private bool _requireOverlap = false;
		[SerializeField] private HitBehaviour _hitBehaviour = HitBehaviour.OneHitPerEntity;
		[SerializeField] private List<HitChannel> _channels;
		private IHittable _hittableEntity = null;
		private HashSet<Entity> _hitEntities = new HashSet<Entity>();
		private HashSet<Hurtbox> _overlappingHurtboxes = new HashSet<Hurtbox>();

		public List<HitChannel> channels => _channels;
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		public event Action<Entity, Hitbox, Hurtbox> onHit;

		private void Start()
		{
			if (entity is IHittable)
				_hittableEntity = entity as IHittable;
		}

		protected virtual void OnEnable()
		{
			_hitEntities.Clear();
			_overlappingHurtboxes.Clear();
			Scene.I.updateLoop.RegisterHitbox(this);
		}

		private void OnDisable()
		{
			if (Scene.hasInstance)
				Scene.I.updateLoop.UnregisterHitbox(this);
		}

		public override void UpdateState()
		{
			_collider.size = new Vector3(
				Mathf.Sign(transform.lossyScale.x),
				Mathf.Sign(transform.lossyScale.y),
				Mathf.Sign(transform.lossyScale.z));
			_overlappingHurtboxes.Clear();
		}

		public virtual bool IsHitting(Hurtbox hurtbox)
		{
			if (_requireOverlap && !IsOverlapping(hurtbox))
				return false;
			else if (_hitBehaviour == HitBehaviour.OneHitPerEntity && HasAlreadyHit(hurtbox.entity))
				return false;
			else
				return hurtbox.channel == HitChannel.None || _channels.Contains(hurtbox.channel);
		}

		public void OnHit(Hurtbox hurtbox)
		{
			if (!_hitEntities.Contains(hurtbox.entity))
				_hitEntities.Add(hurtbox.entity);
			if (_hittableEntity != null)
				_hittableEntity.OnHit(hurtbox.entity, this, hurtbox);
			onHit?.Invoke(hurtbox.entity, this, hurtbox);
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