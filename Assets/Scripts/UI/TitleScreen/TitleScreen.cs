using QGamesTest.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QGamesTest.UI
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private GameChannel m_gameChannel = null;
        [SerializeField] private InputActionReference m_mouseClickInputAction;

        private Animator _animator = null;
        private const string k_triggerHide = "Hide";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_mouseClickInputAction.action.performed += CallbackStartGame;
        }

        private void OnDestroy()
        {
            m_mouseClickInputAction.action.performed -= CallbackStartGame;
        }

        private void CallbackStartGame(InputAction.CallbackContext context)
        {
            m_mouseClickInputAction.action.performed -= CallbackStartGame;
            _animator.SetTrigger(k_triggerHide);
        }
    }
}
