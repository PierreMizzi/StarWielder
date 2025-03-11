using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Base class of a Module
	/// </summary>
	public class BaseModule : MonoBehaviour
	{
		[SerializeField] protected ModuleChannel m_moduleChannel;
		[SerializeField] protected BaseModuleSettings m_settings;
		[SerializeField] protected ModuleType m_type;
		public ModuleType Type => m_type;
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
	}
}