using System;
using System.Collections.Generic;
using System.Linq;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Enemies;
using UnityEngine;

/*

	The game reaches different stages as you play along
	One stage is completed after a certain amount of time
	Before an "enemy" stage, 3 enemies are selected and will spawn during the stage

	- Type of stages
		- Fight
		- Shop
		- Resources

*/

// TODO : 🟧 Tweak and try
// TODO : 🟥 Detect Resource stage end ?


namespace StarWielder.Gameplay
{

	public class StageManager : MonoBehaviour, IStateMachine
	{

		#region Main

		[SerializeField] private EnemyManager m_enemyManager;
		[SerializeField] private GameChannel m_gameChannel;
		public GameChannel gameChannel { get { return m_gameChannel; } }

		private void CallbackStartGame()
		{
			StartStage();
		}

		#endregion

		#region Stage State Manager

		[SerializeField] private List<StageStateManager> m_stageStateManagers = new List<StageStateManager>();

		public T GetStageManager<T>() where T : StageStateManager
		{
			return m_stageStateManagers.Find((StageStateManager item) => item.GetType() == typeof(T)) as T;
		}

		#endregion

		#region MonoBehaviour

		[ContextMenu("Awake")]
		public void Awake()
		{
			InitializeStates();
			BuildStageOrder();

			if (m_startingStageType != StageStateType.None)
				DebugSetStartingStage();

			LogStageOrder();
		}

		private void Start()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onStartGame += CallbackStartGame;
				// m_gameChannel.onGameOver += CallbackGameOver;
			}
		}

		#endregion

		#region Stage Building

		[Header("Stage Building")]
		[SerializeField] private int m_stagesAmount = 6;

		private List<StageStateType> m_stages = new List<StageStateType>();

		private void BuildStageOrder()
		{
			m_stages.Clear();

			// Chelou but it works
			List<int> availableStageIndex = new List<int>();

			for (int i = 0; i < m_stagesAmount; i++)
			{
				m_stages.Add(StageStateType.Fight);

				if (i != 0 && i != m_stagesAmount - 1)
					availableStageIndex.Add(i);
			}

			// Add Resource stage
			InsertStage(StageStateType.Resources, availableStageIndex);
			InsertStage(StageStateType.Resources, availableStageIndex);


		}

		private void InsertStage(StageStateType stageType, List<int> availableStageIndex)
		{
			int rndResourceStageIndex = UnityEngine.Random.Range(0, availableStageIndex.Count);
			rndResourceStageIndex = availableStageIndex[rndResourceStageIndex];
			availableStageIndex.Remove(rndResourceStageIndex);
			m_stages[rndResourceStageIndex] = stageType;
		}

		#endregion

		#region Stage Succession

		private int m_currentStageIndex = 0;
		[SerializeField] private FightStageSettings m_fightStageSettings;
		public FightStageSettings fightStageSettings { get { return m_fightStageSettings; } }

		private void StartStage()
		{
			StageStateType currentStageType = m_stages[m_currentStageIndex];
			ChangeState(currentStageType);
		}

		public void CallbackStageEnded()
		{
			m_currentStageIndex++;

			if (m_currentStageIndex < m_stages.Count - 1)
				StartStage();
			else
				Debug.Log("Game finished !");
		}


		#endregion

		#region State Machine

		public List<AState> states { get; set; } = new List<AState>();
		public Dictionary<int, AState> newStates { get; set; } = new Dictionary<int, AState>();
		public AState currentState { get; set; }

		public void InitializeStates()
		{
			states = new List<AState>()
			{
				new FightStageState(this),
				new ResourcesStageState(this),
			};
		}

		public void UpdateState()
		{
			currentState?.Update();
		}

		public void ChangeState(StageStateType nextState, StageStateType previousState = StageStateType.None)
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

		public StageState StageStateFromType(StageStateType type)
		{
			return (StageState)states.Find((AState state) => state.type == (int)type);
		}

		public T GetState<T>() where T : StageState
		{
			foreach (AState state in states)
			{
				if (state.GetType() == typeof(T))
					return (T)state;
			}

			return null;
		}

		#endregion

		#region Debug

		[Header("Debug")]
		[SerializeField] private StageStateType m_startingStageType = StageStateType.None;

		private void DebugSetStartingStage()
		{
			Debug.Log($"Added starting stage : {m_startingStageType}");
			m_stages.Insert(0, m_startingStageType);
		}

		private void LogStageOrder()
		{
			string stagesLog = "";
			foreach (StageStateType stage in m_stages)
			{
				stagesLog += stage.ToString() + " -> ";
			}

			stagesLog = stagesLog.Remove(stagesLog.Length - 4, 4);
			Debug.Log(stagesLog);
		}

		#endregion

	}
}