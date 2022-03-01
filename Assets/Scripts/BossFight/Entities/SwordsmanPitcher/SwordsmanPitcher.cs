using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcherAnimator))]
	public class SwordsmanPitcher : AnimatedEntity<SwordsmanPitcherAnimator, string>, IEnemyHurtable
	{
		[Header("Pitcher Config")]
		[SerializeField] private BallSpawner _ballSpawner;

		public bool isIdle => animation == "Idle";
		public float idleTime => animation == "Idle" ? totalAnimationTime : 0f;

		public override void OnSpawn()
		{
			Scene.I.entityManager.pitcher = this;
		}

		public override void OnDespawn()
		{
			if (Scene.I.entityManager.pitcher == this)
				Scene.I.entityManager.pitcher = null;
		}

		public void TeleportSlash() => animator.TeleportSlash();

		public void Pitch(PitchType pitchType, StrikeZone strikeZone)
		{
			_ballSpawner.pitchType = pitchType;
			_ballSpawner.strikeZone = strikeZone;
			animator.Pitch();
		}

		public bool CanCancelAnimation(int cancelLevel) => animator.CanCancelAnimation(cancelLevel);

		public void OnHurt(BatterHitRecord hit)
		{
			animator.Hurt();
		}

		protected override void OnStartAnimation(string animation)
		{
			switch (animation)
			{
				case "Idle":
					transform.position = Scene.I.locations.pitchersMound;
					break;
				case "Teleport Slash Strike":
					transform.position = Scene.I.locations.inFrontOfBatter.center;
					break;
			}
		}
	}
}