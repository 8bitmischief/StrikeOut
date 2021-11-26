using UnityEngine;
using CameraShake;
using SharedUnityMischief.Effects;
using SharedUnityMischief.Entities.Animated;
using SharedUnityMischief.Pool;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BatterAnimator))]
	public class Batter : AnimatedEntity<Batter.State, BatterAnimator>
	{
		[SerializeField] private PrefabPool<ParticleEffect> _hitBallEffectPool;
		[SerializeField] private BounceShake.Params _hitBallShakeParams;
		private StrikeZone _strikeZone = StrikeZone.None;
		private Ball _targetBall = null;
		private bool _canCancelAnimation = false;
		private bool _isOnRightSide = false;

		public bool isOnRightSide => _isOnRightSide;

		private void Start()
		{
			_hitBallEffectPool.Prewarm();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_animator.onAllowAnimationCancels += OnAllowAnimationCancels;
			_animator.onTryHitBall += OnTryHitBall;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_animator.onAllowAnimationCancels -= OnAllowAnimationCancels;
			_animator.onTryHitBall -= OnTryHitBall;
		}

		public bool CanSwing(StrikeZone strikeZone)
		{
			switch (state)
			{
				case State.Swing:
				case State.SideStepEnd:
				case State.SwitchSides:
				case State.Settle:
					return _canCancelAnimation;
				case State.Idle:
					return true;
				default:
					return false;
			}
		}

		public bool CanDodgeLeft()
		{
			if (_isOnRightSide)
			{
				return CanSwitchSides() || CanEndSideStep();
			}
			else
			{
				return CanSideStep();
			}
		}

		public bool CanDodgeRight()
		{
			if (_isOnRightSide)
			{
				return CanSideStep();
			}
			else
			{
				return CanSwitchSides() || CanEndSideStep();
			}
		}

		public bool CanSwitchSides()
		{
			switch (state)
			{
				case State.Swing:
				case State.SideStepEnd:
				case State.SwitchSides:
				case State.Settle:
					return _canCancelAnimation;
				case State.Idle:
					return true;
				default:
					return false;
			}
		}

		public bool CanSideStep()
		{
			switch (state)
			{
				case State.Swing:
				case State.SideStepEnd:
				case State.SwitchSides:
				case State.Settle:
					return _canCancelAnimation;
				case State.Idle:
					return true;
				default:
					return false;
			}
		}
		
		public bool CanEndSideStep()
		{
			return state == State.SideStepStart && _canCancelAnimation;
		}

		public void Swing(StrikeZone strikeZone)
		{
			this._strikeZone = strikeZone;
			// Find the ball that's most worth considering for this swing
			Ball bestCandidateBall = null;
			foreach (Ball ball in BossFightScene.I.balls)
			{
				bool chooseClosestToBattingLine = false;
				if (bestCandidateBall == null)
				{
					bestCandidateBall = ball;
				}
				// Above all else, the best candidate is hittable
				else if (CouldSwingInTimeToHitBall(ball))
				{
					if (!CouldSwingInTimeToHitBall(bestCandidateBall))
					{
						bestCandidateBall = ball;
					}
					// Among hittable balls, the best candidate is in the right strike zone
					else if (ball.strikeZone == strikeZone)
					{
						if (bestCandidateBall.strikeZone != strikeZone)
						{
							bestCandidateBall = ball;
						}
						// Among hittable balls in the right strike zone, the best candidate is the one that will soonest become unhittable
						else if (ball.framesUntilUnhittable < bestCandidateBall.framesUntilUnhittable)
						{
							bestCandidateBall = ball;
						}
					}
					else if (bestCandidateBall.strikeZone != strikeZone)
					{
						// Among hittable balls in the wrong strike zone, the best candidate is closest to the batting line
						chooseClosestToBattingLine = true;
					}
				}
				else if (!CouldSwingInTimeToHitBall(bestCandidateBall))
					// Among unhittable balls, the best candidate is closest to the batting line
					chooseClosestToBattingLine = true;
				// Can't decide any other way--just choose the closest tothe batting line
				if (chooseClosestToBattingLine)
				{
					int framesFromBattingLine = ball.willPassBattingLine ? ball.framesUntilPassBattingLine : ball.framesSincePassedBattingLine;
					int bestCandidateFramesFromBattingLine = bestCandidateBall.willPassBattingLine ? bestCandidateBall.framesUntilPassBattingLine : bestCandidateBall.framesSincePassedBattingLine;
					if (framesFromBattingLine != -1 && (bestCandidateFramesFromBattingLine == -1 || framesFromBattingLine < bestCandidateFramesFromBattingLine))
					{
						bestCandidateBall = ball;
					}
				}
			}
			// Figure out if the ball can be swung at
			string swingResultsMessage = "";
			int swingStartupFrames = _animator.defaultSwingStartupFrames;
			bool tryingToHitBall = false;
			_targetBall = null;
			if (bestCandidateBall != null)
			{
				// Figure out if the swing was early or late (- is early, + is late)
				int framesEarlyOrLate;
				if (bestCandidateBall.willPassBattingLine)
				{
					framesEarlyOrLate = _animator.defaultSwingStartupFrames - bestCandidateBall.framesUntilPassBattingLine;
				}
				else
				{
					framesEarlyOrLate = _animator.defaultSwingStartupFrames + bestCandidateBall.framesSincePassedBattingLine;
				}
				if (CouldSwingInTimeToHitBall(bestCandidateBall))
				{
					tryingToHitBall = true;
					if (bestCandidateBall.strikeZone == strikeZone)
					{
						_targetBall = bestCandidateBall;
					}
					// The swing was late, speed up the animation
					if (framesEarlyOrLate > 0)
					{
						swingStartupFrames -= Mathf.FloorToInt((framesEarlyOrLate) / 2);
					}
					// The swing was early, slow down the animation
					else if (framesEarlyOrLate < 0)
					{
						swingStartupFrames += Mathf.FloorToInt((-framesEarlyOrLate) / 2);
					}
					// Keep the swing startup within actual limits
					int slowestPossibleSwingStartupFrames = Mathf.Min(_animator.slowestSwingStartupFrames, bestCandidateBall.framesUntilUnhittable - 1);
					int fastestPossibleSwingStartupFrames = Mathf.Max(_animator.fastestSwingStartupFrames, bestCandidateBall.isHittable ? 0 : bestCandidateBall.framesUntilHittable);
					swingStartupFrames = Mathf.Clamp(swingStartupFrames, fastestPossibleSwingStartupFrames, slowestPossibleSwingStartupFrames);
				}
				else if (CouldAlmostSwingInTimeToHitBall(bestCandidateBall))
				{
					tryingToHitBall = true;
				}
				if (tryingToHitBall)
				{
					swingResultsMessage = $"Swung at \"{bestCandidateBall.name}\"";
					if (framesEarlyOrLate > 0)
					{
						swingResultsMessage += $" {framesEarlyOrLate} {(framesEarlyOrLate == 1 ? "frame" : "frames")} late";
					}
					else if (framesEarlyOrLate < 0)
					{
						swingResultsMessage += $" {-framesEarlyOrLate} {(framesEarlyOrLate == -1 ? "frame" : "frames")} early";
					}
					else
					{
						swingResultsMessage += $" on the exact right frame";
					}
					swingResultsMessage += $"; {(CouldSwingInTimeToHitBall(bestCandidateBall) ? "HITTABLE" : "not hittable")}";
					swingResultsMessage += $"; {(bestCandidateBall.strikeZone == strikeZone ? "CORRECT strike zone" : "wrong strike zone")}";
				}
			}
			if (!tryingToHitBall)
			{
				swingResultsMessage = $"Swung at no ball in particular";
			}
			swingResultsMessage += $"; {swingStartupFrames} swing startup {(swingStartupFrames == 1 ? "frame" : "frames")}";
			Debug.Log(swingResultsMessage);
			// Perform the swing animation
			switch (strikeZone)
			{
				case StrikeZone.North:
					_animator.Swing(BatterAnimator.SwingDirection.North, swingStartupFrames);
					break;
				case StrikeZone.South:
					_animator.Swing(BatterAnimator.SwingDirection.South, swingStartupFrames);
					break;
				case StrikeZone.East:
				case StrikeZone.West:
					if (_isOnRightSide == (strikeZone == StrikeZone.East))
						_animator.Swing(BatterAnimator.SwingDirection.Inside, swingStartupFrames);
					else
						_animator.Swing(BatterAnimator.SwingDirection.Outside, swingStartupFrames);
					break;
			}
		}

		public void DodgeLeft()
		{
			if (_isOnRightSide)
			{
				if (state == State.SideStepStart)
				{
					EndSideStep();
				}
				else
				{
					SwitchSides();
				}
			}
			else
			{
				SideStep();
			}
		}

		public void DodgeRight()
		{
			if (_isOnRightSide)
			{
				SideStep();
			}
			else
			{
				if (state == State.SideStepStart)
				{
					EndSideStep();
				}
				else
				{
					SwitchSides();
				}
			}
		}

		public void SwitchSides()
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			_isOnRightSide = !_isOnRightSide;
			_animator.SwitchSides();
		}

		public void SideStep()
		{
			_animator.SideStep();
		}

		public void EndSideStep()
		{
			_animator.EndSideStep();
		}

		protected override void OnEnterState(State state)
		{
			// Calculate root motion
			switch (state)
			{
				case State.Settle:
				case State.SwitchSides:
				case State.SideStepEnd:
					_animator.SetRootMotion(_isOnRightSide ?
						BossFightScene.I.batterRightPosition :
						BossFightScene.I.batterLeftPosition);
					break;
				case State.SideStepStart:
					_animator.SetRootMotion(_isOnRightSide ?
						BossFightScene.I.batterDodgeRightPosition :
						BossFightScene.I.batterDodgeLeftPosition);
					break;
				case State.Swing:
					switch (_strikeZone)
					{
						case StrikeZone.North:
							if (_isOnRightSide)
							{
								_animator.SetRootMotion(BossFightScene.I.batterRightPosition + new Vector3(-0.9f, 0f, 0f));
							}
							else
							{
								_animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0.9f, 0f, 0f));
							}
							break;
						case StrikeZone.East:
							if (_isOnRightSide)
							{
								_animator.SetRootMotion(BossFightScene.I.batterRightPosition + new Vector3(0f, 0f, 0f));
							}
							else
							{
								_animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(2f, 0f, 0f));
							}
							break;
						case StrikeZone.South:
							if (_isOnRightSide)
							{
								_animator.SetRootMotion(BossFightScene.I.batterRightPosition + new Vector3(-0.5f, 0f, 0f));
							}
							else
							{
								_animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0.5f, 0f, 0f));
							}
							break;
						case StrikeZone.West:
							if (_isOnRightSide)
							{
								_animator.SetRootMotion(BossFightScene.I.batterRightPosition + new Vector3(-2f, 0f, 0f));
							}
							else
							{
								_animator.SetRootMotion(BossFightScene.I.batterLeftPosition + new Vector3(0f, 0f, 0f));
							}
							break;
					}
					break;
			}
		}

		protected override void OnLeaveState(State state)
		{
			_canCancelAnimation = false;
		}

		private void OnAllowAnimationCancels()
		{
			_canCancelAnimation = true;
		}

		private void OnTryHitBall()
		{
			if (_targetBall != null)
			{
				Vector3 targetPosition = new Vector3(15f, 5f, 50f);
				Vector3 shakeDirection;
				if (_targetBall.strikeZone == StrikeZone.North)
				{
					shakeDirection = new Vector3(1f, 0.3f, 0f);
				}
				else if (_targetBall.strikeZone == StrikeZone.South)
				{
					shakeDirection = new Vector3(1f, -0.3f, 0f);
				}
				else
				{
					shakeDirection = new Vector3(1f, 0f, 0f);
				}
				if (_isOnRightSide)
				{
					shakeDirection.x *= -1;
				}
				_targetBall.Hit(targetPosition);
				CameraShaker.Shake(new BounceShake(_hitBallShakeParams, new Displacement(shakeDirection, new Vector3(0f, 0f, 1f))));
				_hitBallEffectPool.Withdraw(new Vector3(_targetBall.transform.position.x, _targetBall.transform.position.y, 0f)).Play();
			}
			_targetBall = null;
		}

		private bool CouldSwingInTimeToHitBall(Ball ball) => CouldAlmostSwingInTimeToHitBall(ball, 0, 0);

		private bool CouldAlmostSwingInTimeToHitBall(Ball ball, int framesOfEarlyLeeway = 8, int framesOfLateLeeway = 6)
		{
			return (ball.isHittable || (ball.willBeHittable && ball.framesUntilHittable - framesOfEarlyLeeway <= _animator.slowestSwingStartupFrames)) &&
				ball.framesUntilUnhittable + framesOfLateLeeway > _animator.fastestSwingStartupFrames;
		}

		public enum State
		{
			None = 0,
			Idle = 1,
			SwitchSides = 2,
			SideStepStart = 3,
			SideStepEnd = 4,
			Swing = 5,
			Settle = 6
		}
	}
}