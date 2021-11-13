using System;
using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, Batter.State> {
		private static readonly int swingNorthHash = Animator.StringToHash("Swing North");
		private static readonly int swingSouthHash = Animator.StringToHash("Swing South");
		private static readonly int swingInwardsHash = Animator.StringToHash("Swing Inwards");
		private static readonly int swingOutwardsHash = Animator.StringToHash("Swing Outwards");
		private static readonly int switchSidesHash = Animator.StringToHash("Switch Sides");
		private static readonly int sideStepHash = Animator.StringToHash("Side Step");
		private static readonly int endSideStepHash = Animator.StringToHash("End Side Step");

		public Action onAllowAnimationCancels;
		public Action onTryHitBall;

		public void SwingNorth () => Trigger(swingNorthHash);

		public void SwingSouth () => Trigger(swingSouthHash);

		public void SwingInwards () => Trigger(swingInwardsHash);

		public void SwingOutwards () => Trigger(swingOutwardsHash);

		public void SwitchSides () => Trigger(switchSidesHash);

		public void SideStep () => Trigger(sideStepHash);

		public void EndSideStep () => Trigger(endSideStepHash);
		
		protected override void OnAnimationEvent (AnimationEvent evt) {
			switch (evt.stringParameter) {
				case "Allow Animation Cancels":
					onAllowAnimationCancels?.Invoke();
					break;
				case "Try Hit Ball":
					onTryHitBall?.Invoke();
					break;
			}
		}
	}
}