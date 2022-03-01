using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BatterAnimator))]
	public partial class Batter : AnimatedEntity<BatterAnimator, string>, IBatterHittable, IBatterHurtable
	{
		public int health => _health;
		public int lives => _lives;
		public bool isOnRightSide => transform.localScale.x < 0f;

		public bool CanSwing()
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

		public void Swing()
		{
			_didSwingHit = false;
			animator.Swing();
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

		public void SetAim(Vector2 aim)
		{
			Scene.I.entityManager.strikeZone.SetAim(aim);
		}

		public void SwitchSides()
		{
			_wasAbleToSwitchSidesPriorToBeingHurt = false;
			_wasAbleToSideStepPriorToBeingHurt = false;
			if (animation == "Hurt" && Scene.I.hitDetectionManager.DoAnyHitboxesHit(BatterArea.Center))
				return;
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			animator.SwitchSides();
		}

		public void SideStep()
		{
			_wasAbleToSwitchSidesPriorToBeingHurt = false;
			_wasAbleToSideStepPriorToBeingHurt = false;
			if (animation == "Hurt" && Scene.I.hitDetectionManager.DoAnyHitboxesHit(isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft))
				return;
			animator.SideStep();
		}

		public void EndSideStep()
		{
			animator.EndSideStep();
		}
	}
}