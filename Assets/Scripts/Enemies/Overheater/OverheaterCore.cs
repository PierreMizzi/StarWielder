using PierreMizzi.Useful;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterCore : MonoBehaviour
	{
		[SerializeField] private Overheater m_overheater;

		[SerializeField] private LayerMask m_starLayerMask;

		#region MonoBehaviour

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (UtilsClass.CheckLayer(m_starLayerMask.value, other.gameObject.layer))
				m_overheater.CallbackTriggerEnterStar(other.GetComponent<Star>());

		}

		#endregion
	}
}