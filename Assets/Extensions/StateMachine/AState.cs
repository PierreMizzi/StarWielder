using System;
using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.Useful.StateMachines
{

    /// <summary>
    /// State Machine base state's behaviour
    /// </summary>
    public abstract class AState
    {
        /// <summary>
        /// Entity using this state 
        /// </summary>
        protected IStateMachine m_stateMachine = null;

        public int type { get; protected set; }

        public void Initialize(IStateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
        }

        /// <summary>
        /// Dictionnary to store specific "EnterState" mode when transitioning from one state to another
        /// </summary>
        protected Dictionary<int, Action> m_stateTransitions = new Dictionary<int, Action>();

        public AState(IStateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
        }

        public void ChangeState(int nextState)
        {
            m_stateMachine.ChangeState(type, nextState);
        }

        /// <summary>
        /// Default method called when entering this behaviour
        /// </summary>
        protected virtual void DefaultEnter()
        {
            Debug.Log($"ENTER : {type}");
        }

        /// <summary>
        /// Seach for an "Enter" method matching previous state, if none, DefaultEnter is called
        /// </summary>
        public virtual void Enter(int previousState)
        {
            if (m_stateTransitions.ContainsKey(previousState))
                m_stateTransitions[previousState]();
            else
                DefaultEnter();
        }

        /// <summary>
        /// State's behaviour called every frame
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called when leaving this behaviour
        /// </summary>
        public virtual void Exit() { }

    }
}
