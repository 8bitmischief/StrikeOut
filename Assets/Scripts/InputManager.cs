using UnityEngine;
using SharedUnityMischief.Input.Control;

namespace StrikeOut {
	public class InputManager : SharedUnityMischief.Input.InputManager {
		[Header("Controls")]
		[SerializeField] private ButtonControl _swingNorth;
		[SerializeField] private ButtonControl _swingEast;
		[SerializeField] private ButtonControl _swingSouth;
		[SerializeField] private ButtonControl _swingWest;
		[SerializeField] private ButtonControl _dodgeLeft;
		[SerializeField] private ButtonControl _dodgeRight;
		[SerializeField] private ButtonControl _pause;

		public ButtonControl swingNorth => _swingNorth;
		public ButtonControl swingEast => _swingEast;
		public ButtonControl swingSouth => _swingSouth;
		public ButtonControl swingWest => _swingWest;
		public ButtonControl dodgeLeft => _dodgeLeft;
		public ButtonControl dodgeRight => _dodgeRight;
		public ButtonControl pause => _pause;
	}
}