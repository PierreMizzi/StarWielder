using System;
using DG.Tweening;
using PierreMizzi.Gameplay.Players;
using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
	[SerializeField] private PlayerChannel m_playerChannel = null;

	#region Energy

	[Header("Energy")]
	[SerializeField] private TextMeshProUGUI m_starEnergyLabel;

	private void CallbackRefreshStarEnergy(float starEnergy)
	{
		m_displayedStarEnergy = starEnergy;
		SetStarEnergyLabel();
	}
	private void SetStarEnergyLabel()
	{
		m_starEnergyLabel.text = String.Format("{0:0.0}", m_displayedStarEnergy) + "<size=40%>K</size>";
	}

	#endregion


	#region MonoBehaviour

	private void Start()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onAbsorbEnemyStar += CallbackIncrementEnergy;
			m_playerChannel.onRefreshStarEnergy += CallbackRefreshStarEnergy;
		}
	}

	private void OnDestroy()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onAbsorbEnemyStar -= CallbackIncrementEnergy;
			m_playerChannel.onRefreshStarEnergy -= CallbackRefreshStarEnergy;
		}
	}


	#endregion

	#region Increment Animation

	[SerializeField] private float m_incrementDuration = 0.75f;

	private float m_displayedStarEnergy;

	private Tween m_incrementTween;

	private void CallbackIncrementEnergy(float currentStarEnergy)
	{
		KillIncrementTween();

		float fromStarEnergy = m_displayedStarEnergy;
		m_incrementTween = DOVirtual
		.Float(
			fromStarEnergy,
			currentStarEnergy,
			m_incrementDuration,
			(float value) =>
			{
				m_displayedStarEnergy = value;
				SetStarEnergyLabel();
			}
		)
		.SetEase(Ease.OutCubic);
	}

	private void KillIncrementTween()
	{
		if (m_incrementTween != null && m_incrementTween.IsPlaying())
			m_incrementTween.Kill();
	}

	#endregion

}