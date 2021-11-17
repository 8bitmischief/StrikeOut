using UnityEngine;

namespace StrikeOut {
	public class Billboard : MonoBehaviour {
		[SerializeField] private bool xRotation = true;
		[SerializeField] private bool yRotation = false;
		[SerializeField] private bool zRotation = false;

#pragma warning disable CS0109 // Ignore "does not hide an accessible" warning during builds
		private new Camera camera;
#pragma warning restore CS0109

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