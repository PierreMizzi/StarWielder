using PierreMizzi.Useful;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
    [RequireComponent(typeof(ShipHealthModifier))]
    public class EnemyBullet : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private LayerMask m_destroyLayerMask;
        [SerializeField] private float m_speed = 10f;
        private bool m_isMoving = true;
        private ShipHealthModifier m_healthModifier;

        private void HitWall()
        {
            m_isMoving = false;
            m_animator.SetTrigger(k_triggerIsWall);
        }

        public void HitShip()
        {
            m_isMoving = false;
            m_animator.SetTrigger(k_triggerIsShip);
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_healthModifier = GetComponent<ShipHealthModifier>();
        }

        private void Start()
        {
            if (m_healthModifier != null)
                m_healthModifier.onModify += HitShip;
        }

        private void OnDestroy()
        {
            if (m_healthModifier != null)
                m_healthModifier.onModify -= HitShip;
        }

        private void Update()
        {
            if (m_isMoving)
                transform.position += transform.up * m_speed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (UtilsClass.CheckLayer(m_destroyLayerMask.value, other.gameObject.layer))
                HitWall();
        }

        #endregion

        #region Animation

        private Animator m_animator = null;

        private const string k_triggerIsWall = "IsWall";
        private const string k_triggerIsShip = "IsShip";

        public void AnimEventDestroy()
        {
            Destroy(gameObject);
        }

        #endregion

    }
}
