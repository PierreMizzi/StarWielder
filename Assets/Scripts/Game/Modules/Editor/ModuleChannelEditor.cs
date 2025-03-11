using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace StarWielder.Gameplay.Modules
{

	[CustomEditor(typeof(ModuleChannel))]
	public class ModuleChannelEditor : Editor
	{
		
		private ModuleChannel m_target;

		private void OnEnable()
		{
			m_target = (ModuleChannel)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (m_target == null || m_target.moduleManager == null)
			{
				return;
			}

			GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
			GUILayout.Label("Modules States");
			foreach (KeyValuePair<ModuleType, BaseModule> pair in m_target.moduleManager.Modules)
			{
				if (pair.Value.IsEnabled)
				{
					if (GUILayout.Button($"{pair.Key} : ON"))
					{
						m_target.onDisableModuleType.Invoke(pair.Key);
					}
				}
				else
				{
					if (GUILayout.Button($"{pair.Key} : OFF"))
					{
						m_target.onEnableModuleType.Invoke(pair.Key);
					}
				}
			}
		}

	}
	
}