using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
    [RequireComponent(typeof(ShipController))]
    [RequireComponent(typeof(HealthEntity))]
    public class Ship : MonoBehaviour
    {

        #region Base

        private ShipController m_controller;

        [SerializeField]
        private PlayerSettings m_settings;

        [SerializeField] private PlayerChannel m_playerChannel = null;

        [SerializeField] private Star m_star;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            m_controller = GetComponent<ShipController>();
            m_healthEntity = GetComponent<HealthEntity>();
        }

        private void Start()
        {
            // Health
            m_healthEntity.Initialize(m_settings.maxHealth);
            SubscribeHealthEntity();

            // Energy   
            m_currentEnergy = m_settings.baseEnergy;

            if (m_playerChannel != null)
            {
                // m_playerChannel.onStarReleased += CallbackStarReleased;
                // m_playerChannel.onStarDocked += CallbackStarDocked;
            }
        }

        private void Update()
        {
            ManageEnergy();
        }

        private void OnDestroy()
        {
            UnsubscribeHealthEntity();

            if (m_playerChannel != null)
            {
                // m_playerChannel.onStarReleased -= CallbackStarReleased;
                // m_playerChannel.onStarDocked -= CallbackStarDocked;
            }
        }

        #endregion

        #region Star

        [SerializeField]
        private Transform m_starAnchor;
        public Transform starAnchor => m_starAnchor;

        #endregion

        #region Energy

        private float m_currentEnergy = 0;
        public float currentEnergy
        {
            get { return m_currentEnergy; }
            set { m_currentEnergy = Mathf.Clamp(value, 0f, m_settings.baseEnergy); }
        }

        public bool hasEnergy => m_currentEnergy > 0;

        public float missingEnergy => m_settings.baseEnergy - m_currentEnergy;

        private void ManageEnergy()
        {
            if (!m_star.isOnShip && hasEnergy)
            {
                currentEnergy -= m_settings.energyDepleatRate * Time.deltaTime;
                m_playerChannel.onRefreshShipEnergy.Invoke(m_currentEnergy);
            }
            else if (m_star.isOnShip && m_star.hasEnergy)
            {
                m_star.currentEnergy -= m_settings.starEnergyDepleatRate * Time.deltaTime;
                m_playerChannel.onRefreshStarEnergy.Invoke(m_star.currentEnergy);
            }

            m_controller.enabled = hasEnergy || (m_star.isOnShip && m_star.hasEnergy);
        }

        public float GetMaxTransferableEnergy(float starEnergy)
        {
            float transferableEnergy;
            if (currentEnergy + starEnergy > m_settings.baseEnergy)
                transferableEnergy = m_settings.baseEnergy - currentEnergy;
            else
                transferableEnergy = starEnergy;

            return transferableEnergy;
        }

        #endregion

        #region Health

        private HealthEntity m_healthEntity;

        public HealthEntity healthEntity { get { return m_healthEntity; } }

        private void SubscribeHealthEntity()
        {
            m_healthEntity.onLostHealth += CallbackLostHealth;
            m_healthEntity.onHealedHealth += CallbackHealedHealth;
        }

        private void UnsubscribeHealthEntity()
        {
            m_healthEntity.onLostHealth -= CallbackLostHealth;
            m_healthEntity.onHealedHealth -= CallbackHealedHealth;
        }

        private void CallbackHealedHealth()
        {
            // SoundManager.PlaySFX(SoundDataID.PLAYER_HEALED);
        }

        private void CallbackLostHealth()
        {
            // m_levelChannel.onPlayerHit.Invoke();
            // SoundManager.PlayRandomSFX(m_hitSounds);
        }

        #endregion


    }
}
