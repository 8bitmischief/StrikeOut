using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	public class BallSpawner : EntitySpawner {
		[SerializeField] private PitchType _pitchType = PitchType.None;
		[SerializeField] private StrikeZone _strikeZone = StrikeZone.None;

		protected override void OnSpawnChild(Entity entity)
		{
			Ball ball = entity as Ball;
			ball.Pitch(_pitchType, _strikeZone);
		}
	}
}