using System;
using UnityEngine;
using SharedUnityMischief;

namespace StrikeOut.BossFight {
	[CreateAssetMenu(menuName = "Strike Out/Data/Pitch Data", fileName = "Pitch Data")]
	public class PitchDataObject : ScriptableObject {
		public GenericDictionary<PitchType, PitchData> pitches = new GenericDictionary<PitchType, PitchData>();

		[Serializable]
		public class PitchData {
			public int earlyHitFrames = 0;
			public int lateHitFrames = 0;
		}
	}
}