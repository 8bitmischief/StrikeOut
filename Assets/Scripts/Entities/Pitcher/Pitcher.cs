using UnityEngine;

namespace StrikeOut {
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<Pitcher.State, PitcherAnimator> {
		private void OnEnable () {
			animator.onSpawnBall += SpawnBall;
		}

		private void OnDisable () {
			animator.onSpawnBall -= SpawnBall;
		}

		public bool CanPitch () {
			return animator.state == State.Idle;
		}

		public void Pitch () {
			animator.Pitch();
		}

		private void SpawnBall (Vector3 position) {
			BossFightManager.SpawnBall(position);
		}

		public enum State {
			None = 0,
			Idle = 1,
			Pitch = 2
		}
	}
}