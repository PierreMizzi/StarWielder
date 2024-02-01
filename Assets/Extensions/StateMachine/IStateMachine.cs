using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.Useful.StateMachines
{

    /// <summary>
    /// Interface for every entity that behaves like a state machine
    /// </summary>
    public interface IStateMachine
    {
        public GameObject gameObject { get; }
        public AState currentState { get; set; }
        public List<AState> states { get; set; }
        public void InitializeStates();
        // {
        //     currentState = new List<AState>()
        //     {
        //         // MyState
        //     };
        // }

        public void UpdateState();
        // {
        // 	currentState?.Update();
        // }

        public void ChangeState(int previousState, int nextState);
        // {
        //     currentState?.Exit();

        //     currentState = states.Find((AState newState) => newState.type == nextState);
        //     if (currentState != null)
        //         currentState.Enter(previousState);
        //     else
        //     {
        //         Debug.LogError($"Couldn't find a new state of type : {nextState}. Going Inactive");
        //     }
        // }
    }
}
