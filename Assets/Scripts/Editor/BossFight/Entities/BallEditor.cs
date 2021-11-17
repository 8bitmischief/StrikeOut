using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight {
	[CustomEditor(typeof(Ball), true)]
	public class BallEditor : BaseEditor {
		public override bool RequiresConstantRepaint () => Application.isPlaying;

		protected override void DrawState () {
			Ball ball = (Ball) target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Ball State", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Hittable", ball.isHittable);
			EditorGUILayout.Toggle("Will Be Hittable", ball.willBeHittable);
			EditorGUILayout.IntField("Frames Until Hittable", ball.framesUntilHittable);
			EditorGUILayout.IntField("Frames Until Unhittable", ball.framesUntilUnhittable);
			EditorGUILayout.Toggle("Has Passed Batting Line", ball.hasPassedBattingLine);
			EditorGUILayout.Toggle("Will Pass Batting Line", ball.willPassBattingLine);
			EditorGUILayout.IntField("Frames Until Pass Batting Line", ball.framesUntilPassBattingLine);
			EditorGUILayout.IntField("Frames Since Passed Batting Line", ball.framesSincePassedBattingLine);
			base.DrawState();
		}
	}
}