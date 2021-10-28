using UnityEngine;
using SharedUnityMischief.Animation;

namespace StrikeOut {
	public abstract class AnimatedEntity<T, U> : MonoBehaviour where U : EnumStateMachineAnimator<T> {
		protected U animator;

		protected void Awake () {
			animator = GetComponent<U>();
		}
	}
}