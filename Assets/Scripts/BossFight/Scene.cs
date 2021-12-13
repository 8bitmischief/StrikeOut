using UnityEngine;
using SharedUnityMischief.Input.Control;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	[DefaultExecutionOrder(-20)]
	public class Scene : SceneManager<Scene>
	{
		[Header("Scene Config")]
		[SerializeField] private bool _syncFrameAdvancesWithGame = false;
		[Header("Children")]
		[SerializeField] private UpdateLoop _updateLoop;
		[SerializeField] private EntityManager _entityManager;
		[SerializeField] private Locations _locations;
		[Header("Data")]
		[SerializeField] private PitchDataObject _pitchData;

		public UpdateLoop updateLoop => _updateLoop;
		public EntityManager entityManager => _entityManager;
		public Locations locations => _locations;
		public PitchDataObject pitchData => _pitchData;

		private void OnEnable()
		{
			_updateLoop.onPreUpdateState += OnPreUpdateState;
			_updateLoop.onPostUpdateState += OnPostUpdateState;
		}

		private void Update()
		{
			if (!Game.I.isPaused)
			{
				if (Game.I.isAdvancingFrameByFrame && !_syncFrameAdvancesWithGame)
					_updateLoop.AdvanceOneFrame();
				else
					_updateLoop.Advance();
			}
		}

		private void OnDisable()
		{
			_updateLoop.onPreUpdateState -= OnPreUpdateState;
			_updateLoop.onPostUpdateState -= OnPostUpdateState;
		}

		private void OnPreUpdateState()
		{
			if (Game.I.input.mode == SimulatedControlMode.Simulate)
				Game.I.input.SimulateUpdate();
		}

		private void OnPostUpdateState()
		{
			Game.I.input.ConsumeInstantaneousInputs();
		}
	}
}