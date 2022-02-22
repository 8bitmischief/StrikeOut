using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, string>
	{
		[Header("Batter Config")]
		[SerializeField] private int _cancelAnimationLevel = 0;
		/*
			Cancel Animation Level:
				0 = Cannot cancel into any other animation
				1-3 = Can cancel into certain specific animations
				4 = Can cancel into anything, cutting off some portion of the animation
				5 = Can cancel into anything, the rest of the animation is just cosmetic
		*/

		public void Swing(SwingDirection direction)
		{
			animator.SetInteger("Swing Direction", (int) direction);
			Trigger("Swing");
		}

		public void SwitchSides(bool quickly) => Trigger("Switch Sides");

		public void SideStep(bool quickly) => Trigger("Side Step");

		public void EndSideStep() => Trigger("End Side Step");

		public void Hurt() => Trigger("Hurt");

		public bool CanCancelAnimation(int cancelLevel = 4) => animation == "Idle" || _cancelAnimationLevel >= cancelLevel;
	
		public enum SwingDirection
		{
			None = 0,
			North = 1,
			Inside = 2,
			South = 3,
			Outside = 4
		}
	}
}