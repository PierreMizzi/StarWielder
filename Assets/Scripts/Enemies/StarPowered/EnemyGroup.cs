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

		private List<EnemyStar> m_enemyStars = new List<EnemyStar>();

		public override void Initialize(EnemyManager manager)
		{
			base.Initialize(manager);

			Awake();

			// TODO : 🟥 Set this in Editor (linkID : 10)
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out EnemyStar star))
				{
					star.Initialize(this);
					m_enemyStars.Add(star); // Replaceable
				}
				else if (child.TryGetComponent(out EnemyTurret turret))
				{
					turret.Initialize(this);
					m_turrets.Add(turret);
				}
			}
		}

		public override void StopBehaviour()
		{
			base.StopBehaviour();
			DeactivateTurrets();
		}

		public void EnemyStarDestroyed(EnemyStar enemyStar)
		{
			if (m_enemyStars.Contains(enemyStar))
				m_enemyStars.Remove(enemyStar);

			if (m_enemyStars.Count == 0)
				Destroy(gameObject);
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

		#region Animations

		public override void AnimEventAppearCompleted()
		{
			ActivateTurrets();
		}

		#endregion


	}
}