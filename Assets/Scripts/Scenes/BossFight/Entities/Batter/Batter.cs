using UnityEngine;
using SharedUnityMischief.Lifecycle;

/*
TODO:
	Make batting more nuanced and juicy
	I'm talkin'
		screenshake
		glass shattering on miss
		sound effect
		animation timing
			swing faster when too early to emphasize the miss
			swing faster + longer recoil when late
			swing slower when early but not too early
		more oomphy effects when perfect hitting
		particles
		trails
		rotating sphere ball
		"single" or "double" or "triple" or "home run"
		points
		fireworks explosion
		variance in pitch location
		lil indicator of where ball will connect
			animation to sell when it'll pass strike zone
		better sprite
*/

namespace StrikeOut {
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<Batter.State, BatterAnimator> {
		[SerializeField] private int preSwingFrames = 4;

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
			if (targetBall != null)
				targetBall.Hit(new Vector3(15f, 5f, 50f));
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