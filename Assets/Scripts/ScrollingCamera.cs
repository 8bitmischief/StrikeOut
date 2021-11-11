using UnityEngine;

namespace StrikeOut {
	[RequireComponent(typeof(Camera))]
	public class ScrollingCamera : MonoBehaviour {
		[SerializeField] private bool refreshEveryFrame = false;

		public Vector2 scroll = new Vector2(0f, 0f);

		private new Camera camera;
		private Vector2 lastRenderedScroll;
		private Vector2 lastRenderedScreenSize;
		private float lastRenderedFieldOfView;

		private void Awake () {
			camera = GetComponent<Camera>();
		}

		private void LateUpdate() {
			if (lastRenderedScroll != scroll || lastRenderedScreenSize.x != Screen.width || lastRenderedScreenSize.y != Screen.height || lastRenderedFieldOfView != camera.fieldOfView || refreshEveryFrame) {
				// Reset the projection matrix so any automatic field of view changes can take effect
				if (lastRenderedScreenSize.x != Screen.width || lastRenderedScreenSize.y != Screen.height || lastRenderedFieldOfView != camera.fieldOfView || refreshEveryFrame)
					camera.ResetProjectionMatrix();
				// Refresh the camera's projection matrix
				ApplyScrollToMatrix();
				lastRenderedScroll = scroll;
				lastRenderedScreenSize = new Vector2(Screen.width, Screen.height);
				lastRenderedFieldOfView = camera.fieldOfView;
			}
		}

		private void ApplyScrollToMatrix () {
			Matrix4x4 matrix = camera.projectionMatrix;
			// Do some perspective math to scroll the camera around
			var w = 2f * camera.nearClipPlane / matrix.m00;
			var h = 2f * camera.nearClipPlane / matrix.m11;
			var left = -w / 2f + scroll.x / 100f;
			var right = left + w;
			var bottom = -h / 2f + scroll.y / 100f;
			var top = bottom + h;
			float near = camera.nearClipPlane;
			float far = camera.farClipPlane;
			float x = (2f * near) / (right - left);
			float y = (2f * near) / (top - bottom);
			float a = (right + left) / (right - left);
			float b = (top + bottom) / (top - bottom);
			float c = -(far + near) / (far - near);
			float d = -(2f * far * near) / (far - near);
			float e = -1f;
			// Generate and set a new projection matrix
			matrix[0,0] = x;	matrix[0,1] = 0f;	matrix[0,2] = a;	matrix[0,3] = 0f;
			matrix[1,0] = 0f;	matrix[1,1] = y;	matrix[1,2] = b;	matrix[1,3] = 0f;
			matrix[2,0] = 0f;	matrix[2,1] = 0f;	matrix[2,2] = c;	matrix[2,3] = d;
			matrix[3,0] = 0f;	matrix[3,1] = 0f;	matrix[3,2] = e;	matrix[3,3] = 0f;
			camera.projectionMatrix = matrix;
		}
	}
}