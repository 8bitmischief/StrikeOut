using UnityEngine;

namespace StrikeOut {
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : OldAnimatedEntity<Batter.State, BatterAnimator> {
		public enum State {
			None = 0,
			Idle = 1
		}
	}
}