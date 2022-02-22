using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterHurtbox : Hurtbox
	{
		[Header("Gizmo")]
		[SerializeField] private Color _color = Color.blue;
		[Header("Areas")]
		[SerializeField] private RelativeBatterArea _area;
		[SerializeField] private RelativeBatterArea _destinationArea;
		private IBatterHurtable _hurtableEntity;

		public BatterArea area
		{
			get
			{
				bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
				switch (_area)
				{
					case RelativeBatterArea.FarSameSide: return isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft;
					case RelativeBatterArea.SameSide: return isOnRightSide ? BatterArea.Right : BatterArea.Left;
					case RelativeBatterArea.Center: return BatterArea.Center;
					case RelativeBatterArea.OppositeSide: return isOnRightSide ? BatterArea.Left : BatterArea.Right;
					case RelativeBatterArea.FarOppositeSide: return isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight;
					default: return BatterArea.None;
				}
			}
		}
		public BatterArea destinationArea
		{
			get
			{
				bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
				switch (_destinationArea)
				{
					case RelativeBatterArea.FarSameSide: return isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft;
					case RelativeBatterArea.SameSide: return isOnRightSide ? BatterArea.Right : BatterArea.Left;
					case RelativeBatterArea.Center: return BatterArea.Center;
					case RelativeBatterArea.OppositeSide: return isOnRightSide ? BatterArea.Left : BatterArea.Right;
					case RelativeBatterArea.FarOppositeSide: return isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight;
					default: return BatterArea.None;
				}
			}
		}

		public event Action<EnemyHitRecord> onHurt;

		private void Awake()
		{
			if (entity is IBatterHurtable)
				_hurtableEntity = entity as IBatterHurtable;
		}

		public bool IsHurtBy(BatterArea area)
		{
			return this.area == area;
		}

		public void OnHurt(EnemyHitRecord hit)
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
			Gizmos.color = new Color(_color.r, _color.g, _color.b, _color.a * 0.35f);
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.color = _color;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
	}
}