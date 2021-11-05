using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class TomatoAnimator : EntityAnimator<Tomato, Tomato.State> {
		private static readonly int HOP_HASH = Animator.StringToHash("Hop");
		private static readonly int FLIP_HASH = Animator.StringToHash("Flip");

		public void Hop () => Trigger(HOP_HASH);

		public void Flip () => Trigger(FLIP_HASH);
	}
}