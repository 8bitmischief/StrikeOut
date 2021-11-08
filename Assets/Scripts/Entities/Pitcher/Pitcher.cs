using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<Pitcher.State, PitcherAnimator> {
		protected override void OnEnable () {
			base.OnEnable();
			animator.onSpawnBall += SpawnBall;
		}

		protected override void OnDisable () {
			base.OnDisable();
			animator.onSpawnBall -= SpawnBall;
		}

		public bool CanPitch () {
			return animator.state == State.Idle;
		}

		public void Pitch () {
			animator.Pitch();
		}

		private void SpawnBall (Vector3 position) {
			Ball ball = Game.I.bossFight.SpawnBall(position);
		}

		public enum State {
			None = 0,
			Idle = 1,
			Pitch = 2
		}
	}
}