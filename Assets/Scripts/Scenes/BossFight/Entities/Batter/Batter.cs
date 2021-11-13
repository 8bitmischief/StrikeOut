using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<Batter.State, BatterAnimator> {
		public bool isOnRightSide { get; private set; } = false;

		private StrikeZone strikeZone = StrikeZone.None;
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
			Ball targetBall = null;
			foreach (Ball ball in Game.I.bossFight.balls)
				if (ball.strikeZone == strikeZone && ball.isHittable)
					if (targetBall == null || targetBall.framesUntilUnhittable > ball.framesUntilUnhittable)
						targetBall = ball;
			if (targetBall != null)
				targetBall.Hit(new Vector3(15f, 5f, 50f));
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