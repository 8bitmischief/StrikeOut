using System.Collections.Generic;
using UnityEngine;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
	public class EntityManager : SharedUnityMischief.Entities.EntityManager
	{
		private Batter _batter = null;
		private Pitcher _pitcher = null;
		private List<Ball> _balls = new List<Ball>();

		public Batter batter { get => _batter; set => _batter = value; }
		public Pitcher pitcher { get => _pitcher; set => _pitcher = value; }
		public List<Ball> balls => _balls;
	}
}