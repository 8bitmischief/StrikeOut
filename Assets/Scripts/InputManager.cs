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

		public SimulatedButtonControl swingNorth => _swingNorth;
		public SimulatedButtonControl swingEast => _swingEast;
		public SimulatedButtonControl swingSouth => _swingSouth;
		public SimulatedButtonControl swingWest => _swingWest;
		public SimulatedButtonControl dodgeLeft => _dodgeLeft;
		public SimulatedButtonControl dodgeRight => _dodgeRight;
		public SimulatedButtonControl pause => _pause;

		public SimulatedControlMode mode {
			get => _mode;
			set {
				_mode = value;
				foreach (SimulatedButtonControl control in buttonControls)
					control.mode = _mode;
			}
		}

		private List<SimulatedButtonControl> buttonControls;
		private SimulatedControlMode _mode = SimulatedControlMode.PassThrough;

		private void Awake () {
			buttonControls = new List<SimulatedButtonControl>()
				{ swingNorth, swingEast, swingSouth, swingWest, dodgeLeft, dodgeRight, pause };
		}

		public void ConsumeInstantaneousInputs () {
			foreach (SimulatedButtonControl control in buttonControls)
				control.ConsumeInstantaneousInputs();
		}

		public void SimulateUpdate () => SimulateUpdate(Time.deltaTime);

		public void SimulateUpdate (float deltaTime) {
			foreach (SimulatedButtonControl control in buttonControls)
				control.SimulateUpdate(deltaTime);
		}
	}
}