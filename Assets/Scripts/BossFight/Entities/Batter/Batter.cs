using System.Reflection;
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
		[SerializeField] private ParticleEffect _hitBallEffect;
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
				_hitBallEffect.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, 0f);
				_hitBallEffect.Play();
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

		private int CalculateSwingStartupFrames(StrikeZone strikeZone)
		{
			EnemyHurtbox targetHurtbox = null;
			int fastestSwingStartupFrames = -1;
			int slowestSwingStartupFrames = -1;
			foreach (EnemyHurtbox hurtbox in Scene.I.hitDetectionManager.enemyHurtboxes)
			{
				if (hurtbox.WillBeHurtBy(strikeZone, animator.fastestSwingStartupFrames, animator.slowestSwingStartupFrames))
				{
					int fastestSwingStartupFramesThatStillHits = hurtbox.isActive ?
						animator.fastestSwingStartupFrames :
						Mathf.Max(hurtbox.framesUntilActive, animator.fastestSwingStartupFrames);
					int slowestSwingStartupFramesThatStillHits = hurtbox.willBeInactive ?
						Mathf.Min(hurtbox.framesUntilInactive - 1, animator.slowestSwingStartupFrames) :
						animator.slowestSwingStartupFrames;
					if (fastestSwingStartupFrames == -1 || fastestSwingStartupFramesThatStillHits < fastestSwingStartupFrames)
					{
						fastestSwingStartupFrames = fastestSwingStartupFramesThatStillHits;
						targetHurtbox = hurtbox;
					}
					if (slowestSwingStartupFrames == -1 || slowestSwingStartupFramesThatStillHits < slowestSwingStartupFrames)
					{
						slowestSwingStartupFrames = slowestSwingStartupFramesThatStillHits;
						if (targetHurtbox == null)
							targetHurtbox = hurtbox;
					}
				}
			}
			BatterAnimator.SwingDirection swingDirection = CalculateSwingDirection(strikeZone);
			int swingFrames;
			string messageDetails = "";
			if (targetHurtbox != null && targetHurtbox.hasIdealHit)
			{
				int offset;
				if (targetHurtbox.framesSinceIdealhit >= 0)
					offset = -targetHurtbox.framesSinceIdealhit;
				else if (targetHurtbox.framesUntilIdealHit >= 0)
					offset = targetHurtbox.framesUntilIdealHit;
				else
					offset = animator.defaultSwingStartupFrames;
				int swingOffset = offset - animator.defaultSwingStartupFrames;
				int preferredSwingStartupFrames = animator.defaultSwingStartupFrames;
				if (swingOffset > 0)
					preferredSwingStartupFrames += Mathf.FloorToInt(swingOffset / 2);
				else
					preferredSwingStartupFrames += Mathf.CeilToInt(swingOffset / 2);
				swingFrames = Mathf.Clamp(preferredSwingStartupFrames, fastestSwingStartupFrames, slowestSwingStartupFrames);
				if (swingOffset > 0)
					messageDetails += $" (swung <color=green>-{swingOffset}</color> {(swingOffset == 1 ? "frame" : "frames")} early;";
				else if (swingOffset < 0)
					messageDetails += $" (swung <color=red>+{-swingOffset}</color> {(-swingOffset == 1 ? "frame" : "frames")} late;";
				else
					messageDetails += " (swung exactly on time;";
				if (swingFrames > offset)
					messageDetails += $" swing will land <color=red>+{swingFrames - offset}</color> {(swingFrames - offset == 1 ? "frame" : "frames")} after the ideal frame)";
				else if (swingFrames < offset)
					messageDetails += $" swing will land <color=green>-{offset - swingFrames}</color> {(offset - swingFrames == 1 ? "frame" : "frames")} before the ideal frame)";
				else
					messageDetails += " swing will land on the ideal frame)";
			}
			else
			{
				if (slowestSwingStartupFrames != -1 && slowestSwingStartupFrames < animator.defaultSwingStartupFrames)
					swingFrames = slowestSwingStartupFrames;
				else if (fastestSwingStartupFrames != -1 && fastestSwingStartupFrames > animator.defaultSwingStartupFrames)
					swingFrames = fastestSwingStartupFrames;
				else
					swingFrames = animator.defaultSwingStartupFrames;
			}
			string message = $"<color=white>{name}</color> swinging with <color=orange>{swingFrames}</color> frame startup";
			if (targetHurtbox != null)
				message += $" in order to hit <color=white>{targetHurtbox.entity.name}</color> within <color=orange>{fastestSwingStartupFrames}</color> to <color=orange>{slowestSwingStartupFrames}</color> frames";
			else
				message += $" at no target in particular";
			message += messageDetails;
			Debug.Log(message);
			return swingFrames;
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