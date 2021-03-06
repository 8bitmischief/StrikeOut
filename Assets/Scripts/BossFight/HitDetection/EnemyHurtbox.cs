using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class EnemyHurtbox : Hurtbox
	{
		[Header("Gizmo")]
		[SerializeField] private Mesh _cubeQuadrant;
		[Header("Single Strike Zone")]
		[SerializeField] private StrikeZone _strikeZone;
		[SerializeField] private BatterHitResult _result;
		[Header("Multi Strike Zone")]
		[SerializeField] private BatterHitResult _north;
		[SerializeField] private BatterHitResult _east;
		[SerializeField] private BatterHitResult _south;
		[SerializeField] private BatterHitResult _west;
		private IEnemyHurtable _hurtableEntity;

		public StrikeZone strikeZone { get => _strikeZone; set => _strikeZone = value; }

		public event Action<BatterHitRecord> onHurt;

		private void Awake()
		{
			if (entity is IEnemyHurtable)
				_hurtableEntity = entity as IEnemyHurtable;
		}

		public bool IsHurtBy(StrikeZone strikeZone)
		{
			BatterHitResult result = GetHitResult(strikeZone);
			return result == BatterHitResult.Damage || result == BatterHitResult.Ball || result == BatterHitResult.Parry;
		}

		public BatterHitResult GetHitResult(StrikeZone strikeZone)
		{
			StrikeZone singleStrikeZone;
			if ((entity != null && entity.transform.localScale.x < 0) && _strikeZone == StrikeZone.East)
				singleStrikeZone = StrikeZone.West;
			else if ((entity != null && entity.transform.localScale.x < 0) && _strikeZone == StrikeZone.West)
				singleStrikeZone = StrikeZone.East;
			else
				singleStrikeZone = _strikeZone;
			if (singleStrikeZone != StrikeZone.None && strikeZone == singleStrikeZone)
			{
				return _result;
			}
			else
			{
				switch (strikeZone)
				{
					case StrikeZone.North: return _north;
					case StrikeZone.East: return (entity != null && entity.transform.localScale.x < 0f) ? _west : _east;
					case StrikeZone.South: return _south;
					case StrikeZone.West: return (entity != null && entity.transform.localScale.x < 0f) ? _east : _west;
					default: return BatterHitResult.None;
				}
			}
		}

		public void OnHurt(BatterHitRecord hit)
		{
			if (_hurtableEntity != null)
				_hurtableEntity.OnHurt(hit);
			onHurt?.Invoke(hit);
		}

		protected override void Register()
		{
			Scene.I.hitDetectionManager.RegisterHurtbox(this);
		}

		protected override void Unregister()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHurtbox(this);
		}

		protected override void DrawGizmo()
		{
			Gizmos.color = ChooseGizmoColor(GetHitResult(StrikeZone.North));
			Gizmos.DrawMesh(_cubeQuadrant, Vector3.zero, Quaternion.identity, Vector3.one);

			Gizmos.color = ChooseGizmoColor(GetHitResult(StrikeZone.East));
			Gizmos.DrawMesh(_cubeQuadrant, Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);

			Gizmos.color = ChooseGizmoColor(GetHitResult(StrikeZone.South));
			Gizmos.DrawMesh(_cubeQuadrant, Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);

			Gizmos.color = ChooseGizmoColor(GetHitResult(StrikeZone.West));
			Gizmos.DrawMesh(_cubeQuadrant, Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
		}

		private Color ChooseGizmoColor(BatterHitResult result)
		{
			switch (result)
			{
				case BatterHitResult.Damage:
					return new Color(0f, 0f, 1f, 0.35f);
				case BatterHitResult.CriticalDamage:
					return new Color(0f, 0f, 1f, 0.55f);
				case BatterHitResult.Ball:
					return new Color(0f, 1f, 0f, 0.35f);
				case BatterHitResult.Parry:
					return new Color(1f, 1f, 0f, 0.35f);
				case BatterHitResult.Armor:
					return new Color(0.6f, 0.6f, 0.6f, 0.5f);
				default:
					return new Color(0f, 0f, 1f, 0.05f);
			}
		}
	}
}