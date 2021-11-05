using UnityEngine;

namespace StrikeOut {
	public abstract class SceneManager : MonoBehaviour {
		public abstract Scene scene { get; }

		protected virtual void Start () {
			Game.I.RegisterSceneManager(this);
		}

		protected virtual void Destroy () {
			Game.I.UnregisterSceneManager(this);
		}

		public virtual void UpdateState () {}
	}
}