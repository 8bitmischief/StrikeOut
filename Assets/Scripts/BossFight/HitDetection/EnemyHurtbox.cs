using System;
using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public class EnemyHurtbox : Hurtbox
	{
		[SerializeField] private EnemyHurtResponse _response = EnemyHurtResponse.None;
		[Header("Strike Zones")]
		[SerializeField] private bool _hitByNorth;
		[SerializeField] private bool _hitByEast;
		[SerializeField] private bool _hitBySouth;
		[SerializeField] private bool _hitByWest;
		private IEnemyHurtable _hurtableEntity = null;

		public EnemyHurtResponse response => _response;
		public bool hitByNorth => _hitByNorth;
		public bool hitByEast => entity.transform.localScale.x >= 0f ? _hitByEast : _hitByWest;
		public bool hitBySouth => _hitBySouth;
		public bool hitByWest => entity.transform.localScale.x >= 0f ? _hitByWest : _hitByEast;

		public event Action<Entity, BatterHitbox, EnemyHurtbox> onHurt;

		private void Awake()
		{
			if (entity is IEnemyHurtable)
				_hurtableEntity = entity as IEnemyHurtable;
		}

		private void OnEnable()
		{
			Scene.I.hitDetectionManager.RegisterHurtbox(this);
		}

		private void OnDisable()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHurtbox(this);
		}

		public void OnHurt(BatterHitbox hitbox)
		{
			if (_hurtableEntity != null)
				_hurtableEntity.OnHurt(hitbox.entity, hitbox, this);
			onHurt?.Invoke(hitbox.entity, hitbox, this);
		}

		public enum EnemyHurtResponse
		{
			None = 0,
			Damage = 1,
			Ball = 2,
			Parry = 3,
			Armor = 4
		}
	}
}