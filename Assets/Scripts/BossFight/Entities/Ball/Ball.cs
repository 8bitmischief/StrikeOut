using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut.BossFight {
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<Ball.State, BallAnimator> {
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
		public bool hasPassedBattingLine { get; private set; } = false;
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
		public int framesSincePassedBattingLine { get; private set; } = -1;

		private PitchDataObject.PitchData pitchData = null;
		private Vector3 prevPosition;
		private Vector3 velocity = Vector3.zero;
		private Vector3 accelerationPerFrame = Vector3.zero;
		private bool justPassedBattingLine = false;

		public override void Reset () {
			strikeZone = StrikeZone.None;
			hasPassedBattingLine = false;
			framesSincePassedBattingLine = -1;
			pitchData = null;
			prevPosition = Vector3.zero;
			velocity = Vector3.zero;
			accelerationPerFrame = Vector3.zero;
			justPassedBattingLine = false;
		}

		public override void OnSpawn () {
			prevPosition = transform.position;
			BossFightScene.I.balls.Add(this);
		}

		public override void UpdateState () {
			if (!BossFightScene.I.updateLoop.isInterpolating && hasPassedBattingLine) {
				if (justPassedBattingLine)
					justPassedBattingLine = false;
				else
					framesSincePassedBattingLine++;
			}
			switch (state) {
				case State.Pitched:
					// Keep track of the ball's velocity and acceleration (determined by animation and root motion)
					if (!BossFightScene.I.updateLoop.isInterpolating) {
						Vector3 newVelocity = (transform.position - prevPosition) / UpdateLoop.timePerUpdate;
						accelerationPerFrame = newVelocity - velocity;
						velocity = newVelocity;
						prevPosition = transform.position;
					}
					break;
				case State.Missed:
					if (strikeZone == StrikeZone.None) {
						// Follow through from where the arc of the pitch left off
						if (!BossFightScene.I.updateLoop.isInterpolating)
							velocity += accelerationPerFrame;
						transform.position += velocity * BossFightScene.I.updateLoop.deltaTime;
						// Despawn the ball once it's behind the camera
						if (!BossFightScene.I.updateLoop.isInterpolating && !isHittable && !willBeHittable && (transform.position.z < -15f || framesInState > 20))
							BossFightScene.I.DespawnEntity(this);
					}
					else {
						// Despawn the ball if the player doesn't hit it in time
						if (!BossFightScene.I.updateLoop.isInterpolating && !isHittable && !willBeHittable)
							BossFightScene.I.DespawnEntity(this);
					}
					break;
				case State.Hit:
					if (!BossFightScene.I.updateLoop.isInterpolating && animator.hasAnimationCompleted)
						BossFightScene.I.DespawnEntity(this);
					break;
			}
		}

		public override void OnDespawn () {
			BossFightScene.I.balls.Remove(this);
		}

		public void Pitch (PitchType pitchType, StrikeZone strikeZone) => Pitch(pitchType, strikeZone, BossFightScene.I.GetStrikeZonePosition(strikeZone));

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
			pitchData = BossFightScene.I.pitchData.pitches[pitchType];
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