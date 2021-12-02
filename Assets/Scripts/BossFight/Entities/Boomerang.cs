using UnityEngine;

namespace StrikeOut.BossFight.Entities
{
	public class Boomerang : BasicEntity
	{
		public void Throw() => animator.Trigger();

		protected override void OnStartAnimation(BasicEntity.Animation animation)
		{
			switch (animation)
			{
				case BasicEntity.Animation.Active:
					animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0f, 2f, 0f));
					break;
				case BasicEntity.Animation.Done:
					BossFightScene.I.entityManager.DespawnEntity(this);
					break;
			}
		}
	}
}
