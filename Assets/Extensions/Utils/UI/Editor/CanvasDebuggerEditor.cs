using UnityEngine;
using UnityEditor;
using PierreMizzi.Useful;

[CustomEditor(typeof(CanvasDebugger))]
public class CanvasDebuggerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		CanvasDebugger canvasDebugger = (CanvasDebugger)target;

		if (GUILayout.Button("Debug Display"))
			canvasDebugger.DebugDisplay();

		if (GUILayout.Button("Debug Hide"))
			canvasDebugger.DebugHide();

	}
}