using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
		protected override void OnEnterState (State state) {
			switch (state) {
				case State.Pitch:
					Vector3 target = new Vector3(0f, 3.2f, 0f);
					animator.SetRootMotionTarget(target);
					break;
				case State.Idle:
					Game.I.bossFight.DespawnEntity(this);
					break;
			}
		}

		public enum State {
			None = 0,
			Idle = 1,
			Pitch = 2
		}
	}
}