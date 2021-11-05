using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(EntityManager))]
	public class BaseballSceneManager : SceneManager {
		public override Scene scene => Scene.Baseball;

		private EntityManager entityManager;

		private void Awake () {
			entityManager = GetComponent<EntityManager>();
		}

		public override void UpdateState () {
			entityManager.UpdateState();
		}
	}
}