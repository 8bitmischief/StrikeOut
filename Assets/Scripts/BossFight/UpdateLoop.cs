using UnityEngine;

namespace StrikeOut.BossFight
{
	public class UpdateLoop : SharedUnityMischief.Lifecycle.UpdateLoop
	{
		protected override void UpdateState()
		{
			// Update entities
			Scene.I.entityManager.UpdateState();
			// Update attacks
			foreach (Attack attack in Scene.I.attacks)
				attack.UpdateState();
			for (int i = Scene.I.attacks.Count - 1; i >= 0; i--)
				if (Scene.I.attacks[i].isDone)
					Scene.I.attacks.RemoveAt(i);
		}
	}
}
