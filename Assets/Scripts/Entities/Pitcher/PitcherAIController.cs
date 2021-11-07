using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Pitcher))]
	public class PitcherAIController : EntityComponent<Pitcher> {
		private void Update () {
			if (entity.CanPitch())
				entity.Pitch();
		}
	}
}