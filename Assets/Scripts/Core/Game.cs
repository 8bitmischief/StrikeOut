using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public class Game : SingletonMonoBehaviour<Game> {
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void Reset () => ResetInstance();

		[Header("Game Config")]
		public bool debugMode = true;

		[Header("Game Managers")]
		public InputManager input;

		public Scene scene => sceneManager?.scene ?? Scene.None;

		private ISceneManager sceneManager;

		private void Update () {
			if (input.start.justPressed)
				Application.Quit();
		}

		public void RegisterSceneManager (ISceneManager sceneManager) {
			this.sceneManager = sceneManager;
		}

		public void UnregisterSceneManager (ISceneManager sceneManager) {
			if (this.sceneManager == sceneManager)
				sceneManager = null;
		}
	}
}