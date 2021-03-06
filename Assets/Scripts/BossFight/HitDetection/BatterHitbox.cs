using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterHitbox : Hitbox
	{
		private static BatterHitRecord ReusedHitRecord = new BatterHitRecord();

		[Header("Gizmo")]
		[SerializeField] private Color _color = Color.red;
		[Header("Hit")]
		[SerializeField] private StrikeZone _strikeZone;
		private IBatterHittable _hittableEntity;

		public event Action<BatterHitRecord> onHit;

		private void Start()
		{
			if (entity is IBatterHittable)
				_hittableEntity = entity as IBatterHittable;
		}

		public bool DoesHit(StrikeZone strikeZone)
		{
			return strikeZone == GetProperlyFlippedStrikeZone();
		}

		public BatterHitRecord CheckForHit(EnemyHurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox))
			{
				StrikeZone hitStrikeZone = GetProperlyFlippedStrikeZone();
				BatterHitResult result = hurtbox.GetHitResult(hitStrikeZone);
				if (result != BatterHitResult.None && result != BatterHitResult.Miss)
				{
					ReusedHitRecord.hitter = entity;
					ReusedHitRecord.hurtee = hurtbox.entity;
					ReusedHitRecord.hitbox = this;
					ReusedHitRecord.hurtbox = hurtbox;
					ReusedHitRecord.strikeZone = hitStrikeZone;
					ReusedHitRecord.result = result;
					return ReusedHitRecord;
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		public void OnHit(BatterHitRecord hit)
		{
			base.OnHit(hit.hurtbox);
			if (_hittableEntity != null)
				_hittableEntity.OnHit(hit);
			onHit?.Invoke(hit);
		}

		protected override void Register()
		{
			base.Register();
			Scene.I.hitDetectionManager.RegisterHitbox(this);
		}

		protected override void Unregister()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHitbox(this);
		}

		protected override void DrawGizmo()
		{
			Gizmos.color = new Color(_color.r, _color.g, _color.b, _color.a * 0.35f);
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.color = _color;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}

		private StrikeZone GetProperlyFlippedStrikeZone()
		{
			if (_strikeZone == StrikeZone.West && entity.transform.localScale.x < 0f)
				return StrikeZone.East;
			else if (_strikeZone == StrikeZone.East && entity.transform.localScale.x < 0f)
				return StrikeZone.West;
			else
				return _strikeZone;
		}
	}
}