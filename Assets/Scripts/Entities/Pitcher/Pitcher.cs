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

		public bool CanPitch () {
			return animator.state == State.Idle;
		}

		public void Pitch () {
			animator.Pitch();
		}

		private void PitchBall (Vector3 spawnPosition) {
			Ball ball = Game.I.bossFight.SpawnBall(spawnPosition);
			switch (Random.Range(1, 5)) {
				case 1:
					ball.Pitch(CardinalDirection.North);
					break;
				case 2:
					ball.Pitch(CardinalDirection.East);
					break;
				case 3:
					ball.Pitch(CardinalDirection.South);
					break;
				case 4:
					ball.Pitch(CardinalDirection.West);
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