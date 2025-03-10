using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	/// <summary>
	/// 
	/// </summary>
	public interface IModuleHandler
	{


		public void InitializeModuleHandler();
		public void CallbackEnableModule(BaseModule module);
		// public void CallbackResetAllModules();

		#region Implementation

		// [SerializeField] private ModuleChannel m_moduleChannel;
			
		#endregion

	}
}