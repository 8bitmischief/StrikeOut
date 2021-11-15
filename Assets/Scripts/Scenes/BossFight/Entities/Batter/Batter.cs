using UnityEngine;
using CameraShake;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<Batter.State, BatterAnimator> {
		[SerializeField] private int preSwingFrames = 4;
		[SerializeField] private BounceShake.Params hitBallShakeParams;

		public bool isOnRightSide { get; private set; } = false;

		private StrikeZone strikeZone = StrikeZone.None;
		private Ball targetBall = null;
		private bool canCancelAnimation = false;

		protected override void OnEnable () {
			base.OnEnable();
			animator.onAllowAnimationCancels += OnAllowAnimationCancels;
			animator.onTryHitBall += OnTryHitBall;
		}

		protected override void OnDisable () {
			base.OnDisable();
			animator.onAllowAnimationCancels -= OnAllowAnimationCancels;
			animator.onTryHitBall -= OnTryHitBall;
		}

		public bool CanSwing (StrikeZone strikeZone) {
			switch (state) {
				case State.Swing:
				case State.SideStepEnd:
				case State.SwitchSides:
					return canCancelAnimation;
				case State.Idle:
					return true;
				default:
					return false;
			}
		}

		public bool CanDodgeLeft () {
			if (isOnRightSide)
				return CanSwitchSides() || CanEndSideStep();
			else
				return CanSideStep();
		}

		public bool CanDodgeRight () {
			if (isOnRightSide)
				return CanSideStep();
			else
				return CanSwitchSides() || CanEndSideStep();
		}

		public bool CanSwitchSides () {
			switch (state) {
				case State.Swing:
				case State.SideStepEnd:
				case State.SwitchSides:
					return canCancelAnimation;
				case State.Idle:
					return true;
				default:
					return false;
			}
		}

		public bool CanSideStep () {
			switch (state) {
				case State.Swing:
				case State.SideStepEnd:
				case State.SwitchSides:
					return canCancelAnimation;
				case State.Idle:
					return true;
				default:
					return false;
			}
		}
		
		public bool CanEndSideStep () {
			return state == State.SideStepStart && canCancelAnimation;
		}

		public void Swing (StrikeZone strikeZone) {
			// Figure out if there's a ball that will be able to be hit
			 targetBall = null;
			foreach (Ball ball in Game.I.bossFight.balls)
				if (ball.strikeZone == strikeZone && (ball.isHittable || ball.willBeHittable))
					if ((ball.isHittable || ball.framesUntilHittable <= preSwingFrames) && ball.framesUntilUnhittable > preSwingFrames)
						if (targetBall == null || targetBall.framesUntilUnhittable > ball.framesUntilUnhittable)
							targetBall = ball;
			this.strikeZone = strikeZone;
			switch (strikeZone) {
				case StrikeZone.North:
					animator.SwingNorth();
					break;
				case StrikeZone.South:
					animator.SwingSouth();
					break;
				case StrikeZone.East:
				case StrikeZone.West:
					if (isOnRightSide == (strikeZone == StrikeZone.East))
						animator.SwingInwards();
					else
						animator.SwingOutwards();
					break;
			}
		}

		public void DodgeLeft () {
			if (isOnRightSide) {
				if (state == State.SideStepStart)
					EndSideStep();
				else
					SwitchSides();
			}
			else
				SideStep();
		}

		public void DodgeRight () {
			if (isOnRightSide)
				SideStep();
			else {
				if (state == State.SideStepStart)
					EndSideStep();
				else
					SwitchSides();
			}
		}

		public void SwitchSides () {
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			isOnRightSide = !isOnRightSide;
			animator.SwitchSides();
		}

		public void SideStep () {
			animator.SideStep();
		}

		public void EndSideStep () {
			animator.EndSideStep();
		}

		protected override void OnEnterState (State state) {
			switch (state) {
				case State.Swing:
				case State.SwitchSides:
				case State.SideStepEnd:
					animator.SetRootMotion(isOnRightSide ?
						Game.I.bossFight.batterRightPosition :
						Game.I.bossFight.batterLeftPosition);
					break;
				case State.SideStepStart:
					animator.SetRootMotion(isOnRightSide ?
						Game.I.bossFight.batterDodgeRightPosition :
						Game.I.bossFight.batterDodgeLeftPosition);
					break;
			}
		}

		protected override void OnLeaveState (State state) {
			canCancelAnimation = false;
		}

		private void OnAllowAnimationCancels () {
			canCancelAnimation = true;
		}

		private void OnTryHitBall () {
			if (targetBall != null) {
				Vector3 targetPosition = new Vector3(15f, 5f, 50f);
				Vector3 shakeDirection;
				if (targetBall.strikeZone == StrikeZone.North)
					shakeDirection = new Vector3(1f, 0.3f, 0f);
				else if (targetBall.strikeZone == StrikeZone.South)
					shakeDirection = new Vector3(1f, -0.3f, 0f);
				else
					shakeDirection = new Vector3(1f, 0f, 0f);
				if (isOnRightSide)
					shakeDirection.x *= -1;
				targetBall.Hit(targetPosition);
				CameraShaker.Shake(new BounceShake(hitBallShakeParams, new Displacement(shakeDirection, new Vector3(0f, 0f, 1f))));
			}
			targetBall = null;
		}

		public enum State {
			None = 0,
			Idle = 1,
			SwitchSides = 2,
			SideStepStart = 3,
			SideStepEnd = 4,
			Swing = 5
		}
	}
}