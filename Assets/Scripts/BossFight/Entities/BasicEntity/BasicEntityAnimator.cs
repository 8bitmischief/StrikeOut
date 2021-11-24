using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut.BossFight {
	[RequireComponent(typeof(Animator))]
	public class BasicEntityAnimator : EntityAnimator<BasicEntity, BasicEntityState> {
		private static readonly int triggerHash = Animator.StringToHash("Trigger");

		public void Trigger () => base.Trigger(triggerHash);
	}
}