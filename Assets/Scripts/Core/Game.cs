using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public class Game : SingletonMonoBehaviour<Game> {
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void Reset () => ResetInstance();

		[Header("Game Config")]
		[SerializeField] private bool _debugMode = true;

		[Header("Game Managers")]
		[SerializeField] private InputManager _input;

		public Scene scene => sceneManager?.scene ?? Scene.None;
		public InputManager input => _input;
		public bool debugMode => _debugMode;

		private ISceneManager sceneManager;

		public void RegisterSceneManager (ISceneManager sceneManager) {
			this.sceneManager = sceneManager;
		}

		public void UnregisterSceneManager (ISceneManager sceneManager) {
			if (this.sceneManager == sceneManager)
				sceneManager = null;
		}
	}
}