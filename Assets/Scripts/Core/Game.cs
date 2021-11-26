using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut
{
	public class Game : SingletonMonoBehaviour<Game>
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void ResetStaticFields() => _instance = null;

		[Header("Game Config")]
		[SerializeField] private bool _debugMode = true;
		[Header("Game Managers")]
		[SerializeField] private InputManager _input;
		[SerializeField] private ISceneManager _sceneManager;

		public bool debugMode => _debugMode;
		public InputManager input => _input;
		public Scene scene => _sceneManager?.scene ?? Scene.None;

		private void Update()
		{
			if (_input.start.justPressed)
			{
				Application.Quit();
			}
		}

		public void RegisterSceneManager(ISceneManager sceneManager)
		{
			_sceneManager = sceneManager;
		}

		public void UnregisterSceneManager(ISceneManager sceneManager)
		{
			if (_sceneManager == sceneManager)
			{
				_sceneManager = null;
			}
		}
	}
}