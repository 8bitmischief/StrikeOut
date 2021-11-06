using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Input.Control;

namespace StrikeOut {
	public class InputManager : SharedUnityMischief.Input.InputManager {
		[Header("Controls")]
		[SerializeField] private SimulatedButtonControl _swingNorth;
		[SerializeField] private SimulatedButtonControl _swingEast;
		[SerializeField] private SimulatedButtonControl _swingSouth;
		[SerializeField] private SimulatedButtonControl _swingWest;
		[SerializeField] private SimulatedButtonControl _dodgeLeft;
		[SerializeField] private SimulatedButtonControl _dodgeRight;
		[SerializeField] private SimulatedButtonControl _pause;

		[Header("Debug Controls")]
		[SerializeField] private ButtonControl _togglePause;
		[SerializeField] private ButtonControl _nextFrame;
		[SerializeField] private ButtonControl _slowTime;

		public SimulatedButtonControl swingNorth => _swingNorth;
		public SimulatedButtonControl swingEast => _swingEast;
		public SimulatedButtonControl swingSouth => _swingSouth;
		public SimulatedButtonControl swingWest => _swingWest;
		public SimulatedButtonControl dodgeLeft => _dodgeLeft;
		public SimulatedButtonControl dodgeRight => _dodgeRight;
		public SimulatedButtonControl pause => _pause;
		public ButtonControl togglePause => _togglePause;
		public ButtonControl nextFrame => _nextFrame;
		public ButtonControl slowTime => _slowTime;
		public SimulatedControlMode mode {
			get => _mode;
			set {
				_mode = value;
				foreach (SimulatedButtonControl control in gameplayControls)
					control.mode = _mode;
			}
		}

		private List<SimulatedButtonControl> gameplayControls;
		private SimulatedControlMode _mode = SimulatedControlMode.PassThrough;

		private void Awake () {
			gameplayControls = new List<SimulatedButtonControl>()
				{ swingNorth, swingEast, swingSouth, swingWest, dodgeLeft, dodgeRight, pause };
		}

		public void ConsumeInstantaneousInputs () {
			foreach (SimulatedButtonControl control in gameplayControls)
				control.ConsumeInstantaneousInputs();
		}

		public void SimulateUpdate () => SimulateUpdate(Time.deltaTime);

		public void SimulateUpdate (float deltaTime) {
			foreach (SimulatedButtonControl control in gameplayControls)
				control.SimulateUpdate(deltaTime);
		}
	}
}