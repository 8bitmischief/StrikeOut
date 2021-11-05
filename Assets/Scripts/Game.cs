using UnityEngine;

namespace StrikeOut {
	public class Game : SharedUnityMischief.Lifecycle.GameManager<Game> {
		public Scene scene => sceneManager?.scene ?? Scene.None;
		public BaseballSceneManager baseball { get; private set; } = null;

		private SceneManager sceneManager;

		protected override void UpdateState () {
			if (sceneManager != null)
				sceneManager.UpdateState();
		}

		public void RegisterSceneManager (SceneManager sceneManager) {
			this.sceneManager = sceneManager;
			if (sceneManager.scene == Scene.Baseball)
				baseball = sceneManager as BaseballSceneManager;
		}

		public void UnregisterSceneManager (SceneManager sceneManager) {
			if (this.sceneManager == sceneManager)
				sceneManager = null;
			if (baseball == sceneManager)
				baseball = null;
		}
	}
}