using UnityEngine;

namespace StrikeOut {
	public class Shadow : MonoBehaviour {
		private void LateUpdate () {
			transform.position = new Vector3(
				transform.position.x,
				0.01f,
				transform.position.z);
		}
	}
}