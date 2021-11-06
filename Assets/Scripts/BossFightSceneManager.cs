using UnityEngine;

namespace StrikeOut {
	public class BossFightSceneManager : SceneManager {
		[Header("Children")]
		[SerializeField] private BossFightUpdateLoop updateLoop;

		public override void UpdateState () {
			if (!updateLoop.updateAutomatically)
				updateLoop.Advance();
		}
	}
}