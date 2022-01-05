using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, string>
	{
		[Tooltip("The fastest a swing can possibly be performed, in frames")]
		[SerializeField] private int _fastestSwingStartupFrames = 2;
		[Tooltip("The number of frames a swing normally takes to perform")]
		[SerializeField] private int _defaultSwingStartupFrames = 4;
		[Tooltip("The slowest a swing can possibly be performed, in frames")]
		[SerializeField] private int _slowestSwingStartupFrames = 8;
		private SwingDirection _swingDirection { get => (SwingDirection) animator.GetInteger("Swing Direction"); set => animator.SetInteger("Swing Direction", (int) value); }
		private int _swingStartupFrames { get => animator.GetInteger("Swing Startup Frames"); set => animator.SetInteger("Swing Startup Frames", value); }

		public int fastestSwingStartupFrames => _fastestSwingStartupFrames;
		public int defaultSwingStartupFrames => _defaultSwingStartupFrames;
		public int slowestSwingStartupFrames => _slowestSwingStartupFrames;

		public void Swing(SwingDirection direction, int startupFrames)
		{
			_swingDirection = direction;
			_swingStartupFrames = startupFrames;
			Trigger("Swing");
		}

		public void SwitchSides() => Trigger("Switch Sides");

		public void SideStep() => Trigger("Side Step");

		public void EndSideStep() => Trigger("End Side Step");

		public void Damage() => Trigger("Damage");

		protected override void OnStartAnimation(string animation)
		{
			switch (animation)
			{
				case "Swing":
					if (_swingStartupFrames == _defaultSwingStartupFrames)
						animationSpeed = 1.00f;
					else
						animationSpeed = ((float) _defaultSwingStartupFrames) / ((float) _swingStartupFrames) + 0.01f;
					break;
			}
		}

		protected override void OnEndAnimation(string animation)
		{
			switch (animation)
			{
				case "Swing":
					animationSpeed = 1.00f;
					break;
			}
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