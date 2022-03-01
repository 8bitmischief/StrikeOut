using UnityEngine;
using SharedUnityMischief.Input.Control;

namespace StrikeOut
{
	public class InputManager : SharedUnityMischief.Input.InputManager
	{
		[Header("Controls")]
		[SerializeField] private SimulatedButtonControl _swing;
		[SerializeField] private SimulatedButtonControl _dodgeLeft;
		[SerializeField] private SimulatedButtonControl _dodgeRight;
		[SerializeField] private Vector2Control _aim;
		[Header("Debug Controls")]
		[SerializeField] private ButtonControl _togglePause;
		[SerializeField] private ButtonControl _nextFrame;
		[SerializeField] private ButtonControl _slowTime;
		[SerializeField] private ButtonControl _forceQuit;
		private SimulatedControlMode _mode = SimulatedControlMode.PassThrough;

		public IButtonControl swing => _swing;
		public IButtonControl dodgeLeft => _dodgeLeft;
		public IButtonControl dodgeRight => _dodgeRight;
		public IVector2Control aim => _aim;
		public IButtonControl togglePause => _togglePause;
		public IButtonControl nextFrame => _nextFrame;
		public IButtonControl slowTime => _slowTime;
		public IButtonControl forceQuit => _forceQuit;
		public SimulatedControlMode mode
		{
			get => _mode;
			set
			{
				_mode = value;
				_swing.mode = _mode;
				_dodgeLeft.mode = _mode;
				_dodgeRight.mode = _mode;
			}
		}

		public void ConsumeInstantaneousInputs()
		{
			_swing.ConsumeInstantaneousInputs();
			_dodgeLeft.ConsumeInstantaneousInputs();
			_dodgeRight.ConsumeInstantaneousInputs();
		}

		public void SimulateUpdate() => SimulateUpdate(Time.deltaTime);

		public void SimulateUpdate(float deltaTime)
		{
			_swing.SimulateUpdate(deltaTime);
			_dodgeLeft.SimulateUpdate(deltaTime);
			_dodgeRight.SimulateUpdate(deltaTime);
		}
	}
}