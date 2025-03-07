using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Enables/Disables Modules accross the game
	/// Stores all available modules and tracks their level
	/// </summary>
	public class GlobalModuleManager : MonoBehaviour
	{

		#region Behaviour

		[Header("Behaviour")]
		[SerializeField] private ModuleChannel m_moduleChannel;

		private void CallbackEnableModule(ModuleType type)
		{
			if (type != ModuleType.None && m_modules.TryGetValue(type, out BaseModule module))
			{
				module.Enable();
			}
		}

		private void CallbackDisableModule(ModuleType type)
		{
			if (type != ModuleType.None && m_modules.TryGetValue(type, out BaseModule module))
			{
				module.Disable();
			}
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_moduleChannel != null)
			{
				m_moduleChannel.onEnableModule += CallbackEnableModule;
				m_moduleChannel.onDisableModule += CallbackDisableModule;
			}

			StoreModules();
		}

        private void OnDestroy()
        {
			if (m_moduleChannel != null)
			{
				m_moduleChannel.onEnableModule -= CallbackEnableModule;
				m_moduleChannel.onDisableModule -= CallbackDisableModule;
			}
		}

        #endregion

        #region Modules

		[Header("Modules")]
		[SerializeField] private Transform m_modulesContainer;

		private Dictionary<ModuleType, BaseModule> m_modules = new Dictionary<ModuleType, BaseModule>();

		private void StoreModules()
		{
			foreach (Transform child in m_modulesContainer)
			{
				if(child.TryGetComponent(out BaseModule module))
				{
					if (m_modules.ContainsKey(module.type) == false)
					{
						m_modules.Add(module.type, module);
					}
				}
			}
		}

		#endregion

	}
}