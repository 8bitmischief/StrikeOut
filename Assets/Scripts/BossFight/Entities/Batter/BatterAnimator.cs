using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, string>
	{
		public void Swing(SwingDirection direction)
		{
			animator.SetInteger("Swing Direction", (int) direction);
			Trigger("Swing");
		}

		public void SwitchSides() => Trigger("Switch Sides");

		public void SideStep() => Trigger("Side Step");

		public void EndSideStep() => Trigger("End Side Step");

		public void Damage() => Trigger("Damage");

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