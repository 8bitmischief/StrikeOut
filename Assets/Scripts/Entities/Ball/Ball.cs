using UnityEngine;

namespace StrikeOut {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
		private void OnEnable () {
			animator.onChangeState += OnChangeState;
		}

		private void OnDisable () {
			animator.onChangeState -= OnChangeState;
		}

		private void OnChangeState (State state, State prevState) {
			if (state == State.Idle)
				Destroy(gameObject);
		}

		public enum State {
			None = 0,
			Idle = 1,
			Pitch = 2
		}
	}
}