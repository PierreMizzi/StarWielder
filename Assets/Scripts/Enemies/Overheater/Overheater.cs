using System.Collections.Generic;
using DG.Tweening;
using PierreMizzi.Rendering;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	public class Overheater : Enemy, IStateMachine
	{

		#region MonoBehaviour

		[SerializeField] private float m_rotationSpeed = 10f;

		private void Start()
		{
			if (m_mineSpawningTimer != null)
				m_mineSpawningTimer.onCycleCompleted += SpawnMine;
		}

		private void Update()
		{
			// transform.rotation *= Quaternion.Euler(Vector3.forward * m_rotationSpeed * Time.deltaTime);
			UpdateState();
		}

		private void OnDestroy()
		{
			if (m_mineSpawningTimer != null)
				m_mineSpawningTimer.onCycleCompleted -= SpawnMine;
		}

		#endregion

		#region Enemy

		public override void Initialize(EnemyManager manager)
		{
			base.Initialize(manager);

			Awake();

			energyDrainSpeed = m_maxEnergy / m_energyDrainDuration;
			energyCoolingSpeed = m_maxEnergy / m_energyCoolingDuration;

			float rndDelay = Random.Range(m_minDelayBeforeSpawning, m_maxDelayBeforeSpawning);
			DOVirtual.DelayedCall(rndDelay, StartSpawning);

			InitializeStates();
		}

		public override void Kill()
		{
			base.Kill();

			StopBehaviour();

			CreateCurrency();
		}

		public override void StopBehaviour()
		{
			StopSpawning();
			ChangeState(OverheaterStateType.Idle);
		}


		#endregion

		#region Mine

		[Header("Mine")]

		// TODO  : Pool Manager for EnemyBullet & Mine
		[SerializeField] private float m_minDelayBeforeSpawning = 1f;
		[SerializeField] private float m_maxDelayBeforeSpawning = 2f;
		[SerializeField] private List<OverheaterMineSpawner> m_mineSpawners = new List<OverheaterMineSpawner>();
		[SerializeField] private CyclicTimer m_mineSpawningTimer;
		private List<OverheaterMineSpawner> m_availableMineSpawners = new List<OverheaterMineSpawner>();

		public void StartSpawning()
		{
			foreach (OverheaterMineSpawner spawner in m_mineSpawners)
				spawner.ComputeRangePositions();

			m_mineSpawningTimer.StartBehaviour();
		}

		public void StopSpawning()
		{
			m_mineSpawningTimer.StopBehaviour();
		}

		private void SpawnMine()
		{
			OverheaterMineSpawner spawner = GetAvailableMineSpawner();

			if (spawner != null)
				spawner.SpawnMine();
		}

		private OverheaterMineSpawner GetAvailableMineSpawner()
		{
			m_availableMineSpawners.Clear();

			foreach (OverheaterMineSpawner mineSpawner in m_mineSpawners)
			{
				if (mineSpawner.canSpawn)
					m_availableMineSpawners.Add(mineSpawner);
			}
			return m_availableMineSpawners.PickRandom();
		}

		#endregion

		#region State Machine

		public AState currentState { get; set; }
		public List<AState> states { get; set; }
		public void InitializeStates()
		{
			if (states == null)
			{
				states = new List<AState>()
				{
					new OverheaterStateIdle(this),
					new OverheaterStateOverheating(this),
					new OverheaterStateCooling(this),
				};
			}

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

		public Star star { get; set; }
		private float m_currentEnergy;

		public float currentEnergy
		{
			get { return m_currentEnergy; }
			set
			{
				m_currentEnergy = Mathf.Clamp(value, 0f, m_maxEnergy);
				m_animator.SetFloat(k_floatEnergyNormalized, currentEnergyNormalized);
			}
		}

		public float currentEnergyNormalized
		{
			get { return m_currentEnergy / m_maxEnergy; }
		}

		public void CallbackTriggerEnterStar(Star star)
		{
			this.star = star;
			star.ChangeState(StarStateType.Locked);

			ChangeState(OverheaterStateType.Overheating);
		}

		#endregion

		#region Animations

		private const string k_floatEnergyNormalized = "EnergyNormalized";
		private const string k_boolHasStar = "HasStar";

		public void SetHasStar(bool hasStar)
		{
			m_animator.SetBool(k_boolHasStar, hasStar);
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