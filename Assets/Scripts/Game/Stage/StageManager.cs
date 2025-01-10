using System.Collections.Generic;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using StarWielder.Gameplay;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*

	The game reaches different stages as you play along
	One stage is completed after a certain amount of time
	Before an "enemy" stage, 3 enemies are selected and will spawn during the stage

	- Type of stages
		- Fight
		- Shop
		- Resources
*/


namespace StarWielder.Gameplay
{

	public class StageManager : MonoBehaviour, IStateMachine
	{

		#region Main

		[SerializeField] private GameChannel m_gameChannel;
		public GameChannel gameChannel { get { return m_gameChannel; } }

		private void CallbackStartGame()
		{
			StartStage();
		}

		private void CallbackGameOver(GameOverReason reason)
		{
			((StageState)currentState).CallbackGameOver();
		}

		#endregion

		#region Stage State Manager

		private List<StageStateManager> m_stageStateManagers = new List<StageStateManager>();

		private void InitializeStageStateManagers()
		{
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out StageStateManager stateManager))
				{
					m_stageStateManagers.Add(stateManager);
				}
			}
		}

		public T GetStageManager<T>() where T : StageStateManager
		{
			return m_stageStateManagers.Find((StageStateManager item) => item.GetType() == typeof(T)) as T;
		}

		#endregion

		#region MonoBehaviour

		[ContextMenu("Awake")]
		public void Awake()
		{
			InitializeStageStateManagers();
			InitializeStates();
		}

		private void Start()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onStartGame += CallbackStartGame;
				m_gameChannel.onGameOver += CallbackGameOver;
			}
		}

        private void Update()
		{
			if (Input.GetKeyDown(KeyCode.N))
			{
				NextStage();
			}
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onStartGame -= CallbackStartGame;
				m_gameChannel.onGameOver -= CallbackGameOver;
			}
		}

		#endregion

		#region Stage Succession

		private int m_currentStageIndex = 0;
		[SerializeField] private List<StageSettings> m_stagesOrder = new List<StageSettings>();

		private void StartStage()
		{
			StageSettings currentStageSettings = m_stagesOrder[m_currentStageIndex];
			ChangeState(currentStageSettings);
		}

		public void CallbackStageEnded()
		{
			m_currentStageIndex++;

			if (m_currentStageIndex < m_stagesOrder.Count)
				StartStage();
			else
				Debug.Log("Game finished !");
		}

		public void NextStage()
		{
			((StageState)currentState)?.Clear();
			CallbackStageEnded();
		}


		#endregion

		#region State Machine

		public List<AState> states { get; set; } = new List<AState>();
		public AState currentState { get; set; }

		public void InitializeStates()
		{
			states = new List<AState>()
			{
				new FightStageState(this),
				new ResourcesStageState(this),
				new IdleStageState(this),
				new ShopStageState(this),
			};
		}

		public void UpdateState()
		{
			currentState?.Update();
		}

		public void ChangeState(StageSettings nextStageSettings, StageStateType previousState = StageStateType.None)
		{
			currentState?.Exit();

			currentState = states.Find((AState newState) => newState.type == (int)nextStageSettings.Type);
			if (currentState != null)
			{
				StageState casted = (StageState)currentState;
				casted.Enter(nextStageSettings);
			}
			else
			{
				Debug.LogError($"Couldn't find a new state of type : {nextStageSettings.Type}. Going Inactive");
			}
		}

		public void ChangeState(int previousState, int nextState) { }


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

		#region Stage Building

		// [Header("Stage Building")]

		#endregion

	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(StageManager))]
public class StageManagerEditor : Editor
{
	private StageManager m_target;

	private void OnEnable()
	{
		m_target = (StageManager)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Next Stage"))
		{
			m_target?.NextStage();
		}

	}
}

#endif