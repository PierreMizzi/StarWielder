using System.Collections.Generic;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PierreMizzi.Gameplay.Players
{

    public class Star : MonoBehaviour, IStateMachine
    {

        #region Base

        [SerializeField] public PlayerChannel m_playerChannel;
        public PlayerChannel playerChannel => m_playerChannel;

        [SerializeField] private CameraChannel m_cameraChannel = null;
        public CameraChannel cameraChannel => m_cameraChannel;

        [SerializeField] private StarSettings m_settings;
        public StarSettings settings => m_settings;

        public InputActionReference m_mouseClickAction;
        public InputActionReference mouseClickAction => m_mouseClickAction;

        #endregion

        #region StateMachine

        [Header("State Machine")]
        [SerializeField] private StarStateType m_initialState = StarStateType.Docked;
        public List<AState> states { get; set; } = new List<AState>();
        public AState currentState { get; set; }

        public virtual void InitiliazeStates()
        {
            states = new List<AState>()
            {
                new StarStateDocked(this),
                new StarStateFree(this),
                new StarStateReturning(this),
                new StarStateTransfering(this),
            };

            ChangeState(m_initialState);
        }

        public void ChangeState(StarStateType nextState, StarStateType previousState = StarStateType.None)
        {
            ChangeState((int)previousState, (int)nextState);
        }

        public void ChangeState(int previousState, int nextState)
        {
            currentState?.Exit();

            currentState = states.Find((AState newState) => newState.type == nextState);
            if (currentState != null)
                currentState.Enter(previousState);
            else
            {
                Debug.LogError($"Couldn't find a new state of type : {nextState}. Going Inactive");
            }
        }

        public void UpdateState()
        {
            currentState?.Update();
        }

        #endregion

        #region MonoBehaviour

        protected void Awake()
        {
            m_circleCollider = GetComponent<CircleCollider2D>();
            m_currentEnergy = m_settings.baseEnergy;

            InitiliazeStates();
            SetOnShip();
        }

        protected void Update()
        {
            UpdateState();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isOnShip)
                return;

            if (UtilsClass.CheckLayer(m_enemyLayer.value, other.gameObject.layer))
                CollideWithEnemy(other);

            // if (UtilsClass.CheckLayer(m_bounceLayer.value, other.gameObject.layer))
            //     Bounce();
        }


        #endregion

        #region Ship

        [Header("Ship")]
        [SerializeField] private Ship m_ship;
        public Ship ship => m_ship;

        [HideInInspector]
        public bool isOnShip;

        public void SetFree()
        {
            isOnShip = false;
            transform.SetParent(null);
        }

        public void SetOnShip()
        {
            isOnShip = true;
            transform.SetParent(ship.starAnchor);
            transform.localPosition = Vector2.zero;
            transform.localRotation = Quaternion.identity;
        }

        #endregion

        #region Speed

        public float currentSpeed => m_settings.baseSpeed + m_currentEnergy * m_settings.speedFromEnergyRatio;

        private float m_currentSquish;
        private Vector3 m_squishFromSpeed;

        public void ManageSquish()
        {
            m_currentSquish = 1f - ((currentSpeed / m_settings.baseSpeed) * m_settings.squishRatio);
            m_squishFromSpeed.Set(m_currentSquish, 1, 1);
            transform.localScale = m_squishFromSpeed;
        }

        public void ResetSquish()
        {
            transform.localScale = Vector3.one;
        }

        #endregion

        #region Energy

        private float m_currentEnergy;
        public float currentEnergy { get { return m_currentEnergy; } set { m_currentEnergy = Mathf.Max(0f, value); } }
        public bool hasEnergy => m_currentEnergy > 0;

        #endregion

        #region Obstacle

        [Header("Obstacle")]
        [SerializeField] private ContactFilter2D m_obstacleFilter;
        [SerializeField] private CircleCollider2D m_circleCollider;

        public ContactFilter2D obstacleFilter => m_obstacleFilter;
        public CircleCollider2D circleCollider => m_circleCollider;

        #endregion

        #region Enemy

        [Header("Enemy")]
        [SerializeField] private LayerMask m_enemyLayer;

        private int m_currentCombo = 1;
        public int currentCombo { get { return m_currentCombo; } set { m_currentCombo = value; } }

        private void CollideWithEnemy(Collider2D other)
        {
            Enemy enemy;
            if (other.gameObject.TryGetComponent(out enemy))
            {
                m_currentCombo += 1;
                m_playerChannel.onRefreshStarCombo.Invoke(m_currentCombo);

                m_currentEnergy += enemy.energy + ComputeComboBonusEnergy(enemy.energy);
                m_playerChannel.onRefreshStarEnergy.Invoke(m_currentEnergy);

                Destroy(enemy.gameObject);
            }
        }

        private float ComputeComboBonusEnergy(float gainedEnergy)
        {
            if (m_currentCombo == 1)
                return 0;
            else
                return gainedEnergy * m_currentCombo * m_settings.comboBonusEnergyRatio;
        }

        #endregion

    }
}