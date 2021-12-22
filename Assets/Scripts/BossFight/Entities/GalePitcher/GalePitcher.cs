using System;
using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(GalePitcherAnimator))]
	public class GalePitcher : AnimatedEntity<GalePitcherAnimator, string>, IHurtable, ISpawner
	{
		private PitchType _pitchedBallPitchType;
		private StrikeZone _pitchedBallStrikeZone;

		public bool isIdle => animation == "Idle";
		public float idleTime => animation == "Idle" ? totalAnimationTime : 0f;

		public event Action onParry;

		public override void OnSpawn()
		{
			Scene.I.entityManager.pitcher = this;
		}

		public override void OnDespawn()
		{
			if (Scene.I.entityManager.pitcher == this)
				Scene.I.entityManager.pitcher = null;
		}

		public void Move(Location location) => animator.Move(Scene.I.locations[location]);

		public void Chop() => animator.Chop();

		public void ThrowBoomerang(bool toTheRight) => animator.ThrowBoomerang(Scene.I.locations.pitchersMound + new Vector3(toTheRight ? 3f : -3f, 0f, 0f), toTheRight);

		public void Pitch(PitchType pitchType, StrikeZone strikeZone)
		{
			_pitchedBallPitchType = pitchType;
			_pitchedBallStrikeZone = strikeZone;
			bool includeWindup = pitchType == PitchType.Curveball;
			bool pitchToTheRight = strikeZone == StrikeZone.West;
			animator.Pitch(includeWindup, pitchToTheRight);
		}

		public void Slash(bool toTheRight) => animator.Slash(toTheRight);

		public void OnHurt(Entity entity, Hitbox hitbox, Hurtbox hurtbox)
		{
			animator.Parry();
			onParry?.Invoke();
		}

		public void OnSpawnChild(Entity entity)
		{
			if (entity is Ball)
				(entity as Ball).Pitch(_pitchedBallPitchType, _pitchedBallStrikeZone);
		}
	}
}