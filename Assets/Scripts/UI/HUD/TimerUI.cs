using PierreMizzi.Useful;
using StarWielder.Gameplay;
using TMPro;
using UnityEngine;

namespace StarWielder.UI
{
	public class TimerUI : MonoBehaviour
	{

		[SerializeField] private GameChannel m_gameChannel = null;

		[SerializeField] private TextMeshProUGUI m_timeLabel = null;

		private void CallbackRefreshTimer(float value)
		{
			m_timeLabel.text = UtilsClass.SecondsToTextTime(value);
		}

		#region MonoBehaviour

		private void Start()
		{
			if (m_gameChannel != null)
				m_gameChannel.onRefreshTimer += CallbackRefreshTimer;
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
				m_gameChannel.onRefreshTimer -= CallbackRefreshTimer;
		}

		#endregion

	}
}