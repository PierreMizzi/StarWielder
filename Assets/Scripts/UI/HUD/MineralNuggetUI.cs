using System.Collections;
using StarWielder.Gameplay;
using TMPro;
using UnityEngine;

public class MineralNuggetUI : MonoBehaviour
{
	[SerializeField] private GameChannel m_gameChannel;
	[SerializeField] private TextMeshProUGUI m_mineralNuggetLabel;

	private int m_totalMineralNuggets;

	private void Start()
	{
		m_totalMineralNuggets = 0;
		m_mineralNuggetLabel.text = m_totalMineralNuggets.ToString();

		if (m_gameChannel != null)
		{
			m_gameChannel.onIncrementMineralNugget += CallbackCollectMineralNugget;
		}
	}

	private void OnDestroy()
	{
		if (m_gameChannel != null)
		{
			m_gameChannel.onIncrementMineralNugget -= CallbackCollectMineralNugget;
		}
	}

	private void CallbackCollectMineralNugget(int amount)
	{
		m_totalMineralNuggets += amount;
		m_mineralNuggetLabel.text = m_totalMineralNuggets.ToString();
	}
}