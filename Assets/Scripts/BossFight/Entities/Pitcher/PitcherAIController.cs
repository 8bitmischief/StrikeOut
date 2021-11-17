using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut.BossFight {
	[RequireComponent(typeof(Pitcher))]
	public class PitcherAIController : EntityComponent<Pitcher> {
		public override void UpdateState () {
			if (entity.IsIdle()) {
				float r = Random.Range(0f, 1f);
				if (r < 0.1f)
					entity.LungeLeft();
				else if (r < 0.2f)
					entity.LungeRight();
				else
					entity.Pitch();
			}
		}
	}
}