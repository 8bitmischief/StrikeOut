using UnityEngine;
using CameraShake;
using SharedUnityMischief.Effects;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BatterAnimator))]
	public partial class Batter : AnimatedEntity<BatterAnimator, string>, IBatterHittable, IBatterHurtable, IBatterPredictedHurtable
	{
		[Header("Batter Config")]
		[SerializeField] private BatterHurtbox _hurtbox;
		[SerializeField] private ParticleEffectSpawner _hitBallEffectSpawner;
		[SerializeField] private BounceShake.Params _hitBallShakeParams;
		[SerializeField] private int _quickDodgeFrames = 3;
		[SerializeField] private int _dodgeLateForgivenessFrames = 2;
		[SerializeField] private bool _wasAbleToSwitchSidesPriorToBeingHurt;
		[SerializeField] private bool _wasAbleToSideStepPriorToBeingHurt;
		private int _health = 3;
		private int _lives = 3;
		private bool _didSwingHit;

		public override void OnSpawn()
		{
			Scene.I.entityManager.batter = this;
		}

		public override void UpdateState()
		{
			if (!UpdateLoop.I.isInterpolating)
			{
				if (animation != "Hurt")
				{
					_wasAbleToSwitchSidesPriorToBeingHurt = CanSwitchSides();
					_wasAbleToSideStepPriorToBeingHurt = CanSideStep();
				}
			}
		}

		public override void LateUpdateState()
		{
			if (!UpdateLoop.I.isInterpolating)
			{
				if (animation == "Hurt" && animationFrame == _dodgeLateForgivenessFrames)
				{
					_health = Mathf.Max(0, _health - 1);
					if (_health == 0 && _lives > 0)
					{
						_health = 3;
						_lives--;
					}
				}
			}
		}

		public override void OnDespawn()
		{
			if (Scene.I.entityManager.batter == this)
				Scene.I.entityManager.batter = null;
		}

		public void OnHit(BatterHitRecord hit)
		{
			_didSwingHit = true;
			if (hit.hurtee is Ball)
			{
				Ball ball = hit.hurtee as Ball;
				Vector3 targetPosition = new Vector3(15f, 5f, 50f);
				Vector3 shakeDirection;
				if (ball.strikeZone == StrikeZone.North)
					shakeDirection = new Vector3(1f, 0.3f, 0f);
				else if (ball.strikeZone == StrikeZone.South)
					shakeDirection = new Vector3(1f, -0.3f, 0f);
				else
					shakeDirection = new Vector3(1f, 0f, 0f);
				if (isOnRightSide)
					shakeDirection.x *= -1;
				ball.Hit(targetPosition);
				CameraShaker.Shake(new BounceShake(_hitBallShakeParams, new Displacement(shakeDirection, new Vector3(0f, 0f, 1f))));
				_hitBallEffectSpawner.SpawnParticleEffect(new Vector3(ball.transform.position.x, ball.transform.position.y, 0f));
			}
		}

		public void OnHurt(EnemyHitRecord hit)
		{
			animator.Hurt();
		}

		public void OnPredictedHurt(EnemyHitRecord hit, int frames)
		{
			Debug.Log($"{hit.hurtee.name} is predicted to be hurt by {hit.hitter.name} in {frames} {(frames == 1 ? "frame" : "frames")}");
		}

		protected override void OnStartAnimation(string animation)
		{
			switch (animation)
			{
				case "Settle":
				case "Switch Sides":
				case "Side Step End":
					animator.SetRootMotion(isOnRightSide ?
						Scene.I.locations.batter.right :
						Scene.I.locations.batter.left, false);
					break;
				case "Side Step Start":
					animator.SetRootMotion(isOnRightSide ?
						Scene.I.locations.batter.farRight :
						Scene.I.locations.batter.farLeft, false);
					break;
			}
		}

		private BatterAnimator.SwingDirection CalculateSwingDirection(StrikeZone strikeZone)
		{
			switch (strikeZone)
			{
				case StrikeZone.North: return BatterAnimator.SwingDirection.North;
				case StrikeZone.South: return BatterAnimator.SwingDirection.South;
				case StrikeZone.East: return isOnRightSide ? BatterAnimator.SwingDirection.Inside : BatterAnimator.SwingDirection.Outside;
				case StrikeZone.West: return isOnRightSide ? BatterAnimator.SwingDirection.Outside : BatterAnimator.SwingDirection.Inside;
				default: return BatterAnimator.SwingDirection.None;
			}
		}
	}
}