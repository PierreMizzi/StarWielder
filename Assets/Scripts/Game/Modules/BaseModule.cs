
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Base class of a Module
	/// </summary>
	public class BaseModule : MonoBehaviour
	{
		[SerializeField] private ModuleType m_type;
		public ModuleType type => m_type;
		[SerializeField] private bool m_isEnabled;
		public bool isEnabled => m_isEnabled;

		public void Enable()
		{
			m_isEnabled = true;
		}

		public void Disable()
		{
			m_isEnabled = false;
		}
	}
}