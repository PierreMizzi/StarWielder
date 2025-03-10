
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Base class of a Module
	/// </summary>
	public class BaseModule : MonoBehaviour
	{
		[SerializeField] private ModuleChannel m_moduleChannel;
		[SerializeField] private ModuleType m_type;
		public ModuleType type => m_type;
		[SerializeField] private bool m_isEnabled;
		public bool isEnabled => m_isEnabled;

		public virtual void Enable()
		{
			m_isEnabled = true;
			m_moduleChannel.onEnableModule?.Invoke(this);
		}

		public virtual void Disable()
		{
			m_isEnabled = false;
		}
	}
}