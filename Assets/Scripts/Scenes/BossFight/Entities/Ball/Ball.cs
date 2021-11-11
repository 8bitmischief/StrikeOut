using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
		public StrikeZone strikeZone { get; private set; } = StrikeZone.None;

		private Vector3 prevPosition;
		private Vector3 velocity = Vector3.zero;
		private Vector3 accelerationPerFrame = Vector3.zero;

		protected override void Awake () {
			base.Awake();
			prevPosition = transform.position;
		}

		public override void UpdateState () {
			switch (state) {
				case State.Thrown:
					// Keep track of the ball's velocity and acceleration (determined by animation and root motion)
					if (!UpdateLoop.I.isInterpolating) {
						Vector3 newVelocity = (transform.position - prevPosition) / UpdateLoop.timePerUpdate;
						accelerationPerFrame = newVelocity - velocity;
						velocity = newVelocity;
						prevPosition = transform.position;
					}
					break;
				case State.HitStrikeZone:
					// Despawn the ball if the player doesn't hit it in time
					if (framesInState > 10)
						Game.I.bossFight.DespawnEntity(this);
					break;
				case State.MissedStrikeZone:
					// Follow through from where the arc of the pitch left off
					if (!UpdateLoop.I.isInterpolating)
						velocity += accelerationPerFrame;
					transform.position += velocity * UpdateLoop.I.deltaTime;
					// Despawn the ball once it's behind the camera
					if (transform.position.z < -15f || framesInState > 20)
						Game.I.bossFight.DespawnEntity(this);
					break;
			}
		}

		public void Throw (PitchType pitch, StrikeZone strikeZone) {
			this.strikeZone = strikeZone;
			animator.Throw(pitch, Game.I.bossFight.GetStrikeZonePosition(strikeZone), true);
		}

		public void Throw (PitchType pitch, Vector3 position) {
			this.strikeZone = StrikeZone.None;
			animator.Throw(pitch, position, false);
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