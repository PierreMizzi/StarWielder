using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            m_gameChannel.onStartGame += CallbackStartGame;
    }

    private void OnDestroy()
    {

        if (m_gameChannel != null)
            m_gameChannel.onStartGame -= CallbackStartGame;
    }

    private void CallbackStartGame()
    {
        _animator.SetTrigger(k_display);
    }
}
