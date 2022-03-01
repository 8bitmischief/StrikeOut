using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
	public class EntityManager : SharedUnityMischief.Entities.EntityManager
	{
		private Batter _batter = null;
		private Entity _pitcher = null;
		private StrikeZone _strikeZone = null;
		private List<Ball> _balls = new List<Ball>();

		public Batter batter { get => _batter; set => _batter = value; }
		public Entity pitcher { get => _pitcher; set => _pitcher = value; }
		public StrikeZone strikeZone { get => _strikeZone; set => _strikeZone = value; }
		public List<Ball> balls => _balls;
	}
}