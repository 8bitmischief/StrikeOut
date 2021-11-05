using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	public class BossFightSceneManager : SceneManager {
		[Header("Children")]
		[SerializeField] private UpdateLoopUpdater updater;

		public override void UpdateState () {
			if (!updater.updateAutomatically)
				updater.UpdateState();
		}
	}
}