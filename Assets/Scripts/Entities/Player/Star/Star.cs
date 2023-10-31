using System.Collections;
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

        [SerializeField] private Ship m_ship;
        public Ship ship => m_ship;

        [SerializeField] public PlayerChannel m_playerChannel;
        public PlayerChannel playerChannel => m_playerChannel;

        [SerializeField] private StarSettings m_settings;
        public StarSettings settings => m_settings;

        public InputActionReference m_mouseClickAction;
        public InputActionReference mouseClickAction => m_mouseClickAction;

        [HideInInspector]
        public bool isDocked;

        #endregion



        #region StateMachine

        [Header("State Machine")]
        [SerializeField] private StarStateType m_baseState = StarStateType.Docked;
        public List<AState> states { get; set; } = new List<AState>();
        public AState currentState { get; set; }

        public virtual void InitiliazeStates()
        {
            states = new List<AState>()
            {
                new StarStateDocked(this),
                new StarStateFree(this),
                new StarStateReturning(this),
            };

            ChangeState(m_baseState);
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
            m_currentSpeed = m_settings.baseSpeed;

            InitiliazeStates();
        }

        protected void Update()
        {
            UpdateState();
        }

        #endregion

        #region Speed

        private float m_currentSpeed = 0f;
        public float currentSpeed => m_currentSpeed;

        public float m_currentSquish;
        public Vector3 m_squishFromSpeed;

        public void ManageSquish()
        {
            m_currentSquish = 1f - ((m_currentSpeed / m_settings.baseSpeed) * m_settings.squishRatio);
            m_squishFromSpeed.Set(m_currentSquish, 1, 1);
            transform.localScale = m_squishFromSpeed;
        }

        public void ResetSquish()
        {
            transform.localScale = Vector3.one;
        }

        #endregion

        #region Bounce

        [Header("Bounce")]
        [SerializeField] private LayerMask m_bounceLayer;

        [SerializeField] private ContactFilter2D m_bounceFilter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDocked)
                return;

            if (UtilsClass.CheckLayer(m_bounceLayer.value, other.gameObject.layer))
                Bounce();
        }

        private void Bounce()
        {
            Debug.DrawRay(transform.position, transform.up, Color.blue, 100);
            // Debug.Break();
            List<RaycastHit2D> hits = new List<RaycastHit2D>();

            if (Physics2D.Raycast(transform.position, transform.up, m_bounceFilter, hits, 100f) > 0)
            {
                RaycastHit2D hit = hits[0];
                if (hit.transform.name == "Star")
                    Debug.LogError("Star got reaycasted");

                transform.up = Vector2.Reflect(transform.up, hit.normal);
            }
        }

        #endregion


    }
}