using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Base class of a Module
	/// </summary>
	public class BaseModule : MonoBehaviour
	{
		[Header("BaseModule")]
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

		public bool IsBuyable
		{
			get
			{
				return !m_isEnabled;
			}
		}

		public virtual void Enable()
		{
			m_isEnabled = true;
		}

		public virtual void Disable()
		{
			m_isEnabled = false;
		}

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
	}
}