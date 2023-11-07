using System;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float m_damage = 10f;
    public float damage => m_damage;

    [Header("Move")]
    [SerializeField] private float m_speed = 10f;

    [SerializeField] private LayerMask m_destroyLayerMask;

    private bool m_isMoving = true;

    private Animator m_animator = null;

    private const string k_triggerIsWall = "IsWall";
    private const string k_triggerIsShip = "IsShip";

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_isMoving)
            transform.position += transform.up * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (UtilsClass.CheckLayer(m_destroyLayerMask.value, other.gameObject.layer))
            HitWall();
    }

    private void HitWall()
    {
        m_isMoving = false;
        m_animator.SetTrigger(k_triggerIsWall);
    }

    public void HitShip()
    {
        m_isMoving = false;
        m_animator.SetTrigger(k_triggerIsShip);
    }

    public void AnimEventDestroy()
    {
        Destroy(gameObject);
    }

}
