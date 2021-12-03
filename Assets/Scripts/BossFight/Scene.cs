using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Input.Control;
using StrikeOut.BossFight.Data;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
	[DefaultExecutionOrder(-20)]
	public class Scene : SceneManager<Scene>
	{
		[Header("Children")]
		[SerializeField] private UpdateLoop _updateLoop;
		[SerializeField] private EntityManager _entityManager;
		[SerializeField] private Locations _locations;
		[Header("Data")]
		[SerializeField] private PitchDataObject _pitchData;
		private Batter _batter = null;
		private Pitcher _pitcher = null;
		private List<Ball> _balls = new List<Ball>();

		public UpdateLoop updateLoop => _updateLoop;
		public EntityManager entityManager => _entityManager;
		public Locations locations => _locations;
		public PitchDataObject pitchData => _pitchData;
		public Batter batter { get => _batter; set => _batter = value; }
		public Pitcher pitcher { get => _pitcher; set => _pitcher = value; }
		public List<Ball> balls => _balls;

		private void OnEnable()
		{
			_updateLoop.onPreUpdateState += OnPreUpdateState;
			_updateLoop.onPostUpdateState += OnPostUpdateState;
		}

		private void Update()
		{
			if (!Game.I.isPaused)
			{
				if (Game.I.isAdvancingFrameByFrame)
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