using UnityEngine;

namespace StrikeOut.BossFight
{
	public class UpdateLoop : SharedUnityMischief.Lifecycle.UpdateLoop
	{
		private void Start()
		{
			Physics.autoSimulation = false;
		}

		protected override void UpdateState()
		{
			Scene.I.entityManager.EarlyUpdateEntities();
			Scene.I.entityManager.UpdateEntities();
			if (!isInterpolating)
				Physics.Simulate(UpdateLoop.TimePerUpdate);
			Scene.I.hitDetectionManager.CheckForHits();
			Scene.I.entityManager.CheckEntityInteractions();
			Scene.I.entityManager.LateUpdateEntities();
			Scene.I.entityManager.SpawnAndDespawnEntities();
			if (isFinalUpdateThisFrame)
				Scene.I.entityManager.RenderEntities();
		}
	}
}