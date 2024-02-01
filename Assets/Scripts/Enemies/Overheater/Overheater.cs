using System.Collections.Generic;
using PierreMizzi.Rendering;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	public class Overheater : Enemy, IStateMachine
	{

		#region MonoBehaviour

		private void Update()
		{
			UpdateState();
		}

		protected override void Awake()
		{
			base.Awake();
			energyDrainSpeed = m_maxEnergy / m_energyDrainDuration;
			energyCoolingSpeed = m_maxEnergy / m_energyCoolingDuration;
		}

		protected override void OnDestroy()
		{
			CreateCurrency();
		}

		#endregion

		#region Enemy

		public override void Initialize(EnemyManager manager)
		{
			base.Initialize(manager);

			Awake();

			InitializeStates();
		}

		#endregion

		#region Mine

		#endregion

		#region State Machine

		public AState currentState { get; set; }
		public List<AState> states { get; set; }
		public void InitializeStates()
		{
			states = new List<AState>()
			{
				new OverheaterStateIdle(this),
				new OverheaterStateOverheating(this),
				new OverheaterStateCooling(this),
			};

			ChangeState(OverheaterStateType.Idle);
		}
		public void UpdateState()
		{
			currentState?.Update();
		}

		public void ChangeState(OverheaterStateType nextState, OverheaterStateType previousState = OverheaterStateType.None)
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

		#endregion

		#region Overheating

		[Header("Overheating")]
		[SerializeField] private float m_maxEnergy;
		[SerializeField] private float m_energyDrainDuration;
		[SerializeField] private float m_energyCoolingDuration;
		[SerializeField] private MaterialPropertyBlockModifier m_materialPropertyBlock;

		public float energyDrainSpeed { get; private set; }
		public float energyCoolingSpeed { get; private set; }
		public float maxEnergy => m_maxEnergy;

		public Star star { get; private set; }
		private float m_currentEnergy;

		public float currentEnergy
		{
			get { return m_currentEnergy; }
			set
			{
				m_currentEnergy = Mathf.Clamp(value, 0f, m_maxEnergy);
			}
		}

		public float currentEnergyNormalized
		{
			get { return m_currentEnergy / m_maxEnergy; }
		}

		public void LockStar(Star star)
		{
			this.star = star;
			star.ChangeState(StarStateType.Locked);

			ChangeState(OverheaterStateType.Overheating);
		}

		public void UnlockStar()
		{
			this.star = null;
		}

		#endregion

		#region Currency

		[SerializeField] private Currency m_currencyPrefab;

		private void CreateCurrency()
		{
			Currency currency = Instantiate(m_currencyPrefab, transform.position, Quaternion.identity);
			currency.Collect();
		}

		#endregion

	}
}