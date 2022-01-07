using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<BallAnimator, string>
	{
		[SerializeField] private EnemyHurtbox _hurtbox;

		public StrikeZone strikeZone => _hurtbox.strikeZone;

		public override void ResetComponent()
		{
			_hurtbox.strikeZone = StrikeZone.None;
		}

		public override void OnSpawn()
		{
			Scene.I.entityManager.balls.Add(this);
		}

		public override void UpdateState()
		{
			if (!Scene.I.updateLoop.isInterpolating)
			{
				if ((animation == "Pitch" || animation == "Hit") && hasAnimationCompleted)
					DespawnEntity(this);
			}
		}

		public override void OnDespawn()
		{
			Scene.I.entityManager.balls.Remove(this);
		}

		public void Pitch(PitchType pitchType, StrikeZone strikeZone) => Pitch(pitchType, strikeZone, Scene.I.locations.strikeZone[strikeZone]);
		public void Pitch(PitchType pitchType, Vector3 target) => Pitch(pitchType, StrikeZone.None, target);
		private void Pitch(PitchType pitchType, StrikeZone strikeZone, Vector3 target)
		{
			_hurtbox.strikeZone = strikeZone;
			animator.Pitch(pitchType, target, strikeZone != StrikeZone.None);
		}

		public void Hit(Vector3 target)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
			animator.Hit(target);
		}
	}
}