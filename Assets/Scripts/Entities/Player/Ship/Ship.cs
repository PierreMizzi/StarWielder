using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
    [RequireComponent(typeof(ShipController))]
    [RequireComponent(typeof(HealthEntity))]
    public class Ship : MonoBehaviour
    {

        #region Fields

        private ShipController m_controller;

        private HealthEntity m_healthEntity;

        public HealthEntity healthEntity { get { return m_healthEntity; } }


        [SerializeField]
        private PlayerSettings m_settings;

        [SerializeField]
        private Transform m_starAnchor;

        #endregion


        #region Methods

        #region MonoBehaviour

        private void Awake()
        {
            m_controller = GetComponent<ShipController>();
            m_healthEntity = GetComponent<HealthEntity>();
        }

        private void Start()
        {
            m_healthEntity.Initialize(m_settings.maxHealth);
            SubscribeHealthEntity();
        }

        private void OnDestroy()
        {
            UnsubscribeHealthEntity();
        }

        #endregion

        #region Health

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

        #endregion

    }
}
