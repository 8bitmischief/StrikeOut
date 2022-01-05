using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, string>
	{
		[Header("Batter Config")]
		[SerializeField] private int _cancelAnimationLevel = 0;

		public void Swing(SwingDirection direction)
		{
			animator.SetInteger("Swing Direction", (int) direction);
			Trigger("Swing");
		}

		public void SwitchSides() => Trigger("Switch Sides");

		public void SideStep() => Trigger("Side Step");

		public void EndSideStep() => Trigger("End Side Step");

		public void Damage() => Trigger("Damage");

		public bool CanCancelAnimation(int cancelLevel = 1)
		{
			return animation == "Idle" || _cancelAnimationLevel >= cancelLevel;
		}

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