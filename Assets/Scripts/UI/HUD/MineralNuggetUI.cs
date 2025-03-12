using System.Collections;
using StarWielder.Gameplay;
using TMPro;
using UnityEngine;

public class MineralNuggetUI : MonoBehaviour
{
	[SerializeField] private GameChannel m_gameChannel;
	[SerializeField] private TextMeshProUGUI m_mineralNuggetLabel;

	private void Start()
	{
		m_mineralNuggetLabel.text = "0";

		if (m_gameChannel != null)
		{
			m_gameChannel.onIncrementMineralNugget += RefreshText;
			m_gameChannel.onDecrementMineralNugget += RefreshText;
		}
	}

	private void OnDestroy()
	{
		if (m_gameChannel != null)
		{
			m_gameChannel.onIncrementMineralNugget -= RefreshText;
			m_gameChannel.onDecrementMineralNugget -= RefreshText;
		}
	}

	private void RefreshText(int amount)
	{
		m_mineralNuggetLabel.text = m_gameChannel.mineralNuggetCurrentAmount.ToString();
	}
}