using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoomerangAnimator))]
	public class Boomerang : AnimatedEntity<BoomerangAnimator, Boomerang.Animation>
	{
		private bool _thrownToTheRight = false;

		public void Throw(bool toTheRight)
		{
			_thrownToTheRight = toTheRight;
			animator.Throw();
		}

		protected override void OnStartAnimation(Animation animation)
		{
			switch (animation)
			{
				case Animation.Throw:
					if (_thrownToTheRight)
					{
						animator.SetRootMotion(BossFightScene.I.batterRightPosition + new Vector3(0f, 1.5f, 0f));
					}
					else
					{
						animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0f, 1.5f, 0f));
					}
					break;
				case Animation.Rebound:
					if (_thrownToTheRight)
					{
						animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0f, 1.5f, 0f));
					}
					else
					{
						animator.SetRootMotion(BossFightScene.I.batterRightPosition + new Vector3(0f, 1.5f, 0f));
					}
					break;
				case Animation.Return:
					animator.SetRootMotion(BossFightScene.I.pitcher.transform.position + new Vector3(0f, 1.5f, 0f));
					break;
				case Animation.Done:
					DespawnEntity(this);
					break;
			}
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Throw = 2,
			Rebound = 3,
			Return = 4,
			Done = 5
		}
	}
}
