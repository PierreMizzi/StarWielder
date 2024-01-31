using StarWielder.Gameplay.Player;

namespace StarWielder.Gameplay.Enemies
{
	public class Overheater : Enemy
	{

		#region Enemy

		#endregion

		#region Mine

		#endregion

		#region Overheating

		public void LockStar(Star star)
		{
			star.ChangeState(StarStateType.Locked);
		}

		#endregion

	}
}