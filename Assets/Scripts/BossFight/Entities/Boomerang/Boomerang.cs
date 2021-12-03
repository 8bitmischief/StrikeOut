using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoomerangAnimator))]
	public class Boomerang : AnimatedEntity<BoomerangAnimator, Boomerang.Animation>
	{
		public void Throw()
		{
			animator.Throw(BossFightScene.I.batterLeftPosition);
		}

		protected override void OnStartAnimation(Animation animation)
		{
			switch (animation)
			{
				case Animation.Done:
					DespawnEntity(this);
					break;
			}
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Thrown = 2,
			Done = 3
		}
	}
}
