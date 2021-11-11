using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<Pitcher.State, PitcherAnimator> {
		protected override void OnEnable () {
			base.OnEnable();
			animator.onPitchBall += PitchBall;
		}

		protected override void OnDisable () {
			base.OnDisable();
			animator.onPitchBall -= PitchBall;
		}

		public bool IsIdle () {
			return animator.state == State.Idle;
		}

		public void Pitch () {
			animator.Pitch();
		}

		public void LungeLeft () {
			transform.localScale = new Vector3(1f, 1f, 1f);
			animator.Lunge(new Vector3(-2.6f, 0f, 3f));
		}

		public void LungeRight () {
			transform.localScale = new Vector3(-1f, 1f, 1f);
			animator.Lunge(new Vector3(2.6f, 0f, 3f));
		}

		protected override void OnEnterState (State state) {
			switch (state) {
				case State.BackOff:
					transform.localScale = new Vector3(1f, 1f, 1f);
					animator.SetRootMotion(new Vector3(0f, 0f, 25f), true);
					break;
			}
		}

		private void PitchBall (Vector3 spawnPosition) {
			Ball ball = Game.I.bossFight.SpawnBall(spawnPosition);
			switch (Random.Range(1, 3)) {
				case 1:
					ball.Throw(Ball.Pitch.Curveball, CardinalDirection.North);
					break;
				case 2:
					ball.Throw(Ball.Pitch.Curveball, new Vector3(5f, 2f, 0f));
					break;
			}
		}

		public enum State {
			None = 0,
			Idle = 1,
			Pitch = 2,
			Lunge = 3,
			BackOff = 4
		}
	}
}