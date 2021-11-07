using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class TomatoAnimator : EntityAnimator<Tomato, Tomato.State> {
		private static readonly int hopHash = Animator.StringToHash("Hop");
		private static readonly int flipHash = Animator.StringToHash("Flip");
		private static readonly Vector3 authoredHopRootMotion = new Vector3(3f, 0f, 0f);

		public void Hop (Vector3 position) {
			Trigger(hopHash, position - transform.position - Vector3.Scale(authoredHopRootMotion, transform.localScale));
		}

		public void Flip () => Trigger(flipHash);
	}
}