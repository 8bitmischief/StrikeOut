using UnityEngine;

namespace StrikeOut.BossFight
{
	public class UpdateLoop : SharedUnityMischief.Lifecycle.UpdateLoop
	{
		protected override void UpdateState()
		{
			Scene.I.entityManager.UpdateState();
		}
	}
}
