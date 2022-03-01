using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	public class BallSpawner : EntitySpawner<Ball> {
		[Header("Ball Config")]
		[SerializeField] private PitchType _pitchType = PitchType.None;
		[SerializeField] private Vector2 _target = Vector2.zero;

		public PitchType pitchType { get => _pitchType; set => _pitchType = value; }
		public Vector2 target { get => _target; set => _target = value; }

		protected override void OnSpawnChildEntity(Ball ball)
		{
			ball.Pitch(_pitchType, _target);
		}
	}
}