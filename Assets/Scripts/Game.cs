using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public class Game : SingletonMonoBehaviour<Game> {
		[Header("Game Config")]
		[SerializeField] private bool _debugMode = true;

		[Header("Game Managers")]
		[SerializeField] private InputManager _input;

		public Scene scene => sceneManager?.scene ?? Scene.None;
		public InputManager input => _input;
		public BossFightScene bossFight { get; private set; } = null;
		public bool debugMode => _debugMode;

		private SceneManager sceneManager;

		public void RegisterSceneManager (SceneManager sceneManager) {
			this.sceneManager = sceneManager;
			if (sceneManager.scene == Scene.BossFight)
				bossFight = sceneManager as BossFightScene;
		}

		public void UnregisterSceneManager (SceneManager sceneManager) {
			if (this.sceneManager == sceneManager)
				sceneManager = null;
			if (bossFight == sceneManager)
				bossFight = null;
		}
	}
}