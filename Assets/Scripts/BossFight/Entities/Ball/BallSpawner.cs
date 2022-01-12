using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	public class BallSpawner : EntitySpawner<Ball> {
		[Header("Ball Config")]
		[SerializeField] private PitchType _pitchType = PitchType.None;
		[SerializeField] private StrikeZone _strikeZone = StrikeZone.None;

		public PitchType pitchType { get => _pitchType; set => _pitchType = value; }
		public StrikeZone strikeZone { get => _strikeZone; set => _strikeZone = value; }

		protected override void OnSpawnChildEntity(Ball ball)
		{
			ball.Pitch(_pitchType, _strikeZone);
		}
	}
}