using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(HitDetectionBox), true)]
	public class HitDetectionBoxEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			HitDetectionBox box = (HitDetectionBox) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("State", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Active", box.isActive);
			EditorGUILayout.IntField("Frames Until Active", box.framesUntilActive);
			EditorGUILayout.IntField("Frames Until Inactive", box.framesUntilInactive);
			base.DrawState();
		}
	}
}