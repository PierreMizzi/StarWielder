using System;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{


	[CreateAssetMenu(fileName = "PlayerChannel", menuName = "ScriptableObjects/Channels/PlayerChannel", order = 1)]
	public class PlayerChannel : ScriptableObject
	{
		#region Energy

		public FloatDelegate onAbsorbEnemyStar;
		public FloatDelegate onRefreshStarEnergy;

		#endregion

		#region Dash

		public Action onUseDash;
		public FloatDelegate onRefreshCooldownDash;
		public Action onRechargeDash;

		#endregion

		#region Health

		public FloatDelegate onRefreshHealth;

		#endregion

		#region Combo

		public IntDelegate onRefreshStarCombo;

		#endregion

		public void OnEnable()
		{
			onAbsorbEnemyStar = (float energy) => { };
			onRefreshStarEnergy = (float energy) => { };

			onUseDash = () => { };
			onRefreshCooldownDash = (float value) => { };
			onRechargeDash = () => { };

			onRefreshHealth = (float normalizedHealth) => { };

			onRefreshStarCombo = (int combo) => { };
		}
	}
}