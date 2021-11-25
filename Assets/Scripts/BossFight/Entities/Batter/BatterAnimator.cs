using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, Batter.State>
	{
		private static readonly int SwingHash = Animator.StringToHash("Swing");
		private static readonly int SwingDirectionHash = Animator.StringToHash("Swing Direction");
		private static readonly int SwingStartupFramesHash = Animator.StringToHash("Swing Startup Frames");
		private static readonly int SwitchSidesHash = Animator.StringToHash("Switch Sides");
		private static readonly int SideStepHash = Animator.StringToHash("Side Step");
		private static readonly int EndSideStepHash = Animator.StringToHash("End Side Step");

		[SerializeField] public int fastestSwingStartupFrames = 2;
		[SerializeField] public int defaultSwingStartupFrames = 4;
		[SerializeField] public int slowestSwingStartupFrames = 8;

		private int swingStartupFrames { get => animator.GetInteger(SwingStartupFramesHash); set => animator.SetInteger(SwingStartupFramesHash, value); }

		public event Action onAllowAnimationCancels;
		public event Action onTryHitBall;

		public void Swing(SwingDirection direction, int startupFrames)
		{
			animator.SetInteger(SwingDirectionHash, (int) direction);
			swingStartupFrames = startupFrames;
			Trigger(SwingHash);
		}

		public void SwitchSides() => Trigger(SwitchSidesHash);

		public void SideStep() => Trigger(SideStepHash);

		public void EndSideStep() => Trigger(EndSideStepHash);

		protected override void OnEnterState(Batter.State state)
		{
			switch (state)
			{
				case Batter.State.Swing:
					if (swingStartupFrames == defaultSwingStartupFrames)
					{
						animationSpeed = 1.00f;
					}
					else
					{
						animationSpeed = 0.01f + ((float) defaultSwingStartupFrames) / ((float) swingStartupFrames);
					}
					break;
			}
		}

		protected override void OnLeaveState(Batter.State state)
		{
			switch (state)
			{
				case Batter.State.Swing:
					animationSpeed = 1.00f;
					break;
			}
		}
		
		protected override void OnAnimationEvent(AnimationEvent evt)
		{
			switch (evt.stringParameter)
			{
				case "Allow Animation Cancels":
					onAllowAnimationCancels?.Invoke();
					break;
				case "Try Hit Ball":
					animationSpeed = 1.00f;
					onTryHitBall?.Invoke();
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