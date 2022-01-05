using UnityEngine;
using SharedUnityMischief.Effects;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcherAnimator))]
	public class SwordsmanPitcher : AnimatedEntity<SwordsmanPitcherAnimator, string>
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

		public void Move(Location location) => animator.Move(Scene.I.locations[location]);

		public void Slash() => animator.Slash();

		public void EnterRaisedSwordStance() => animator.EnterRaisedSwordStance();

		public void MeleeDownwardSlash() => animator.MeleeDownwardSlash();

		public void Pitch(PitchType pitchType, StrikeZone strikeZone)
		{
			_ballSpawner.pitchType = pitchType;
			_ballSpawner.strikeZone = strikeZone;
			animator.Pitch();
		}

		public bool CanCancelAnimation(int cancelLevel) => animator.CanCancelAnimation(cancelLevel);
	}
}