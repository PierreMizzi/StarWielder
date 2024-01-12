using System;
using DG.Tweening;
using StarWielder.Gameplay.Player;
using TMPro;
using UnityEngine;

namespace StarWielder.UI
{

	public class EnergyUI : MonoBehaviour
	{
		[SerializeField] private PlayerChannel m_playerChannel = null;

		#region Energy

		[Header("Energy")]
		[SerializeField] private TextMeshProUGUI m_starEnergyLabel;

		private void CallbackRefreshStarEnergy(float starEnergy)
		{
			SetStarEnergy(starEnergy);
		}

		private void SetStarEnergy(float starEnergy)
		{
			m_displayedStarEnergy = starEnergy;

			m_starEnergyLabel.text = String.Format("{0:0.0}", m_displayedStarEnergy) + "<size=40%>K</size>";
			m_animator.SetFloat(k_floatStarEnergy, m_displayedStarEnergy);
		}

		#endregion


		#region MonoBehaviour

		private void Awake()
		{
			m_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onStarDocked += CallbackStarDocked;
				m_playerChannel.onStarFree += CallbackStarFree;

				m_playerChannel.onAbsorbEnemyStar += CallbackIncrementEnergy;
				m_playerChannel.onRefreshStarEnergy += CallbackRefreshStarEnergy;
			}
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onStarDocked -= CallbackStarDocked;
				m_playerChannel.onStarFree -= CallbackStarFree;

				m_playerChannel.onAbsorbEnemyStar -= CallbackIncrementEnergy;
				m_playerChannel.onRefreshStarEnergy -= CallbackRefreshStarEnergy;
			}
		}


		#endregion

		#region Increment Tween

		[SerializeField] private float m_incrementDuration = 0.75f;

		private float m_displayedStarEnergy;

		private Tween m_incrementTween;

		private void CallbackIncrementEnergy(float currentStarEnergy)
		{
			m_animator.SetTrigger(k_triggerIncrement);

			float fromStarEnergy = m_displayedStarEnergy;
			m_incrementTween = DOVirtual
			.Float(
				fromStarEnergy,
				currentStarEnergy,
				m_incrementDuration,
				(float starEnergy) =>
				{
					SetStarEnergy(starEnergy);
				}
			)
			.SetEase(Ease.OutCubic);
		}

		#endregion

		#region Animations

		[Header("Animations")]

		private Animator m_animator;

		private const string k_boolIsStarDocked = "IsStarDocked";
		private const string k_triggerIncrement = "Increment";
		private const string k_floatStarEnergy = "StarEnergy";

		private void CallbackStarDocked()
		{
			m_animator.SetBool(k_boolIsStarDocked, true);
		}

		private void CallbackStarFree()
		{
			m_animator.SetBool(k_boolIsStarDocked, false);
		}

		#endregion

	}
}