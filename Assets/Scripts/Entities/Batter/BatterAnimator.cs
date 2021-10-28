using UnityEngine;
using SharedUnityMischief.Animation;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BatterAnimator : EnumStateMachineAnimator<Batter.State> {}
}