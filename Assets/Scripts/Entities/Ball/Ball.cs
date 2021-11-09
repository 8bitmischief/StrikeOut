using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
		public CardinalDirection strikeZone { get; private set; } = CardinalDirection.None;

		public void Pitch (CardinalDirection strikeZone) {
			this.strikeZone = strikeZone;
			animator.Pitch();
		}

		protected override void OnEnterState (State state) {
			switch (state) {
				case State.Pitch:
					animator.SetRootMotionTarget(BossFightSceneManager.strikeZonePositions[strikeZone]);
					break;
				case State.Done:
					Game.I.bossFight.DespawnEntity(this);
					break;
			}
		}

		public enum State {
			None = 0,
			Idle = 1,
			Done = 2,
			Pitch = 3,
		}
	}
}