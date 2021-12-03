using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Input.Control;

namespace StrikeOut
{
	public class InputManager : SharedUnityMischief.Input.InputManager
	{
		[Header("Controls")]
		[SerializeField] private SimulatedButtonControl _swingNorth;
		[SerializeField] private SimulatedButtonControl _swingEast;
		[SerializeField] private SimulatedButtonControl _swingSouth;
		[SerializeField] private SimulatedButtonControl _swingWest;
		[SerializeField] private SimulatedButtonControl _dodgeLeft;
		[SerializeField] private SimulatedButtonControl _dodgeRight;
		[Header("Debug Controls")]
		[SerializeField] private ButtonControl _togglePause;
		[SerializeField] private ButtonControl _nextFrame;
		[SerializeField] private ButtonControl _slowTime;
		[SerializeField] private ButtonControl _forceQuit;
		private SimulatedControlMode _mode = SimulatedControlMode.PassThrough;
		private List<SimulatedButtonControl> _gameplayControls;

		public SimulatedButtonControl swingNorth => _swingNorth;
		public SimulatedButtonControl swingEast => _swingEast;
		public SimulatedButtonControl swingSouth => _swingSouth;
		public SimulatedButtonControl swingWest => _swingWest;
		public SimulatedButtonControl dodgeLeft => _dodgeLeft;
		public SimulatedButtonControl dodgeRight => _dodgeRight;
		public ButtonControl togglePause => _togglePause;
		public ButtonControl nextFrame => _nextFrame;
		public ButtonControl slowTime => _slowTime;
		public ButtonControl forceQuit => _forceQuit;
		public SimulatedControlMode mode
		{
			get => _mode;
			set
			{
				_mode = value;
				foreach (SimulatedButtonControl control in _gameplayControls)
				{
					control.mode = _mode;
				}
			}
		}

		private void Awake()
		{
			_gameplayControls = new List<SimulatedButtonControl>()
			{
				swingNorth, swingEast, swingSouth, swingWest, dodgeLeft, dodgeRight
			};
		}

		public void ConsumeInstantaneousInputs()
		{
			foreach (SimulatedButtonControl control in _gameplayControls)
			{
				control.ConsumeInstantaneousInputs();
			}
		}

		public void SimulateUpdate() => SimulateUpdate(Time.deltaTime);

		public void SimulateUpdate(float deltaTime)
		{
			foreach (SimulatedButtonControl control in _gameplayControls)
			{
				control.SimulateUpdate(deltaTime);
			}
		}
	}
}