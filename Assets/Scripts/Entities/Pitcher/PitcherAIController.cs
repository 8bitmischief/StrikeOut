using UnityEngine;

namespace StrikeOut {
	[RequireComponent(typeof(Pitcher))]
	public class PitcherAIController : MonoBehaviour {
		private Pitcher pitcher;

		private void Awake () {
			pitcher = GetComponent<Pitcher>();
		}

		private void Update () {
			if (pitcher.CanPitch())
				pitcher.Pitch();
		}
	}
}