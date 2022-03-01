using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<BallAnimator, string>
	{
		[SerializeField] private EnemyHurtbox _hurtbox;

		public Vector2 target => _hurtbox.target;

		public override void ResetComponent()
		{
			_hurtbox.target = Vector2.zero;
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

		public void Pitch(PitchType pitchType, Vector2 target)
		{
			_hurtbox.target = target;
			animator.Pitch(pitchType, Scene.I.locations.GetStrikeZonePosition(target), true);
		}

		public void Hit(Vector3 target)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
			animator.Hit(target);
		}
	}
}