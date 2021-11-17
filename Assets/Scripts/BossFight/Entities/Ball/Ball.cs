using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
		public override bool appendSpawnIndexToName => true;

		public StrikeZone strikeZone { get; private set; } = StrikeZone.None;
		public bool isHittable {
			get {
				if (animator == null)
					return false;
				switch (state) {
					case State.Pitched: return animator.animationFrameDuration - animator.animationFrame <= pitchData.earlyHitFrames;
					case State.Missed: return animator.animationFrame < pitchData.lateHitFrames;
					default: return false;
				}
			}
		}
		public bool willBeHittable => framesUntilHittable > 0;
		public int framesUntilHittable {
			get {
				if (animator == null)
					return -1;
				switch (state) {
					case State.Pitched: return Mathf.Max(0, animator.animationFrameDuration - animator.animationFrame - pitchData.earlyHitFrames);
					case State.Missed: return animator.animationFrame < pitchData.lateHitFrames ? 0 : -1;
					default: return -1;
				}
			}
		}
		public int framesUntilUnhittable {
			get {
				if (animator == null)
					return -1;
				switch (state) {
					case State.Pitched: return animator.animationFrameDuration - animator.animationFrame + pitchData.lateHitFrames;
					case State.Missed: return pitchData.lateHitFrames > animator.animationFrame ? pitchData.lateHitFrames - animator.animationFrame : -1;
					default: return -1;
				}
			}
		}
		public bool hasPassedBattingLine = false;
		public bool willPassBattingLine => state == State.Pitched;
		public int framesUntilPassBattingLine {
			get {
				if (animator == null)
					return -1;
				switch (state) {
					case State.Pitched: return animator.animationFrameDuration - animator.animationFrame;
					default: return -1;
				}
			}
		}
		public int framesSincePassedBattingLine = -1;

		private PitchDataObject.PitchData pitchData = null;
		private Vector3 prevPosition;
		private Vector3 velocity = Vector3.zero;
		private Vector3 accelerationPerFrame = Vector3.zero;
		private bool justPassedBattingLine = false;

		protected override void Awake () {
			base.Awake();
			prevPosition = transform.position;
		}

		public override void OnSpawn () {
			Game.I.bossFight.balls.Add(this);
		}

		public override void UpdateState () {
			if (!Game.I.bossFight.updateLoop.isInterpolating && hasPassedBattingLine) {
				if (justPassedBattingLine)
					justPassedBattingLine = false;
				else
					framesSincePassedBattingLine++;
			}
			switch (state) {
				case State.Pitched:
					// Keep track of the ball's velocity and acceleration (determined by animation and root motion)
					if (!Game.I.bossFight.updateLoop.isInterpolating) {
						Vector3 newVelocity = (transform.position - prevPosition) / UpdateLoop.timePerUpdate;
						accelerationPerFrame = newVelocity - velocity;
						velocity = newVelocity;
						prevPosition = transform.position;
					}
					break;
				case State.Missed:
					if (strikeZone == StrikeZone.None) {
						// Follow through from where the arc of the pitch left off
						if (!Game.I.bossFight.updateLoop.isInterpolating)
							velocity += accelerationPerFrame;
						transform.position += velocity * Game.I.bossFight.updateLoop.deltaTime;
						// Despawn the ball once it's behind the camera
						if (!Game.I.bossFight.updateLoop.isInterpolating && !isHittable && !willBeHittable && (transform.position.z < -15f || framesInState > 20))
							Game.I.bossFight.DespawnEntity(this);
					}
					else {
						// Despawn the ball if the player doesn't hit it in time
						if (!Game.I.bossFight.updateLoop.isInterpolating && !isHittable && !willBeHittable)
							Game.I.bossFight.DespawnEntity(this);
					}
					break;
				case State.Hit:
					if (!Game.I.bossFight.updateLoop.isInterpolating && animator.hasAnimationCompleted)
						Game.I.bossFight.DespawnEntity(this);
					break;
			}
		}

		public override void OnDespawn () {
			Game.I.bossFight.balls.Remove(this);
		}

		public void Pitch (PitchType pitchType, StrikeZone strikeZone) => Pitch(pitchType, strikeZone, Game.I.bossFight.GetStrikeZonePosition(strikeZone));

		public void Pitch (PitchType pitchType, Vector3 target) => Pitch(pitchType, StrikeZone.None, target);

		protected override void OnEnterState (State state) {
			switch (state) {
				case State.Missed:
					hasPassedBattingLine = true;
					justPassedBattingLine = true;
					framesSincePassedBattingLine = 0;
					break;
			}
		}

		private void Pitch (PitchType pitchType, StrikeZone strikeZone, Vector3 target) {
			this.strikeZone = strikeZone;
			pitchData = Game.I.bossFight.pitchData.pitches[pitchType];
			animator.Pitch(pitchType, target, strikeZone != StrikeZone.None);
		}

		public void Hit (Vector3 target) {
			animator.Hit(target);
		}

		public enum State {
			None = 0,
			Idle = 1,
			Pitched = 2,
			Hit = 3,
			Missed = 4
		}
	}
}