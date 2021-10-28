using UnityEngine;

namespace StrikeOut {
	public class Billboard : MonoBehaviour {
		[SerializeField] private bool xRotation = true;
		[SerializeField] private bool yRotation = false;
		[SerializeField] private bool zRotation = false;

		private new Camera camera;

		private void Awake () {
			camera = Camera.main;
		}

		private void LateUpdate () {
			Vector3 rotation = transform.eulerAngles;
			Vector3 cameraRotation = camera.transform.eulerAngles;
			transform.eulerAngles = new Vector3(
				xRotation ? cameraRotation.x : rotation.x,
				yRotation ? cameraRotation.y : rotation.y,
				zRotation ? cameraRotation.z : rotation.z);
		}
	}
}