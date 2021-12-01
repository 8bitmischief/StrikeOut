using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BasicEntityAnimator))]
	public abstract class BasicEntity : AnimatedEntity<BasicEntityAnimator, BasicEntity.Animation>
	{
		public enum Animation
		{
			None = 0,
			Idle = 1,
			Active = 2,
			Done = 3
		}
	}
}