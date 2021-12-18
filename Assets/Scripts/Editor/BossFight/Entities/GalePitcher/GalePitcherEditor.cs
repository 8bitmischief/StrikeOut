using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(GalePitcher), true)]
	public class GalePitcherEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			GalePitcher pitcher = (GalePitcher) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Pitcher State", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Idle", pitcher.isIdle);
			EditorGUILayout.FloatField("Idle Time", pitcher.idleTime);
			base.DrawState();
		}
	}
}