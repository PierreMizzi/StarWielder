using PierreMizzi.Gameplay.Players;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{

	[Header("Main")]
	[SerializeField] private PlayerChannel m_playerChannel = null;

	#region Dash

	[Header("Dash")]
	[SerializeField] private Image m_dashImage;
	[SerializeField] private Color m_dashAvailableColor = Color.white;
	[SerializeField] private Color m_dashUnavailableColor = Color.white;

	private void CallbackUseDash()
	{
		m_dashImage.color = m_dashUnavailableColor;
	}

	private void CallbackRefreshCooldownDash(float value)
	{
		m_dashImage.fillAmount = value;
	}

	private void CallbackRechargeDash()
	{
		m_dashImage.color = m_dashAvailableColor;
	}

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onUseDash += CallbackUseDash;
			m_playerChannel.onRefreshCooldownDash += CallbackRefreshCooldownDash;
			m_playerChannel.onRechargeDash += CallbackRechargeDash;
		}
	}

	private void OnDestroy()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onUseDash -= CallbackUseDash;
			m_playerChannel.onRefreshCooldownDash -= CallbackRefreshCooldownDash;
			m_playerChannel.onRechargeDash -= CallbackRechargeDash;
		}
	}


	#endregion

}