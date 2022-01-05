using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BallAnimator))]
	public class Ball : AnimatedEntity<BallAnimator, string>
	{
		[SerializeField] private EnemyHurtbox _hurtbox;
		[SerializeField, Range(0f, 1f)] private float _passBattingLine = 0f;
		private float _prevPassBattingLine = 0f;
		private int _framesSincePassedBattingLine = -1;
		private int _estimatedFramesUntilPassBattingLine = -1;

		public StrikeZone strikeZone => _hurtbox.strikeZone;
		public bool isHittable => _hurtbox.isActive;
		public bool willBeHittable => framesUntilHittable > 0;
		public int framesUntilHittable => _hurtbox.framesUntilActive;
		public int framesUntilUnhittable => _hurtbox.framesUntilInactive;
		public bool hasPassedBattingLine => _passBattingLine == 1f;
		public bool willPassBattingLine => 0f < _passBattingLine && _passBattingLine < 1f;
		public int framesUntilPassBattingLine => _estimatedFramesUntilPassBattingLine;
		public int framesSincePassedBattingLine => _framesSincePassedBattingLine;

		public override void ResetComponent()
		{
			_passBattingLine = 0f;
			_hurtbox.strikeZone = StrikeZone.None;
			_prevPassBattingLine = 0f;
			_framesSincePassedBattingLine = -1;
			_estimatedFramesUntilPassBattingLine = -1;
		}

		public override void OnSpawn()
		{
			Scene.I.entityManager.balls.Add(this);
		}

		public override void UpdateState()
		{
			if (!Scene.I.updateLoop.isInterpolating)
			{
				if (_passBattingLine == 1f)
					_framesSincePassedBattingLine++;
				else
					_framesSincePassedBattingLine = -1;
				if (_passBattingLine > _prevPassBattingLine && _passBattingLine != 1f)
					_estimatedFramesUntilPassBattingLine = Mathf.RoundToInt((1f - _passBattingLine) / (_passBattingLine - _prevPassBattingLine));
				else
					_estimatedFramesUntilPassBattingLine = -1;
				_prevPassBattingLine = _passBattingLine;
				if ((animation == "Pitch" || animation == "Hit") && hasAnimationCompleted)
					DespawnEntity(this);
			}
		}

		public override void OnDespawn()
		{
			Scene.I.entityManager.balls.Remove(this);
		}

		public void Pitch(PitchType pitchType, StrikeZone strikeZone) => Pitch(pitchType, strikeZone, Scene.I.locations.strikeZone[strikeZone]);
		public void Pitch(PitchType pitchType, Vector3 target) => Pitch(pitchType, StrikeZone.None, target);
		private void Pitch(PitchType pitchType, StrikeZone strikeZone, Vector3 target)
		{
			_hurtbox.strikeZone = strikeZone;
			animator.Pitch(pitchType, target, strikeZone != StrikeZone.None);
		}

		public void Hit(Vector3 target)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
			animator.Hit(target);
		}
	}
}