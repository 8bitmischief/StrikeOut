using UnityEngine;
using CameraShake;
using SharedUnityMischief;
using SharedUnityMischief.Effects;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<BatterAnimator, string>, IBatterHittable, IBatterHurtable, IBatterPredictedHurtable
	{
		[Header("Batter Config")]
		[SerializeField] private BatterHurtbox _hurtbox;
		[SerializeField] private ParticleEffectSpawner _hitBallEffectSpawner;
		[SerializeField] private BounceShake.Params _hitBallShakeParams;
		private int _health = 3;
		private int _lives = 3;
		private bool _isOnRightSide = false;

		public int health => _health;
		public int lives => _lives;
		public BatterArea area => _hurtbox.area;
		public BatterArea destinationArea => _hurtbox.destinationArea;
		public bool isOnRightSide => _isOnRightSide;

		public override void OnSpawn()
		{
			Scene.I.entityManager.batter = this;
		}

		public override void OnDespawn()
		{
			if (Scene.I.entityManager.batter == this)
				Scene.I.entityManager.batter = null;
		}

		public bool CanSwing(StrikeZone strikeZone)
		{
			switch (animation)
			{
				default:
					return animator.CanCancelAnimation(4);
			}
		}

		public bool CanDodge(Direction direction)
		{
			switch (direction)
			{
				case Direction.Left:
					if (_isOnRightSide)
						return CanSwitchSides() || CanEndSideStep();
					else
						return CanSideStep();
				case Direction.Right:
					if (_isOnRightSide)
						return CanSideStep();
					else
						return CanSwitchSides() || CanEndSideStep();
				default:
					return false;
			}
		}

		public bool CanSwitchSides()
		{
			switch (animation)
			{
				case "Wince":
					return Scene.I.hitDetectionManager.GetHitboxesThatHit(BatterArea.Center).Count == 0;
				default:
					return animator.CanCancelAnimation(4);
			}
		}

		public bool CanSideStep()
		{
			switch (animation)
			{
				case "Wince":
					return Scene.I.hitDetectionManager.GetHitboxesThatHit(_isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft).Count == 0;
				default:
					return animator.CanCancelAnimation(4);
			}
		}
		
		public bool CanEndSideStep()
		{
			return animation == "Side Step Start" && animator.CanCancelAnimation(1);
		}

		public void Swing(StrikeZone strikeZone)
		{
			switch (strikeZone)
			{
				case StrikeZone.North:
					animator.Swing(BatterAnimator.SwingDirection.North);
					break;
				case StrikeZone.South:
					animator.Swing(BatterAnimator.SwingDirection.South);
					break;
				case StrikeZone.East:
					animator.Swing(_isOnRightSide ? BatterAnimator.SwingDirection.Inside : BatterAnimator.SwingDirection.Outside);
					break;
				case StrikeZone.West:
					animator.Swing(_isOnRightSide ? BatterAnimator.SwingDirection.Outside : BatterAnimator.SwingDirection.Inside);
					break;
			}
		}

		public void Dodge(Direction direction)
		{
			// Dodge outwards (side step)
			if (_isOnRightSide == (direction == Direction.Right))
			{
				SideStep();
			}
			// Dodge inwards (switch sides)
			else
			{
				if (animation == "Side Step Start")
					EndSideStep();
				else
					SwitchSides();
			}
		}

		public void SwitchSides()
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			_isOnRightSide = !_isOnRightSide;
			animator.SwitchSides();
		}

		public void SideStep()
		{
			animator.SideStep();
		}

		public void EndSideStep()
		{
			animator.EndSideStep();
		}

		public void OnHit(BatterHitRecord hit)
		{
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
				if (_isOnRightSide)
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
				case "Hitstun":
					_health = Mathf.Max(0, _health - 1);
					if (_health == 0 && _lives > 0)
					{
						_health = 3;
						_lives--;
					}
					break;
				case "Settle":
				case "Switch Sides":
				case "Side Step End":
					animator.SetRootMotion(_isOnRightSide ?
						Scene.I.locations.batter.right :
						Scene.I.locations.batter.left, false);
					break;
				case "Side Step Start":
					animator.SetRootMotion(_isOnRightSide ?
						Scene.I.locations.batter.farRight :
						Scene.I.locations.batter.farLeft, false);
					break;
			}
		}
	}
}