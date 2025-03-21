using System;
using System.Collections.Generic;
using DG.Tweening;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{

	// Overheater
	public class Overheater : Enemy, IStateMachine
	{

		#region Enemy

		public override void Initialize(EnemyManager manager)
		{
			base.Initialize(manager);

			Awake();

			// Energy
			energyDrainSpeed = m_requiredEnergy / m_energyDrainDuration;
			energyCoolingSpeed = m_requiredEnergy / m_energyCoolingDuration;
			m_currentEnergy = 0;
			
			// Spawning
			float rndDelay = UnityEngine.Random.Range(m_minDelayBeforeSpawning, m_maxDelayBeforeSpawning);
			DOVirtual.DelayedCall(rndDelay, StartSpawning);

			// EnemyStars
			StoreFakeSunSockets();
			PoolEnemyStars();

			// SunSockets
			StoreMineSpawners();

			// State Machine
			InitializeStates();
			ChangeState(OverheaterStateType.Active);
		}

		public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			base.SetPositionAndRotation(position, rotation);
			m_originPosition = position;
		}

		public override void Kill()
		{
			FreeEnemyStars();

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

		#region Behaviour
		
		[SerializeField] private SunSocket m_sunSocket;
		public SunSocket SunSocket => m_sunSocket;

		[SerializeField] private float m_rotationSpeed;
		public float rotationSpeed => m_rotationSpeed;

		public Star sun { get; set; }

		public void CallbackTriggerEnterStar(Star star)
		{
			this.sun = star;
			ChangeState(OverheaterStateType.Overheating);
		}

		#endregion

		#region MonoBehaviour
	

		private void Start()
		{
			if (m_mineSpawningTimer != null)
				m_mineSpawningTimer.onCycleCompleted += SpawnMine;
		}

		private void Update()
		{
			UpdateState();
		}

		private void OnDestroy()
		{
			if (m_mineSpawningTimer != null)
				m_mineSpawningTimer.onCycleCompleted -= SpawnMine;
		}

		#endregion

		#region EnemyStars

		[Header("Enemy Stars")]
		[SerializeField] private EnemyStar m_enemyStarPrefab;
		
		[SerializeField] private Transform m_fakeSunSocketsContainer;
		private List<SunSocket> m_fakeSunSockets = new List<SunSocket>();

		private List<EnemyStar> m_enemyStars = new List<EnemyStar>();

		private void StoreFakeSunSockets()
		{
			if (m_fakeSunSocketsContainer == null || m_fakeSunSockets.Count != 0)
			{
				return;
			}

			foreach (Transform child in m_fakeSunSocketsContainer)
			{
				if (child.TryGetComponent(out SunSocket sunSocket))
				{
					m_fakeSunSockets.Add(sunSocket);
				}
			}
		}

		private void PoolEnemyStars()
		{
			foreach (SunSocket fakeSunSocket in m_fakeSunSockets)
			{
				GameObject objectPooled = m_poolingChannel.onGetFromPool.Invoke(m_enemyStarPrefab.gameObject);

				if (objectPooled != null && objectPooled.TryGetComponent(out EnemyStar star))
				{
					star.QuickAppear();
					star.SetUninteractable();
					star.transform.SetParent(fakeSunSocket.transform);
					star.transform.localPosition = Vector3.zero;

					fakeSunSocket.Close();

					m_enemyStars.Add(star);
				}
			}
		}

		private void FreeEnemyStars()
		{
			foreach (EnemyStar star in m_enemyStars)
			{
				star.transform.parent = null;
				star.SetInteractable();
			}
			m_enemyStars.Clear();
		}

		#endregion

		#region Mines Spawners

		[Header("Mine Spawners")]

		[SerializeField] private Transform m_mineSpawnersContainer;
		[SerializeField] private float m_minDelayBeforeSpawning = 1f;
		[SerializeField] private float m_maxDelayBeforeSpawning = 2f;
		[SerializeField] private CyclicTimer m_mineSpawningTimer;
		private List<OverheaterMineSpawner> m_mineSpawners = new List<OverheaterMineSpawner>();

		private void StoreMineSpawners()
		{
			if (m_mineSpawnersContainer == null || m_mineSpawners.Count != 0)
			{
				return;
			}

			m_mineSpawners.Clear();
			foreach (Transform child in m_mineSpawnersContainer)
			{
				if (child.TryGetComponent(out OverheaterMineSpawner spawner))
				{
					m_mineSpawners.Add(spawner);
				}
			}
		}

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
			List<OverheaterMineSpawner> availableSpawner = new List<OverheaterMineSpawner>();

			foreach (OverheaterMineSpawner mineSpawner in m_mineSpawners)
			{
				if (mineSpawner.canSpawn)
				{
					availableSpawner.Add(mineSpawner);
				}
			}
			return availableSpawner.PickRandom();
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
					new OverheaterStateActive(this),
				};
			}
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

		#region Energy Management

		[Header("Energy")]
		[SerializeField] private float m_requiredEnergy = 6f;
		public float RequiredEnergy => m_requiredEnergy;

		private float m_currentEnergy;

		public float currentEnergy
		{
			get { return m_currentEnergy; }
			set
			{
				m_currentEnergy = Mathf.Clamp(value, 0f, m_requiredEnergy);
				m_animator.SetFloat(k_floatEnergyNormalized, currentEnergyNormalized);
			}
		}
		public float currentEnergyNormalized
		{
			get { return m_currentEnergy / m_requiredEnergy; }
		}

		[Header("Energ Draining")]
		[SerializeField] private float m_energyDrainDuration;
		public float energyDrainSpeed { get; private set; }

		[Header("Energy Cooling")]
		[SerializeField] private float m_energyCoolingDuration;
		public float energyCoolingSpeed { get; private set; }

		#endregion

		#region Shaking

		[Header("Shaking")]
		[SerializeField] private AnimationCurve m_vibratoCurve;
		[SerializeField] private AnimationCurve m_shakeCurve;

		private float m_currentVibrato;
		private float m_vibrato;
		private float m_currentShake;

		private float rndAngle;
		private Vector3 rndOffset;

		private Vector3 m_originPosition;

		public void Shake(float progress)
		{
			m_currentVibrato = m_vibratoCurve.Evaluate(progress);
			m_currentShake = m_shakeCurve.Evaluate(progress);

			m_vibrato += Time.deltaTime;

			if (m_vibrato >= m_currentVibrato)
			{
				m_vibrato = 0;
				rndAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
				rndOffset = new Vector3(Mathf.Cos(rndAngle), Mathf.Sin(rndAngle));
				transform.position = m_originPosition + rndOffset * m_currentShake;
			}
		}
		
		#endregion

		#region Animations

		private const string k_floatEnergyNormalized = "EnergyNormalized";

		private const string k_floatHeatProgress = "Progress";

		public void SetHeatProgress(float progress)
		{
			m_animator.SetFloat(k_floatHeatProgress, progress);
		}

		#endregion

		#region Currency

		[SerializeField] private Currency m_currencyPrefab;

        private void CreateCurrency()
		{
			Currency currency = m_poolingChannel.onGetFromPool.Invoke(m_currencyPrefab.gameObject).GetComponent<Currency>();
			currency.transform.position = transform.position;
			currency.Collect();
		}

		#endregion

		#region Debug

		[Header("Debug")]
		[SerializeField, Range(0f, 1f)] private float m_testValue;

		#endregion

	}
}