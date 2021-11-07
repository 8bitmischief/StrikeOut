using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<Batter.State, BatterAnimator> {
		public bool isOnRightSide { get; private set; } = false;

		private bool canCancelAnimation = false;

		private void OnEnable () {
			animator.onChangeState += OnChangeState;
			animator.onAllowAnimationCancels += OnAllowAnimationCancels;
		}

		private void OnDisable () {
			animator.onChangeState -= OnChangeState;
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
			animator.SwitchSides(isOnRightSide ? BossFightSceneManager.batterRightPosition : BossFightSceneManager.batterLeftPosition);
		}

		public void SideStep () {
			animator.SideStep(isOnRightSide ? BossFightSceneManager.batterRightPosition : BossFightSceneManager.batterLeftPosition);
		}

		public void EndSideStep () {
			animator.EndSideStep();
		}

		private void OnChangeState (State state, State prevState) {
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