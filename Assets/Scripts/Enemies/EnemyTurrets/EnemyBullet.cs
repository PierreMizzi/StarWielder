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

    private void Update()
    {
        transform.position += transform.up * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (UtilsClass.CheckLayer(m_destroyLayerMask.value, other.gameObject.layer))
            Destroy(gameObject);
    }

}
