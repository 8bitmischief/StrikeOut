using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut
{
	public interface ISceneManager
	{
		SceneId sceneId { get; }
	}

	public abstract class SceneManager<T> : SingletonMonoBehaviour<T>, ISceneManager where T : MonoBehaviour
	{
		[Header("Scene Config")]
		[SerializeField] private SceneId _sceneId = SceneId.None;

		public SceneId sceneId => _sceneId;

		protected virtual void Start()
		{
			Game.I.RegisterSceneManager(this);
		}

		protected override void OnDestroy()
		{
			if (Game.hasInstance)
				Game.I.UnregisterSceneManager(this);
			base.OnDestroy();
		}

		public virtual void UpdateState() {}
	}
}