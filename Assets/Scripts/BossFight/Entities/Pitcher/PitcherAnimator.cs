using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.Animation>
	{
		private static readonly int TravelStartHash = Animator.StringToHash("Travel Start");
		private static readonly int TravelBrakeHash = Animator.StringToHash("Travel Brake");
		private static readonly int TravelEndHash = Animator.StringToHash("Travel End");
		private static readonly int PitchHash = Animator.StringToHash("Pitch");
		private static readonly int LungeHash = Animator.StringToHash("Lunge");
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");

		[Header("Children")]
		[SerializeField] private Transform _spawnLocation;
		[SerializeField] private float _maxTravelSpeed = 500f;
		[SerializeField] private float _travelAcceleration = 1f;
		[SerializeField] private float _travelDeceleration = 1f;
		private Vector3 _targetPosition;
		private Vector3 _velocity;

		public event Action<Vector3> onSpawnBall;
		public event Action<Vector3> onSpawnBoomerang;

		public void Pitch() => Trigger(PitchHash);

		public void Lunge(Vector3 target) => Trigger(LungeHash, target);

		public void ThrowBoomerang() => Trigger(ThrowBoomerangHash);

		public void TravelTo(Vector3 targetPosition, float speed)
		{
			// Start accelerating
			_targetPosition = targetPosition;
			_velocity = Vector3.zero;
			Trigger(TravelStartHash);
		}

		public override void UpdateState()
		{
			base.UpdateState();
			if (!BossFightUpdateLoop.I.isInterpolating)
			{
				Vector3 vectorToTarget = _targetPosition - transform.position;
				float distanceToTarget = vectorToTarget.magnitude;
				float speed = _velocity.magnitude;
				switch (animation)
				{
					case Pitcher.Animation.TravelStart:
					case Pitcher.Animation.Travel:
						// Speed up
						speed = Mathf.Min(speed + _travelAcceleration, _maxTravelSpeed);
						_velocity = vectorToTarget.normalized * speed;
						float distanceTraveledIfBraking = (speed / _travelDeceleration + 1f) * speed / 2f * BossFightUpdateLoop.TimePerUpdate;
						// Begin braking when close to destination
						if (distanceToTarget <= distanceTraveledIfBraking)
						{
							Trigger(TravelBrakeHash);
						}
						break;
					case Pitcher.Animation.TravelBrake:
						// Brake
						if (speed > 0f)
						{
							_velocity *= Mathf.Max(speed - _travelDeceleration, 0f) / speed;
						}
						// Full stop reached
						int framesUntilFullyStopped = Mathf.FloorToInt(speed / _travelDeceleration);
						if (framesUntilFullyStopped <= 12)
						{
							Trigger(TravelEndHash, _targetPosition);
						}
						break;
				}
			}
			// Apply velocity
			switch (animation)
			{
				case Pitcher.Animation.TravelStart:
				case Pitcher.Animation.Travel:
				case Pitcher.Animation.TravelBrake:
					transform.position += _velocity * BossFightUpdateLoop.I.deltaTime;
					break;
			}
		}

		protected override void OnAnimationEvent(AnimationEvent evt)
		{
			switch (evt.stringParameter)
			{
				case "Pitch Ball":
					onSpawnBall?.Invoke(_spawnLocation.transform.position);
					break;
				case "Throw Boomerang":
					onSpawnBoomerang?.Invoke(_spawnLocation.transform.position);
					break;
			}
		}
	}
}