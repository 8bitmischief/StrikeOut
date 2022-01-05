using UnityEngine;
using UnityEditor;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public abstract class HitDetectionBox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;
		[SerializeField] private Color _color = Color.black;
		[SerializeField] private bool _isActive = false;
		[SerializeField, Range(0f, 1f)] private float _activeAmount;
		[SerializeField, Range(0f, 1f)] private float _inactiveAmount;
		private bool _prevIsActive;
		private bool _prevWillBeActive;
		private float _prevActiveAmount;
		private float _prevInactiveAmount;
		private int _estimatedFramesUntilActive = -1;
		private int _estimatedFramesUntilInactive = -1;

		public bool isActive => _isActive || (_activeAmount >= 1f && _inactiveAmount < 1f);
		public bool willBeActive => _estimatedFramesUntilActive >= 0;
		public bool willBeInactive => _estimatedFramesUntilInactive >= 0;
		public int framesUntilActive => _estimatedFramesUntilActive;
		public int framesUntilInactive => _estimatedFramesUntilInactive;
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		protected virtual void OnEnable()
		{
			_prevIsActive = isActive;
			_prevWillBeActive = willBeActive;
			_prevActiveAmount = _activeAmount;
			_prevInactiveAmount = _inactiveAmount;
			if (isActive || willBeActive)
				Register();
		}

		protected virtual void OnDisable()
		{
			_prevIsActive = isActive;
			_prevWillBeActive = willBeActive;
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
				if (_activeAmount > _prevActiveAmount && _activeAmount != 1f)
					_estimatedFramesUntilActive = Mathf.RoundToInt((1f - _activeAmount) / (_activeAmount - _prevActiveAmount));
				else
					_estimatedFramesUntilActive = -1;
				if (_inactiveAmount > _prevInactiveAmount && _inactiveAmount != 1f)
					_estimatedFramesUntilInactive = Mathf.RoundToInt((1f - _inactiveAmount) / (_inactiveAmount - _prevInactiveAmount));
				else
					_estimatedFramesUntilInactive = -1;
				if ((isActive || willBeActive) && !(_prevIsActive || _prevWillBeActive))
					Register();
				else if (!(isActive || willBeActive) && (_prevIsActive || _prevWillBeActive))
					Unregister();
				_prevIsActive = isActive;
				_prevWillBeActive = willBeActive;
				_prevActiveAmount = _activeAmount;
				_prevInactiveAmount = _inactiveAmount;
			}
		}

		public bool WillBeActive(int frames)
		{
			return (isActive || (_estimatedFramesUntilActive != -1 && _estimatedFramesUntilActive <= frames)) &&
				(_estimatedFramesUntilInactive == -1 || _estimatedFramesUntilInactive > frames);
		}

		protected abstract void Register();

		protected abstract void Unregister();

		private void OnDrawGizmos()
		{
			if (isActive)
				DrawBox();
		}

		private void OnDrawGizmosSelected()
		{
			if (!isActive && Selection.activeGameObject == gameObject)
				DrawBox();
		}

		private void DrawBox()
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = new Color(_color.r, _color.g, _color.b, _color.a * 0.35f);
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.color = _color;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = matrix;
		}
	}
}