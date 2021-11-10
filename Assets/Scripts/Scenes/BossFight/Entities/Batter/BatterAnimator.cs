using System;
using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EntityAnimator<Batter, Batter.State> {
		private static readonly int switchSidesHash = Animator.StringToHash("Switch Sides");
		private static readonly int sideStepHash = Animator.StringToHash("Side Step");
		private static readonly int endSideStepHash = Animator.StringToHash("End Side Step");

		public Action onAllowAnimationCancels;

		public void SwitchSides () => Trigger(switchSidesHash);

		public void SideStep () => Trigger(sideStepHash);

		public void EndSideStep () => Trigger(endSideStepHash);
		
		protected override void OnAnimationEvent (AnimationEvent evt) {
			if (evt.stringParameter == "Allow Animation Cancels")
				onAllowAnimationCancels?.Invoke();
		}
	}
}