using System;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{


	[CreateAssetMenu(fileName = "PlayerChannel", menuName = "ScriptableObjects/Channels/PlayerChannel", order = 1)]
	public class PlayerChannel : ScriptableObject
	{
		#region Energy

		public FloatDelegate onRefreshStarEnergy;
		public IntDelegate onRefreshStarCombo;

		#endregion

		#region Dash

		public Action onUseDash;
		public FloatDelegate onRefreshCooldownDash;
		public Action onRechargeDash;

		#endregion

		#region Health

		public FloatDelegate onRefreshHealth;

		#endregion

		public void OnEnable()
		{
			onRefreshStarEnergy = (float energy) => { };
			onRefreshStarCombo = (int combo) => { };

			onUseDash = () => { };
			onRefreshCooldownDash = (float value) => { };
			onRechargeDash = () => { };

			onRefreshHealth = (float normalizedHealth) => { };
		}
	}
}