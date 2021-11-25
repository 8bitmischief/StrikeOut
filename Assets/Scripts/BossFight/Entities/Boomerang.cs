using UnityEngine;

namespace StrikeOut.BossFight.Entities {
	public class Boomerang : BasicEntity {
		public void Throw ()
		{
			animator.Trigger();
		}

		protected override void OnEnterState (BasicEntityState state)
		{
			switch (state) {
				case BasicEntityState.Active:
					animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0f, 2f, 0f));
					break;
				case BasicEntityState.Done:
					BossFightScene.I.entityManager.DespawnEntity(this);
					break;
			}
		}
	}
}
