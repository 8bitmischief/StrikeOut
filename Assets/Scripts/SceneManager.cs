using UnityEngine;

namespace StrikeOut {
	public abstract class SceneManager : MonoBehaviour {
		[Header("Scene Config")]
		[SerializeField] private Scene _scene;

		public Scene scene => _scene;

		protected virtual void Start () {
			Game.I.RegisterSceneManager(this);
		}

		protected virtual void Destroy () {
			Game.I.UnregisterSceneManager(this);
		}

		public virtual void UpdateState () {}
	}
}