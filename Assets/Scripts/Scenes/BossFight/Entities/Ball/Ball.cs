using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
		public CardinalDirection strikeZone { get; private set; } = CardinalDirection.None;

		public override void UpdateState () {
			switch (state) {
				case State.HitStrikeZone:
					if (framesInState > 10)
						Game.I.bossFight.DespawnEntity(this);
					break;
				case State.MissedStrikeZone:
					if (framesInState > 20)
						Game.I.bossFight.DespawnEntity(this);
					break;
			}
		}

		public void Throw (Pitch pitch, CardinalDirection strikeZone) {
			this.strikeZone = strikeZone;
			animator.Throw(pitch, BossFightScene.strikeZonePositions[strikeZone], true);
		}

		public void Throw (Pitch pitch, Vector3 position) {
			this.strikeZone = CardinalDirection.None;
			animator.Throw(pitch, position, false);
		}

		public enum Pitch {
			None = 0,
			Curveball = 1
		}

		public enum State {
			None = 0,
			Idle = 1,
			Thrown = 2,
			AtStrikeZone = 3,
			HitStrikeZone = 4,
			MissedStrikeZone = 5
		}
	}
}