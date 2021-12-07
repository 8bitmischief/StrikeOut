using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StrikeOut.BossFight.UI
{
	public class BatterHealth : MonoBehaviour
	{
		[SerializeField] private List<Image> _healthPoints;
		[SerializeField] private Sprite _fullSprite;
		[SerializeField] private Sprite _emptySprite;

		private void Update()
		{
			SetHealth(Scene.I.batter.health);
		}

		public void SetHealth(int health)
		{
			for (int i = 0; i < _healthPoints.Count; i++)
			{
				_healthPoints[i].sprite = i < health ? _fullSprite : _emptySprite;
			}
		}
	}
}