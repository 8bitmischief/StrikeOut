using UnityEngine;
using UnityEditor;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public abstract class HitDetectionBox : EntityComponent
	{
		[SerializeField] private BoxCollider _collider;
		[SerializeField] private bool _isActive = false;
		[SerializeField, Range(0f, 1f)] private float _activeAmount;
		[SerializeField, Range(0f, 1f)] private float _inactiveAmount;
		[SerializeField, Range(0f, 1f)] private float _idealHitAmount;
		private bool _prevIsActive;
		private bool _prevWillBeActive;
		private float _prevActiveAmount;
		private float _prevInactiveAmount;
		private float _prevIdealHitAmount;
		private int _estimatedFramesUntilActive = -1;
		private int _estimatedFramesUntilInactive = -1;
		private int _estimatedFramesUntilIdealHit = -1;
		private int _framesSinceIdealHit = -1;

		public bool isActive => _isActive || (_activeAmount >= 1f && _inactiveAmount < 1f);
		public bool willBeActive => _estimatedFramesUntilActive >= 0;
		public bool willBeInactive => _estimatedFramesUntilInactive >= 0;
		public bool hasIdealHit => _idealHitAmount > 0f;
		public int framesUntilActive => _estimatedFramesUntilActive;
		public int framesUntilInactive => _estimatedFramesUntilInactive;
		public int framesUntilIdealHit => _estimatedFramesUntilIdealHit;
		public int framesSinceIdealhit => _framesSinceIdealHit;
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder + 50;

		protected virtual void OnEnable()
		{
			_prevIsActive = isActive;
			_prevWillBeActive = willBeActive;
			_prevActiveAmount = _activeAmount;
			_prevInactiveAmount = _inactiveAmount;
			_prevIdealHitAmount = _idealHitAmount;
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
				if (_idealHitAmount > _prevIdealHitAmount && _idealHitAmount != 1f)
					_estimatedFramesUntilIdealHit = Mathf.RoundToInt((1f - _idealHitAmount) / (_idealHitAmount - _prevIdealHitAmount));
				else
					_estimatedFramesUntilIdealHit = -1;
				if (_idealHitAmount == 1f)
					_framesSinceIdealHit++;
				else
					_framesSinceIdealHit = -1;
				if ((isActive || willBeActive) && !(_prevIsActive || _prevWillBeActive))
					Register();
				else if (!(isActive || willBeActive) && (_prevIsActive || _prevWillBeActive))
					Unregister();
				_prevIsActive = isActive;
				_prevWillBeActive = willBeActive;
				_prevActiveAmount = _activeAmount;
				_prevInactiveAmount = _inactiveAmount;
				_prevIdealHitAmount = _idealHitAmount;
			}
		}

		public bool WillBeActive(int startFrame, int endFrame = -1)
		{
			if (endFrame == -1)
				endFrame = startFrame;
			return (isActive || (_estimatedFramesUntilActive != -1 && _estimatedFramesUntilActive <= endFrame)) &&
				(_estimatedFramesUntilInactive == -1 || _estimatedFramesUntilInactive > startFrame);
		}

		protected abstract void Register();

		protected abstract void Unregister();

		protected abstract void DrawGizmo();

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
	}
}