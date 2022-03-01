using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public abstract class HitDetectionBox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;
		[SerializeField] private bool _isActive = false;
		private bool _prevIsActive;

		public bool isActive => _isActive;
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		protected virtual void OnEnable()
		{
			_prevIsActive = isActive;
			if (isActive)
				Register();
		}

		protected virtual void OnDisable()
		{
			_prevIsActive = isActive;
			Unregister();
		}

		public override void UpdateState()
		{
			_collider.size = new Vector3(
				Mathf.Sign(transform.lossyScale.x),
				Mathf.Sign(transform.lossyScale.y),
				Mathf.Sign(transform.lossyScale.z));
			if (!UpdateLoop.I.isInterpolating)
			{
				if (isActive && !_prevIsActive)
					Register();
				else if (!isActive && _prevIsActive)
					Unregister();
				_prevIsActive = isActive;
			}
		}

		protected abstract void Register();

		protected abstract void Unregister();

		protected abstract void DrawGizmo();

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (isActive)
			{
				Matrix4x4 matrix = Gizmos.matrix;
				Gizmos.matrix = transform.localToWorldMatrix;
				DrawGizmo();
				Gizmos.matrix = matrix;
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (!isActive && Selection.activeGameObject == gameObject)
			{
				Matrix4x4 matrix = Gizmos.matrix;
				Gizmos.matrix = transform.localToWorldMatrix;
				DrawGizmo();
				Gizmos.matrix = matrix;
			}
		}
#endif
	}
}