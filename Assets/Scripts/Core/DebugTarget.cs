using UnityEngine;

namespace StrikeOut
{
	public class DebugTarget : MonoBehaviour
	{
		[SerializeField] private Color _color = Color.yellow;
		[SerializeField] private bool _outline = false;
		[SerializeField] private bool _fill = true;
		[SerializeField] private Mesh _mesh;

		private void OnDrawGizmos()
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = transform.localToWorldMatrix;
			if (_fill)
			{
				Gizmos.color = new Color(_color.r, _color.g, _color.b, _color.a * 0.35f);
				if (_mesh == null)
					Gizmos.DrawCube(Vector3.zero, Vector3.one);
				else
					Gizmos.DrawMesh(_mesh, Vector3.zero, Quaternion.identity, Vector3.one);
			}
			Gizmos.color = _color;
			if (_outline)
			{
				if (_mesh == null)
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				else
					Gizmos.DrawWireMesh(_mesh, Vector3.zero, Quaternion.identity, Vector3.one);
			}
			Gizmos.matrix = matrix;
		}
	}
}