using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	public class BallSpawner : EntitySpawner {
		[SerializeField] private PitchType _pitchType = PitchType.None;
		[SerializeField] private StrikeZone _strikeZone = StrikeZone.None;

		public PitchType pitchType { get => _pitchType; set => _pitchType = value; }
		public StrikeZone strikeZone { get => _strikeZone; set => _strikeZone = value; }

		protected override void OnSpawnChild(Entity entity)
		{
			Ball ball = entity as Ball;
			ball.Pitch(_pitchType, _strikeZone);
		}
	}
}