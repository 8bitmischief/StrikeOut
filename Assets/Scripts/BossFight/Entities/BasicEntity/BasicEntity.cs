using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut.BossFight {
	[RequireComponent(typeof(BasicEntityAnimator))]
	public abstract class BasicEntity : AnimatedEntity<BasicEntityState, BasicEntityAnimator> {}
}