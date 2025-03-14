using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	public class HomingLazerModule : BaseModule
	{
		
		#region Behaviour
			
		[SerializeField] private Ship m_ship;
		[SerializeField] private Star m_sun;
		private bool canFire = false;

		#endregion

		#region HomingLazer

		[SerializeField] private HomingLazer m_homingLazerPrefab;

		#endregion

	}
}