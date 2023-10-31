using System.Collections;
using System.Collections.Generic;
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
            InitiliazeStates();
        }

        protected void Update()
        {
            UpdateState();
        }

        #endregion



    }
}