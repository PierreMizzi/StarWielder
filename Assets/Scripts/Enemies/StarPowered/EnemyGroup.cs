using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	/// <summary>
	/// Base class for enemies. EnemyGroup are composed of EnemyStars, EnemyShips & EnemyTurrets
	/// </summary>
	public class EnemyGroup : Enemy
	{

		#region Main

		private int healthPoint;

		public override void Initialize(EnemyManager manager)
		{
			base.Initialize(manager);

			Awake();

			healthPoint = m_enemyStars.Count;

			// TODO : 🟥 Just do it once
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out EnemyTurret turret))
				{
					turret.Initialize(this);
					m_turrets.Add(turret);
				}
			}

			foreach (EnemyStar star in m_enemyStars)
			{
				star.Initialize(this);
			}
		}

		public override void StopBehaviour()
		{
			base.StopBehaviour();
			DeactivateTurrets();
		}



		#endregion

		#region Enemy

		public override void Kill()
		{
			m_poolingChannel.onReleaseToPool(gameObject);

			// Actions on EnemyTurrets
			DeactivateTurrets();

			// Actions on EnemyStar

			// Animations
			// Reset automaticaly

		}

		#endregion

		#region Turrets

		private List<EnemyTurret> m_turrets = new List<EnemyTurret>();

		private void ActivateTurrets()
		{
			foreach (EnemyTurret turret in m_turrets)
				turret.Activate();
		}

		public void DeactivateTurrets()
		{
			foreach (EnemyTurret turret in m_turrets)
				turret.Deactivate();
		}

		#endregion

		#region EnemyStar

		[SerializeField] private List<EnemyStar> m_enemyStars = new List<EnemyStar>();

		public void EnemyStarAppear(int index)
		{
			m_enemyStars[index].Appear();
		}

		public void EnemyStarKilled(EnemyStar enemyStar)
		{
			healthPoint--;
			if (healthPoint == 0)
				Kill();
		}

		#endregion

		#region Animations

		public override void AnimEventAppearCompleted()
		{
			ActivateTurrets();
		}

		#endregion


	}
}