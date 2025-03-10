using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public delegate void ModuleTypeDelegate(ModuleType type);
	public delegate void ModuleDelegate(BaseModule module);

	[CreateAssetMenu(fileName = "ModuleChannel", menuName = "StarWielder/ModuleChannel", order = 0)]
	public class ModuleChannel : ScriptableObject
	{
		public ModuleTypeDelegate onEnableModuleType;
		public ModuleDelegate onEnableModule;
		public ModuleTypeDelegate onDisableModuleType;

		private void OnEnable()
		{
			onEnableModuleType = (ModuleType type) => {};
			onEnableModule = (BaseModule module)=> {};
			onDisableModuleType = (ModuleType type) => { };
		}

		#region Debug

		[SerializeField] private ModuleType m_type;

		[ContextMenu("Call EnableModule")]
		public void EnableModule()
		{
			onEnableModuleType?.Invoke(m_type);
		}

		[ContextMenu("Call DisableModule")]
		public void DisableModule()
		{
			onDisableModuleType?.Invoke(m_type);
		}

		#endregion
	
		#region Name
			
		#endregion
	}
}