using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	/// <summary>
	/// 
	/// </summary>
	public interface IModuleHandler
	{	

		public ModuleChannel ModuleChannel { get; }
		// public BaseModuleSettings Settings { get; }
		// public bool IsEnabled { get; set; }
		
		public void SubscribeModuleHandler()
		{
			if (ModuleChannel != null)
			{
				ModuleChannel.onEnableModule += CallbackEnableModule;
			}
		}

		public void UnsubscribeModuleHandler()
		{
			if (ModuleChannel != null)
			{
				ModuleChannel.onDisableModule -= CallbackEnableModule;
			}
		}

		public void CallbackEnableModule(BaseModule module);

	}
}