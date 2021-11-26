using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut
{
	public interface ISceneManager
	{
		Scene scene { get; }
	}

	public abstract class SceneManager<T> : SingletonMonoBehaviour<T>, ISceneManager where T : MonoBehaviour
	{
		[Header("Scene Config")]
		[SerializeField] private Scene _scene = Scene.None;

		public Scene scene => _scene;

		protected virtual void Start()
		{
			Game.I.RegisterSceneManager(this);
		}

		protected override void OnDestroy()
		{
			if (Game.hasInstance)
			{
				Game.I.UnregisterSceneManager(this);
			}
			base.OnDestroy();
		}

		public virtual void UpdateState() {}
	}
}