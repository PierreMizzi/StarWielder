using System;
using System.Collections.Generic;
using UnityEngine;


/*
	Which module is happening where ?

	TwinDashStar :
		- DashStarManager.cs, Ship.prefab component
*/

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

		private void CallbackEnableModuleType(ModuleType type)
		{
			if (type != ModuleType.None && m_modules.TryGetValue(type, out BaseModule module))
			{
				module.Enable();
			}
		}

		private void CallbackDisableModuleType(ModuleType type)
		{
			if (type != ModuleType.None && m_modules.TryGetValue(type, out BaseModule module))
			{
				module.Disable();
			}
		}

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
			StoreModules();
			if (m_moduleChannel != null)
			{
				m_moduleChannel.moduleManager = this;
			}
		}

        private void Start()
		{
			if (m_moduleChannel != null)
			{
				m_moduleChannel.onEnableModuleType += CallbackEnableModuleType;
				m_moduleChannel.onDisableModuleType += CallbackDisableModuleType;
			}
		}

        private void OnDestroy()
        {
			if (m_moduleChannel != null)
			{
				m_moduleChannel.onEnableModuleType -= CallbackEnableModuleType;
				m_moduleChannel.onDisableModuleType -= CallbackDisableModuleType;
			}
		}

        #endregion

        #region Modules

		[Header("Modules")]
		[SerializeField] private Transform m_modulesContainer;

		private Dictionary<ModuleType, BaseModule> m_modules = new Dictionary<ModuleType, BaseModule>();

		public Dictionary<ModuleType, BaseModule> Modules => m_modules;

		private void StoreModules()
		{
			foreach (Transform child in m_modulesContainer)
			{
				if(child.TryGetComponent(out BaseModule module))
				{
					if (m_modules.ContainsKey(module.Type) == false)
					{
						module.Initialize(this);
						m_modules.Add(module.Type, module);
					}
				}
			}
		}

		#endregion

		#region Modules UI
			
		[Header("Modules UI")]
		[SerializeField] private RectTransform m_moduleUIContainer;
		public RectTransform ModuleUIContainer => m_moduleUIContainer;

		#endregion

	}
}