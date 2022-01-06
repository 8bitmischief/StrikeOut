using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BatterAnimator))]
	public partial class Batter : AnimatedEntity<BatterAnimator, string>, IBatterHittable, IBatterHurtable, IBatterPredictedHurtable
	{
		public int health => _health;
		public int lives => _lives;
		public bool isOnRightSide => transform.localScale.x < 0f;

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
					if (isOnRightSide)
						return CanSwitchSides() || CanEndSideStep();
					else
						return CanSideStep();
				case Direction.Right:
					if (isOnRightSide)
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
				case "Swing":
					return (animator.CanCancelAnimation(1) && _didSwingHit) ||
						animator.CanCancelAnimation(4);
				case "Hurt":
					return _wasAbleToSwitchSidesPriorToBeingHurt && animationFrame <= _dodgeLateForgivenessFrames;
				default:
					return animator.CanCancelAnimation(4);
			}
		}

		public bool CanSideStep()
		{
			switch (animation)
			{
				case "Swing":
					return (animator.CanCancelAnimation(1) && _didSwingHit) ||
						animator.CanCancelAnimation(4);
				case "Hurt":
					return _wasAbleToSideStepPriorToBeingHurt && animationFrame <= _dodgeLateForgivenessFrames;
				default:
					return animator.CanCancelAnimation(4);
			}
		}

		public bool CanEndSideStep()
		{
			return animation == "Side Step Start" &&
				animator.CanCancelAnimation(1);
		}

		public void Swing(StrikeZone strikeZone)
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
			int swingStartupFrames;
			if (targetHurtbox != null && targetHurtbox.entity is Ball)
			{
				Ball ball = targetHurtbox.entity as Ball;
				int battingLineOffset;
				if (ball.hasPassedBattingLine)
					battingLineOffset = -ball.framesSincePassedBattingLine;
				else if (ball.willPassBattingLine)
					battingLineOffset = ball.framesUntilPassBattingLine;
				else
					battingLineOffset = 0;
				int swingOffset = battingLineOffset - animator.defaultSwingStartupFrames;
				if (swingOffset > 0)
					swingOffset = Mathf.FloorToInt(swingOffset / 2);
				else
					swingOffset = Mathf.CeilToInt(swingOffset / 2);
				int idealSwingStartupFrames = animator.defaultSwingStartupFrames + swingOffset;
				swingStartupFrames = Mathf.Clamp(idealSwingStartupFrames, fastestSwingStartupFrames, slowestSwingStartupFrames);
			}
			else
			{
				if (slowestSwingStartupFrames != -1 && slowestSwingStartupFrames < animator.defaultSwingStartupFrames)
					swingStartupFrames = slowestSwingStartupFrames;
				else if (fastestSwingStartupFrames != -1 && fastestSwingStartupFrames > animator.defaultSwingStartupFrames)
					swingStartupFrames = fastestSwingStartupFrames;
				else
					swingStartupFrames = animator.defaultSwingStartupFrames;
			}
			_didSwingHit = false;
			if (targetHurtbox != null)
				Debug.Log($"Swing must start up between {fastestSwingStartupFrames} and {slowestSwingStartupFrames} frames to hit {targetHurtbox.entity.name}, choosing {swingStartupFrames}");
			animator.Swing(swingDirection, swingStartupFrames);
		}

		public void Dodge(Direction direction)
		{
			if (isOnRightSide == (direction == Direction.Right))
				SideStep();
			else if (animation == "Side Step Start")
				EndSideStep();
			else
				SwitchSides();
		}

		public void SwitchSides()
		{
			_wasAbleToSwitchSidesPriorToBeingHurt = false;
			_wasAbleToSideStepPriorToBeingHurt = false;
			if (animation == "Hurt" && Scene.I.hitDetectionManager.GetHitboxesThatHit(BatterArea.Center).Count > 0)
				return;
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			bool quickly = _quickDodgeFrames > 0 &&
				Scene.I.hitDetectionManager.DoAnyHitboxesHit(_hurtbox.area, 0, _quickDodgeFrames - 1);
			animator.SwitchSides(quickly);
		}

		public void SideStep()
		{
			_wasAbleToSwitchSidesPriorToBeingHurt = false;
			_wasAbleToSideStepPriorToBeingHurt = false;
			if (animation == "Hurt" && Scene.I.hitDetectionManager.GetHitboxesThatHit(isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft).Count > 0)
				return;
			bool quickly = _quickDodgeFrames > 0 &&
				Scene.I.hitDetectionManager.DoAnyHitboxesHit(_hurtbox.area, 0, _quickDodgeFrames - 1);
			animator.SideStep(quickly);
		}

		public void EndSideStep()
		{
			animator.EndSideStep();
		}
	}
}