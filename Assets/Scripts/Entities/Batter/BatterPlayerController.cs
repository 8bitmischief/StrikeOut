using UnityEngine;

namespace StrikeOut {
	[RequireComponent(typeof(Batter))]
	public class BatterPlayerController : MonoBehaviour {
		private Batter batter;

		private void Awake () {
			batter = GetComponent<Batter>();
		}
	}
}