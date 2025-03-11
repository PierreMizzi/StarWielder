using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public delegate void ModuleTypeDelegate(ModuleType type);
	public delegate void ModuleDelegate(BaseModule module);

	[CreateAssetMenu(fileName = "ModuleChannel", menuName = "StarWielder/Channels/ModuleChannel", order = 0)]
	public class ModuleChannel : ScriptableObject
	{
		[HideInInspector] public GlobalModuleManager moduleManager;

		public ModuleTypeDelegate onEnableModuleType;
		public ModuleDelegate onEnableModule;
		public ModuleTypeDelegate onDisableModuleType;
		public ModuleDelegate onDisableModule;

		private void OnEnable()
		{
			onEnableModuleType = (ModuleType type) => { };
			onDisableModuleType = (ModuleType type) => { };
		}

		#region Name

		#endregion
	}
}