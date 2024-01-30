using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class CurrencyUI : MonoBehaviour
	{

		[SerializeField] private GameChannel m_gameChannel;

		[SerializeField] private RectTransform m_rectTransform;
		public static Vector3 worldPosition;
		[SerializeField] private Camera m_camera;

		[SerializeField] private TextMeshProUGUI m_currencyLabel;
		private int m_totalCurrency;

		[ContextMenu("Test")]
		public void Test()
		{
			StartCoroutine("Start");
		}

		private IEnumerator Start()
		{
			m_totalCurrency = 0;
			m_currencyLabel.text = m_totalCurrency.ToString();

			yield return new WaitForSeconds(1f);
			worldPosition = m_camera.ScreenToWorldPoint(m_rectTransform.position);
			worldPosition.z = 0;
			yield return null;

			if (m_gameChannel != null)
			{
				m_gameChannel.onCollectCurrency += CallbackCollectCurrency;
			}
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onCollectCurrency -= CallbackCollectCurrency;
			}
		}

		private void CallbackCollectCurrency(int amount)
		{
			m_totalCurrency += amount;
			m_currencyLabel.text = m_totalCurrency.ToString();
		}
	}
}
