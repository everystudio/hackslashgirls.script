using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PanelBattleLog))]
public class PanelBattleLogEditor : Editor
{
	public override void OnInspectorGUI()
	{
		PanelBattleLog panel = target as PanelBattleLog;
		base.OnInspectorGUI();

		if (GUILayout.Button("ArrayTest"))
		{
			panel.AddMessage(panel.TestMessage);

		}

	}
}


