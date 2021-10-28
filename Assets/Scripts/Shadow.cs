using UnityEngine;

namespace StrikeOut {
	public class Shadow : MonoBehaviour {
		[Header("Children")]
		[SerializeField] private Transform visuals;

		private void LateUpdate () {
			transform.position = new Vector3(
				transform.position.x,
				0.01f,
				transform.position.z);
		}
	}
}