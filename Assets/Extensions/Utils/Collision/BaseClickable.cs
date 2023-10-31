using System;
using UnityEngine;

/// <summary>
/// Basic collision detection script, dispatches events whenever there is a collision
/// </summary>
public class BaseClickable : MonoBehaviour
{
    public Action onClick = null;

    [SerializeField]
    private GameObject _owner = null;
    public GameObject owner
    {
        get { return _owner; }
    }

    private void Awake()
    {
        onClick = () => { };
    }

    public void OnClick()
    {
        onClick.Invoke();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
