using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(TomatoAnimator))]
	public class Tomato : AnimatedEntity<Tomato.State, TomatoAnimator> {
		public void Hop () => animator.Hop();

		public void Flip () => animator.Flip();

		public override void UpdateState() {
		}

		public enum State {
			None = 0,
			Idle = 1,
			Hop = 2,
			Flip = 3,
			Wiggle = 4
		}
	}
}