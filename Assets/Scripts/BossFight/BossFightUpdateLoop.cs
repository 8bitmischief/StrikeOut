using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Lifecycle;
using SharedUnityMischief.Input.Control;

namespace StrikeOut.BossFight
{
	public class BossFightUpdateLoop : UpdateLoop
	{
		[Header("Managers")]
		[SerializeField] public EntityManager entityManager;

		protected override void UpdateState()
		{
			if (Game.I.input.mode == SimulatedControlMode.Simulate)
				Game.I.input.SimulateUpdate();
			entityManager.UpdateState();
			Game.I.input.ConsumeInstantaneousInputs();
		}

		public override void Pause()
		{
			Game.I.input.mode = SimulatedControlMode.Simulate;
			base.Pause();
		}

		public override void Resume()
		{
			Game.I.input.mode = SimulatedControlMode.PassThrough;
			base.Resume();
		}
	}
}
