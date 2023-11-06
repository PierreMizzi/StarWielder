using UnityEngine;

public class MoveTowardsDestination : MonoBehaviour
{
    public Transform destination; // Référence à l'objet de destination
    public float acceleration = 1.0f; // L'accélération de l'objet
    public float maxSpeed = 5.0f; // La vitesse maximale de l'objet
    public float arrivalDistance = 0.1f; // La distance minimale pour considérer l'arrivée

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 direction = destination.position - transform.position;
        float distance = direction.magnitude;

        if (distance > arrivalDistance)
        {
            // Calcule la vitesse souhaitée en fonction de l'accélération
            float desiredSpeed = Mathf.Sqrt(2 * acceleration * distance);

            // Limite la vitesse à la vitesse maximale
            float finalSpeed = Mathf.Min(desiredSpeed, maxSpeed);

            // Calcule la direction de déplacement
            Vector3 velocity = direction.normalized * finalSpeed;

            // Applique la vélocité à l'objet
            rb.velocity = velocity;
        }
        else
        {
            // L'objet est arrivé à destination
            Debug.Log("Arrivé à destination!");
            rb.velocity = Vector3.zero;
        }
    }
}