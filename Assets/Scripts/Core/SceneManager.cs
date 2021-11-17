using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut {
	public interface ISceneManager {
		Scene scene { get; }
	}

	public abstract class SceneManager<T> : SingletonMonoBehaviour<T>, ISceneManager where T : MonoBehaviour {
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