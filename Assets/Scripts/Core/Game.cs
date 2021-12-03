using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Input.Control;

namespace StrikeOut
{
	[DefaultExecutionOrder(-40)]
	public class Game : SingletonMonoBehaviour<Game>
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void ResetStaticFields() => ClearInstance();

		[Header("Game Config")]
		[SerializeField] private bool _debugMode = true;
		[Header("Game Managers")]
		[SerializeField] private InputManager _input;
		[SerializeField] private ISceneManager _sceneManager;
		private bool _isPaused = false;
		private bool _willPauseNextFrame = false;
		private bool _willResumeNextFrame = false;
		private bool _willAdvanceOneFrame = false;
		private bool _isAdvancingFrameByFrame = false;
		private bool _isSlowMotion = false;

		public bool debugMode => _debugMode;
		public bool isPaused => _isPaused;
		public bool isAdvancingFrameByFrame => _isAdvancingFrameByFrame;
		public InputManager input => _input;
		public Scene scene => _sceneManager?.scene ?? Scene.None;

		private void Update()
		{
			_input.mode = SimulatedControlMode.Simulate;
			if (_debugMode)
			{
				// Advance one frame
				if (_willAdvanceOneFrame)
				{
					_willAdvanceOneFrame = false;
					_isPaused = false;
					_willPauseNextFrame = true;
					Time.timeScale = 0f;
					_isAdvancingFrameByFrame = true;
				}
				// Resume the game
				else if (_willResumeNextFrame)
				{
					_willResumeNextFrame = false;
					_isPaused = false;
				}
				// Pause the game
				else if (_willPauseNextFrame)
				{
					_willPauseNextFrame = false;
					_isPaused = true;
					_isAdvancingFrameByFrame = false;
				}
				// Queue pausing for next frame (when timeScale will take effect)
				if ((_input.togglePause.justPressed || (_input.nextFrame.justPressed && !_isPaused)) && !_willPauseNextFrame && !_willResumeNextFrame)
				{
					if (_isPaused)
					{
						_willResumeNextFrame = true;
						Time.timeScale = _isSlowMotion ? 0.1f : 1f;
					}
					else
					{
						_willPauseNextFrame = true;
						Time.timeScale = 0f;
					}
				}
				// Advance one frame
				else if (_input.nextFrame.justPressed && !_willPauseNextFrame)
				{
					_willAdvanceOneFrame = true;
					Time.timeScale = 1f;
				}
				// Slow down time
				_isSlowMotion = _input.slowTime.isHeld;
				if (!_willPauseNextFrame && !_isPaused && !_willAdvanceOneFrame)
				{
					Time.timeScale = _isSlowMotion ? 0.1f : 1f;
				}
				// Quit the game
				if (_input.start.justPressed)
				{
					Application.Quit();
				}
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