using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public class BossFightManager : SingletonMonoBehaviour<BossFightManager> {
		[Header("Prefabs")]
		[SerializeField] private Ball ballPrefab;

		public static Ball SpawnBall (Vector3 position) {
			Ball ballPrefab = I.ballPrefab;
			Ball ball = Instantiate(ballPrefab, position, Quaternion.identity);
			ball.name = ballPrefab.name;
			return ball;
		}
	}
}