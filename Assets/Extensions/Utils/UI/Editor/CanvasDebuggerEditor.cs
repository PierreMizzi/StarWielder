using UnityEngine;
using UnityEditor;
using PierreMizzi.Useful;

[CustomEditor(typeof(CanvasDebugger))]
public class CanvasDebuggerEditor : Editor
{
	private CanvasDebugger canvasDebugger;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		canvasDebugger = (CanvasDebugger)target;

		if (GUILayout.Button("Debug Display"))
			canvasDebugger.DebugDisplay();

		if (GUILayout.Button("Debug Hide"))
			canvasDebugger.DebugHide();


		GUILayout.Label("Isolate Child");

		foreach (Transform child in canvasDebugger.transform)
		{
			if (GUILayout.Button(child.name))
				canvasDebugger.IsolateChild(child.GetSiblingIndex());
		}

	}


}