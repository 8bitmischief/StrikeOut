using System;
using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, Batter.State> {
		private static readonly int swingHash = Animator.StringToHash("Swing");
		private static readonly int swingDirectionHash = Animator.StringToHash("Swing Direction");
		private static readonly int swingStartupFramesHash = Animator.StringToHash("Swing Startup Frames");
		private static readonly int switchSidesHash = Animator.StringToHash("Switch Sides");
		private static readonly int sideStepHash = Animator.StringToHash("Side Step");
		private static readonly int endSideStepHash = Animator.StringToHash("End Side Step");

		[SerializeField] public int fastestSwingStartupFrames = 2;
		[SerializeField] public int defaultSwingStartupFrames = 4;
		[SerializeField] public int slowestSwingStartupFrames = 8;

		public Action onAllowAnimationCancels;
		public Action onTryHitBall;

		private int swingStartupFrames { get => animator.GetInteger(swingStartupFramesHash); set => animator.SetInteger(swingStartupFramesHash, value); }

		public void Swing (SwingDirection direction, int startupFrames) {
			animator.SetInteger(swingDirectionHash, (int) direction);
			swingStartupFrames = startupFrames;
			Trigger(swingHash);
		}

		public void SwitchSides () => Trigger(switchSidesHash);

		public void SideStep () => Trigger(sideStepHash);

		public void EndSideStep () => Trigger(endSideStepHash);

		protected override void OnEnterState (Batter.State state) {
			switch (state) {
				case Batter.State.Swing:
					if (swingStartupFrames == defaultSwingStartupFrames)
						animationSpeed = 1.00f;
					else
						animationSpeed = 0.01f + ((float) defaultSwingStartupFrames) / ((float) swingStartupFrames);
					break;
			}
		}

		protected override void OnLeaveState (Batter.State state) {
			switch (state) {
				case Batter.State.Swing:
					animationSpeed = 1.00f;
					break;
			}
		}
		
		protected override void OnAnimationEvent (AnimationEvent evt) {
			switch (evt.stringParameter) {
				case "Allow Animation Cancels":
					onAllowAnimationCancels?.Invoke();
					break;
				case "Try Hit Ball":
					animationSpeed = 1.00f;
					onTryHitBall?.Invoke();
					break;
			}
		}

		public enum SwingDirection {
			None = 0,
			North = 1,
			Inside = 2,
			South = 3,
			Outside = 4
		}
	}
}