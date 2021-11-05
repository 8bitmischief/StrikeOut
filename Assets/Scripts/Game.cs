using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public class Game : SingletonMonoBehaviour<Game> {
		public Scene scene => sceneManager?.scene ?? Scene.None;
		public BossFightSceneManager baseball { get; private set; } = null;

		private SceneManager sceneManager;

		private void Update () {
			if (sceneManager != null)
				sceneManager.UpdateState();
		}

		public void RegisterSceneManager (SceneManager sceneManager) {
			this.sceneManager = sceneManager;
			if (sceneManager.scene == Scene.Baseball)
				baseball = sceneManager as BossFightSceneManager;
		}

		public void UnregisterSceneManager (SceneManager sceneManager) {
			if (this.sceneManager == sceneManager)
				sceneManager = null;
			if (baseball == sceneManager)
				baseball = null;
		}
	}
}