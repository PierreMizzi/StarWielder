using StarWielder.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;


namespace StarWielder.UI
{
	// TODO : Vignette effect when emergency power is out
	public class EmergencyEnergyUI : MonoBehaviour
	{

		[Header("Main")]
		[SerializeField] private PlayerChannel m_playerChannel = null;

		[Header("Sliders")]
		[SerializeField] private Slider m_leftForegroundSlider;
		[SerializeField] private Slider m_rightForegroundSlider;

		private void CallbackRefreshEmergencyEnergy(float normalizedEnergy)
		{
			m_leftForegroundSlider.value = normalizedEnergy;
			m_rightForegroundSlider.value = normalizedEnergy;
		}

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshEmergencyEnergy += CallbackRefreshEmergencyEnergy;
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshEmergencyEnergy -= CallbackRefreshEmergencyEnergy;
		}

		#endregion

	}
}