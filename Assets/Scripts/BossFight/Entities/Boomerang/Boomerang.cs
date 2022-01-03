using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoomerangAnimator))]
	public class Boomerang : AnimatedEntity<BoomerangAnimator, Boomerang.Animation>, IEnemyHittable
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
						animator.SetRootMotion(Scene.I.locations.batter.right + new Vector3(0f, 1.5f, 0f), false);
					}
					else
					{
						animator.SetRootMotion(Scene.I.locations.batter.left + new Vector3(0f, 1.5f, 0f), false);
					}
					break;
				case Animation.Rebound:
					if (_thrownToTheRight)
					{
						animator.SetRootMotion(Scene.I.locations.batter.left + new Vector3(0f, 1.5f, 0f), false);
					}
					else
					{
						animator.SetRootMotion(Scene.I.locations.batter.right + new Vector3(0f, 1.5f, 0f), false);
					}
					break;
				case Animation.Return:
					animator.SetRootMotion(Scene.I.entityManager.pitcher.transform.position + new Vector3(0f, 1.5f, 0f), false);
					break;
				case Animation.Done:
					DespawnEntity(this);
					break;
			}
		}

		public void OnHit(EnemyHitRecord hit)
		{
			animator.Hit();
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Throw = 2,
			Rebound = 3,
			Return = 4,
			Hit = 5,
			Done = 6
		}
	}
}
