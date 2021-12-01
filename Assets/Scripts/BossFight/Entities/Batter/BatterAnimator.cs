using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, Batter.Animation>
	{
		private static readonly int SwingHash = Animator.StringToHash("Swing");
		private static readonly int SwingDirectionHash = Animator.StringToHash("Swing Direction");
		private static readonly int SwingStartupFramesHash = Animator.StringToHash("Swing Startup Frames");
		private static readonly int SwitchSidesHash = Animator.StringToHash("Switch Sides");
		private static readonly int SideStepHash = Animator.StringToHash("Side Step");
		private static readonly int EndSideStepHash = Animator.StringToHash("End Side Step");

		[SerializeField] private int _fastestSwingStartupFrames = 2;
		[SerializeField] private int _defaultSwingStartupFrames = 4;
		[SerializeField] private int _slowestSwingStartupFrames = 8;
		private int _swingStartupFrames { get => animator.GetInteger(SwingStartupFramesHash); set => animator.SetInteger(SwingStartupFramesHash, value); }

		public int fastestSwingStartupFrames => _fastestSwingStartupFrames;
		public int defaultSwingStartupFrames => _defaultSwingStartupFrames;
		public int slowestSwingStartupFrames => _slowestSwingStartupFrames;

		public event Action onAllowAnimationCancels;
		public event Action onTryHitBall;

		public void Swing(SwingDirection direction, int startupFrames)
		{
			animator.SetInteger(SwingDirectionHash, (int) direction);
			_swingStartupFrames = startupFrames;
			Trigger(SwingHash);
		}

		public void SwitchSides() => Trigger(SwitchSidesHash);

		public void SideStep() => Trigger(SideStepHash);

		public void EndSideStep() => Trigger(EndSideStepHash);

		protected override void OnStartAnimation(Batter.Animation animation)
		{
			switch (animation)
			{
				case Batter.Animation.Swing:
					if (_swingStartupFrames == _defaultSwingStartupFrames)
					{
						animationSpeed = 1.00f;
					}
					else
					{
						animationSpeed = 0.01f + ((float) _defaultSwingStartupFrames) / ((float) _swingStartupFrames);
					}
					break;
			}
		}

		protected override void OnEndAnimation(Batter.Animation animation)
		{
			switch (animation)
			{
				case Batter.Animation.Swing:
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