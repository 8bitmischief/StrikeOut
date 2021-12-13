using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<PitcherAnimator, Pitcher.Animation>
	{
		public bool isIdle => animation == Animation.Idle;
		public float idleTime => animation == Animation.Idle ? totalAnimationTime : 0f;

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

		public void Pitch() => animator.Pitch();

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Move = 2,
			Pitch = 3,
			Chop = 4,
			ThrowBoomerang = 5,
		}
	}
}