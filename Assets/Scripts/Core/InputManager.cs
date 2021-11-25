using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Input.Control;

namespace StrikeOut
{
	public class InputManager : SharedUnityMischief.Input.InputManager
	{
		[Header("Controls")]
		public SimulatedButtonControl swingNorth;
		public SimulatedButtonControl swingEast;
		public SimulatedButtonControl swingSouth;
		public SimulatedButtonControl swingWest;
		public SimulatedButtonControl dodgeLeft;
		public SimulatedButtonControl dodgeRight;
		public SimulatedButtonControl start;

		[Header("Debug Controls")]
		public ButtonControl togglePause;
		public ButtonControl nextFrame;
		public ButtonControl slowTime;
		public ButtonControl alternateMode;

		public SimulatedControlMode mode
		{
			get => _mode;
			set
			{
				_mode = value;
				foreach (SimulatedButtonControl control in gameplayControls)
				{
					control.mode = _mode;
				}
			}
		}

		private List<SimulatedButtonControl> gameplayControls;
		private SimulatedControlMode _mode = SimulatedControlMode.PassThrough;

		private void Awake()
		{
			gameplayControls = new List<SimulatedButtonControl>()
			{
				swingNorth, swingEast, swingSouth, swingWest, dodgeLeft, dodgeRight, start
			};
		}

		public void ConsumeInstantaneousInputs()
		{
			foreach (SimulatedButtonControl control in gameplayControls)
			{
				control.ConsumeInstantaneousInputs();
			}
		}

		public void SimulateUpdate() => SimulateUpdate(Time.deltaTime);

		public void SimulateUpdate(float deltaTime)
		{
			foreach (SimulatedButtonControl control in gameplayControls)
			{
				control.SimulateUpdate(deltaTime);
			}
		}
	}
}