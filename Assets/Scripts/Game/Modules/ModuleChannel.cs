using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public delegate void ModuleDelegate(ModuleType type);

	[CreateAssetMenu(fileName = "ModuleChannel", menuName = "StarWielder/ModuleChannel", order = 0)]
	public class ModuleChannel : ScriptableObject
	{
		public ModuleDelegate onEnableModule;
		public ModuleDelegate onDisableModule;

		private void OnEnable()
		{
			onEnableModule = (ModuleType type) => {};
			onDisableModule = (ModuleType type) => { };
		}

		#region Debug

		[SerializeField] private ModuleType m_type;

		[ContextMenu("Call EnableModule")]
		public void EnableModule()
		{
			onEnableModule?.Invoke(m_type);
		}

		[ContextMenu("Call DisableModule")]
		public void DisableModule()
		{
			onDisableModule?.Invoke(m_type);
		}

		#endregion
	}
}