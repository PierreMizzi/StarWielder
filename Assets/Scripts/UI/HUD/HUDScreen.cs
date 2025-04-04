using StarWielder.Gameplay;
using UnityEngine;

namespace StarWielder.UI
{

	public class HUDScreen : MonoBehaviour
	{
		[SerializeField] private GameChannel m_gameChannel = null;

		private Animator _animator = null;

		private const string k_display = "Display";

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			if (m_gameChannel != null)
				m_gameChannel.onFirstDocking += CallbackFirstDocking;
		}

		private void OnDestroy()
		{

			if (m_gameChannel != null)
				m_gameChannel.onFirstDocking -= CallbackFirstDocking;
		}

		private void CallbackFirstDocking()
		{
			_animator.SetTrigger(k_display);
		}
	}
}
