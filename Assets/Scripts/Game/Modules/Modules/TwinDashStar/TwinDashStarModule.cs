using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public class TwinDashStarModule : BaseModule 
	{
		[SerializeField] private DashStarManager m_dashStarManager;

        public override void Enable()
        {
            base.Enable();
			m_dashStarManager.CreateDashStar();
		}

        public override void Disable()
        {
            base.Disable();
			m_dashStarManager.DeleteDashStar();
        }

    }
}