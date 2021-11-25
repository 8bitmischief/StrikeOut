using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BasicEntityAnimator))]
	public abstract class BasicEntity : AnimatedEntity<BasicEntityState, BasicEntityAnimator> {}
}