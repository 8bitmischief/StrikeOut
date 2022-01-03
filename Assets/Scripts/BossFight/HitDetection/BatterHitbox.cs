using System;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public class BatterHitbox : Hitbox
	{
		[Header("Strike Zones")]
		[SerializeField] private bool _hitsNorth;
		[SerializeField] private bool _hitsEast;
		[SerializeField] private bool _hitsSouth;
		[SerializeField] private bool _hitsWest;
		private IBatterHittable _hittableEntity = null;

		public bool hitsNorth => _hitsNorth;
		public bool hitsEast => entity.transform.localScale.x >= 0f ? _hitsEast : _hitsWest;
		public bool hitsSouth => _hitsSouth;
		public bool hitsWest => entity.transform.localScale.x >= 0f ? _hitsWest : _hitsEast;

		public event Action<Entity, BatterHitbox, EnemyHurtbox> onHit;

		private void Start()
		{
			if (entity is IBatterHittable)
				_hittableEntity = entity as IBatterHittable;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			Scene.I.hitDetectionManager.RegisterHitbox(this);
		}

		private void OnDisable()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHitbox(this);
		}

		public bool IsHitting(EnemyHurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox))
			{
				return hitsNorth && hurtbox.hitByNorth ||
					hitsEast && hurtbox.hitByEast ||
					hitsSouth && hurtbox.hitBySouth ||
					hitsWest && hurtbox.hitByWest;
			}
			else
			{
				return false;
			}
		}

		public void OnHit(EnemyHurtbox hurtbox)
		{
			base.OnHit(hurtbox);
			if (_hittableEntity != null)
				_hittableEntity.OnHit(hurtbox.entity, this, hurtbox);
			onHit?.Invoke(hurtbox.entity, this, hurtbox);
		}
	}
}