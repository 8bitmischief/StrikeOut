using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<Batter.State, BatterAnimator> {
		public bool isOnRightSide { get; private set; } = false;

		private bool canCancelAnimation = false;

		protected override void OnEnable () {
			base.OnEnable();
			animator.onAllowAnimationCancels += OnAllowAnimationCancels;
		}

		protected override void OnDisable () {
			base.OnDisable();
			animator.onAllowAnimationCancels -= OnAllowAnimationCancels;
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
				case State.SwitchSides:
				case State.SideStepEnd:
					animator.SetRootMotion(isOnRightSide ?
						BossFightScene.batterRightPosition :
						BossFightScene.batterLeftPosition, true);
					break;
				case State.SideStepStart:
					animator.SetRootMotion(isOnRightSide ?
						BossFightScene.batterRightPosition + new Vector3(2f, 0f, 0f) :
						BossFightScene.batterLeftPosition - new Vector3(2f, 0f, 0f), true);
					break;
			}
		}

		protected override void OnLeaveState (State state) {
			canCancelAnimation = false;
		}

		private void OnAllowAnimationCancels () {
			canCancelAnimation = true;
		}

		public enum State {
			None = 0,
			Idle = 1,
			SwitchSides = 2,
			SideStepStart = 3,
			SideStepEnd = 4
		}
	}
}