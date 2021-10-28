using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public class Toolbox : SingletonMonoBehaviour<Toolbox> {
		public static InputManager input => I.inputManager;

		[Header("Managers")]
		[SerializeField] private InputManager inputManager;
	}
}