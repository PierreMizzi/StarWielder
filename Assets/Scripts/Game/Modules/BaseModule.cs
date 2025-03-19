using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Base class of a Module
	/// </summary>
	public class BaseModule : MonoBehaviour
	{

		#region Behaviour
		[Header("BaseModule")]
		protected GlobalModuleManager m_moduleManager;
		[SerializeField] protected ModuleChannel m_moduleChannel;
		[SerializeField] protected BaseModuleSettings m_settings;
		public BaseModuleSettings Settings => m_settings;
		public ModuleType Type
		{
			get
			{
				if (m_settings == null)
				{
					return ModuleType.None;
				}
				else
				{
					return Settings.Type;
				}
			}
		}
		protected bool m_isEnabled;
		public bool IsEnabled => m_isEnabled;

		protected bool m_isAvailable = false;

		public bool IsBuyable
		{
			get
			{
				return !m_isEnabled;
			}
		}

		public virtual void Initialize(GlobalModuleManager moduleManager)
		{
			m_moduleManager = moduleManager;
		}

		public virtual void Enable()
		{
			m_isEnabled = true;
			InstantiateModuleUI();
		}

		public virtual void Disable()
		{
			m_isEnabled = false;
			DestroyModuleUI();
		}

		public virtual void SetAvailable()
		{
			m_isAvailable = true;
			m_moduleUI?.SetAvailable();
		}

		public virtual void SetUnavailable()
		{
			m_isAvailable = false;
			m_moduleUI?.SetUnavailable();
		}

		#endregion

		#region MonoBehaviour
		protected virtual void Start()
		{
#if UNITY_EDITOR
			if (m_settings == null)
			{
				return;
			}

			if (m_settings.AutoEnable)
			{
				Enable();
			}
#endif
		}
		
		#endregion

		#region UI

		[Header("UI")]
		[SerializeField] protected BaseModuleUI m_moduleUIPrefab;
		protected BaseModuleUI m_moduleUI;

		protected virtual void InstantiateModuleUI()
		{
			if (m_moduleManager == null || 
				m_moduleManager.ModuleUIContainer == null ||
				m_moduleUIPrefab == null)
			{
				return;
			}

			m_moduleUI = Instantiate(m_moduleUIPrefab, m_moduleManager.ModuleUIContainer);
			m_moduleUI.Initialize(this);
		}

		protected virtual void DestroyModuleUI()
		{
			if (m_moduleUI == null)
			{
				return;
			}

			Destroy(m_moduleUI.gameObject);
			m_moduleUI = null;
		}

		#endregion
	
	}
}