using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
	public class BossFightScene : SceneManager<BossFightScene>
	{
		[Header("Locations")]
		[SerializeField] private Transform batterLeft;
		[SerializeField] private Transform batterDodgeLeft;
		[SerializeField] private Transform batterRight;
		[SerializeField] private Transform batterDodgeRight;
		[SerializeField] private Transform northStrikeZone;
		[SerializeField] private Transform eastStrikeZone;
		[SerializeField] private Transform southStrikeZone;
		[SerializeField] private Transform westStrikeZone;

		[Header("Children")]
		public BossFightUpdateLoop updateLoop;

		[Header("Data")]
		public PitchDataObject pitchData;

		public Vector3 batterLeftPosition => batterLeft.position;
		public Vector3 batterDodgeLeftPosition => batterDodgeLeft.position;
		public Vector3 batterRightPosition => batterRight.position;
		public Vector3 batterDodgeRightPosition => batterDodgeRight.position;
		public Vector3 northStrikeZonePosition => northStrikeZone.position;
		public Vector3 eastStrikeZonePosition => eastStrikeZone.position;
		public Vector3 southStrikeZonePosition => southStrikeZone.position;
		public Vector3 westStrikeZonePosition => westStrikeZone.position;
		public EntityManager entityManager => updateLoop.entityManager;

		public List<Ball> balls { get; private set; } = new List<Ball>();

		private void Update()
		{
			// Pause the game
			if (Game.I.input.togglePause.justPressed)
			{
				if (updateLoop.isPaused)
				{
					updateLoop.Resume();
				}
				else if (Game.I.debugMode)
				{
					updateLoop.Pause();
				}
			}
			// Step through individual frames
			if (Game.I.input.nextFrame.justPressed && Game.I.debugMode)
			{
				if (!updateLoop.isPaused)
				{
					updateLoop.Pause();
				}
				if (Game.I.input.alternateMode.isHeld)
				{
					updateLoop.Advance(0.018f, true);
				}
				else
				{
					updateLoop.AdvanceOneFrame(true);
				}
			}
			// Slow down time
			if (Game.I.input.slowTime.justReleased || Game.I.input.slowTime.justPressed)
				Time.timeScale = Game.I.input.slowTime.isHeld ? 0.10f : 1.00f;
			// Update the game
			if (!updateLoop.updateAutomatically)
			{
				updateLoop.Advance();
			}
		}

		public Vector3 GetStrikeZonePosition(StrikeZone strikeZone)
		{
			switch (strikeZone)
			{
				case StrikeZone.North: return northStrikeZone.position;
				case StrikeZone.East: return eastStrikeZone.position;
				case StrikeZone.South: return southStrikeZone.position;
				case StrikeZone.West: return westStrikeZone.position;
				default: return Vector3.zero;
			}
		}
	}
}