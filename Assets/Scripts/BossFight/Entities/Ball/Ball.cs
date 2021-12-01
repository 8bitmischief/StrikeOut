using UnityEngine;
using SharedUnityMischief.Lifecycle;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<BallAnimator, Ball.Animation>
	{
		private StrikeZone _strikeZone = StrikeZone.None;
		private PitchDataObject.PitchData _pitchData;
		private Vector3 _prevPosition = Vector3.zero;
		private Vector3 _velocity = Vector3.zero;
		private Vector3 _accelerationPerFrame = Vector3.zero;
		private bool _hasPassedBattingLine = false;
		private bool _justPassedBattingLine = false;
		private int _framesSincePassedBattingLine = -1;

		public StrikeZone strikeZone => _strikeZone;
		public bool isHittable
		{
			get
			{
				switch (animation)
				{
					case Animation.Pitched: return animationFrameDuration - animationFrame <= _pitchData.earlyHitFrames;
					case Animation.Missed: return animationFrame < _pitchData.lateHitFrames;
					default: return false;
				}
			}
		}
		public bool willBeHittable => framesUntilHittable > 0;
		public int framesUntilHittable
		{
			get
			{
				switch (animation)
				{
					case Animation.Pitched: return Mathf.Max(0, animationFrameDuration - animationFrame - _pitchData.earlyHitFrames);
					case Animation.Missed: return animationFrame < _pitchData.lateHitFrames ? 0 : -1;
					default: return -1;
				}
			}
		}
		public int framesUntilUnhittable
		{
			get
			{
				switch (animation)
				{
					case Animation.Pitched: return animationFrameDuration - animationFrame + _pitchData.lateHitFrames;
					case Animation.Missed: return _pitchData.lateHitFrames > animationFrame ? _pitchData.lateHitFrames - animationFrame : -1;
					default: return -1;
				}
			}
		}
		public bool hasPassedBattingLine => _hasPassedBattingLine;
		public bool willPassBattingLine => animation == Animation.Pitched;
		public int framesUntilPassBattingLine
		{
			get
			{
				if (animator == null)
				{
					return -1;
				}
				switch (animation)
				{
					case Animation.Pitched: return animationFrameDuration - animationFrame;
					default: return -1;
				}
			}
		}
		public int framesSincePassedBattingLine => _framesSincePassedBattingLine;

		public override void ResetComponent()
		{
			_strikeZone = StrikeZone.None;
			_hasPassedBattingLine = false;
			_framesSincePassedBattingLine = -1;
			_pitchData = null;
			_prevPosition = Vector3.zero;
			_velocity = Vector3.zero;
			_accelerationPerFrame = Vector3.zero;
			_justPassedBattingLine = false;
		}

		public override void OnSpawn()
		{
			_prevPosition = transform.position;
			BossFightScene.I.balls.Add(this);
		}

		public override void UpdateState()
		{
			if (!BossFightScene.I.updateLoop.isInterpolating && _hasPassedBattingLine)
			{
				if (_justPassedBattingLine)
				{
					_justPassedBattingLine = false;
				}
				else
				{
					_framesSincePassedBattingLine++;
				}
			}
			switch (animation)
			{
				case Animation.Pitched:
					// Keep track of the ball's velocity and acceleration (determined by animation and root motion)
					if (!BossFightScene.I.updateLoop.isInterpolating)
					{
						Vector3 newVelocity = (transform.position - _prevPosition) / UpdateLoop.TimePerUpdate;
						_accelerationPerFrame = newVelocity - _velocity;
						_velocity = newVelocity;
						_prevPosition = transform.position;
					}
					break;
				case Animation.Missed:
					if (_strikeZone == StrikeZone.None)
					{
						// Follow through from where the arc of the pitch left off
						if (!BossFightScene.I.updateLoop.isInterpolating)
						{
							_velocity += _accelerationPerFrame;
						}
						transform.position += _velocity * BossFightScene.I.updateLoop.deltaTime;
						// Despawn the ball once it's behind the camera
						if (!BossFightScene.I.updateLoop.isInterpolating && !isHittable && !willBeHittable && (transform.position.z < -15f || totalAnimationFrames > 20))
						{
							BossFightScene.I.entityManager.DespawnEntity(this);
						}
					}
					else
					{
						// Despawn the ball if the player doesn't hit it in time
						if (!BossFightScene.I.updateLoop.isInterpolating && !isHittable && !willBeHittable)
						{
							BossFightScene.I.entityManager.DespawnEntity(this);
						}
					}
					break;
				case Animation.Hit:
					if (!BossFightScene.I.updateLoop.isInterpolating && hasAnimationCompleted)
					{
						BossFightScene.I.entityManager.DespawnEntity(this);
					}
					break;
			}
		}

		public override void OnDespawn()
		{
			BossFightScene.I.balls.Remove(this);
		}

		public void Pitch(PitchType pitchType, StrikeZone strikeZone) => Pitch(pitchType, strikeZone, BossFightScene.I.GetStrikeZonePosition(strikeZone));

		public void Pitch(PitchType pitchType, Vector3 target) => Pitch(pitchType, StrikeZone.None, target);

		protected override void OnStartAnimation(Animation animation)
		{
			switch (animation)
			{
				case Animation.Missed:
					_hasPassedBattingLine = true;
					_justPassedBattingLine = true;
					_framesSincePassedBattingLine = 0;
					break;
			}
		}

		private void Pitch(PitchType pitchType, StrikeZone strikeZone, Vector3 target)
		{
			_strikeZone = strikeZone;
			_pitchData = BossFightScene.I.pitchData.pitches[pitchType];
			animator.Pitch(pitchType, target, strikeZone != StrikeZone.None);
		}

		public void Hit(Vector3 target)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
			animator.Hit(target);
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Pitched = 2,
			Hit = 3,
			Missed = 4
		}
	}
}