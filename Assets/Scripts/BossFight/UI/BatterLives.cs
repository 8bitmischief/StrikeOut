using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StrikeOut.BossFight.UI
{
	public class BatterLives : MonoBehaviour
	{
		[SerializeField] private List<Image> _lifePoints;
		[SerializeField] private Sprite _fullSprite;
		[SerializeField] private Sprite _emptySprite;

		private void Update()
		{
			SetLives(Scene.I.entityManager.batter.lives);
		}

		public void SetLives(int lives)
		{
			for (int i = 0; i < _lifePoints.Count; i++)
			{
				_lifePoints[i].sprite = i < lives ? _fullSprite : _emptySprite;
			}
		}
	}
}