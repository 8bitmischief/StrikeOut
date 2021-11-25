using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut
{
	public class Game : SingletonMonoBehaviour<Game>
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void ResetSingletonClass() => instance = null;

		[Header("Game Config")]
		public bool debugMode = true;

		[Header("Game Managers")]
		public InputManager input;

		private ISceneManager sceneManager = null;
		public Scene scene => sceneManager?.scene ?? Scene.None;

		private void Update()
		{
			if (input.start.justPressed)
			{
				Application.Quit();
			}
		}

		public void RegisterSceneManager(ISceneManager sceneManager)
		{
			this.sceneManager = sceneManager;
		}

		public void UnregisterSceneManager(ISceneManager sceneManager)
		{
			if (this.sceneManager == sceneManager)
			{
				this.sceneManager = null;
			}
		}
	}
}