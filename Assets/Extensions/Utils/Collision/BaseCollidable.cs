using UnityEngine;

/*
    Created by Pierre Mizzi
*/

/// <summary>
/// Custom delegate used to dispatch collision events
/// </summary>
/// <param name="source">Object who detected the collision</param>
/// <param name="other">Collided object</param>
public delegate void ColliderDelegate(BaseCollidable source, Collider other);

/// <summary>
/// Basic collision detection script, dispatches events whenever there is a collision
/// </summary>
public class BaseCollidable : MonoBehaviour
{
    [SerializeField]
    private LayerMask _collisionMask;

    public ColliderDelegate onTriggerEnterEvent = null;
    public ColliderDelegate onTriggerStayEvent = null;
    public ColliderDelegate onTriggerExitEvent = null;

    [SerializeField]
    private GameObject _owner = null;
    public GameObject owner
    {
        get { return _owner; }
    }

    private void Awake()
    {
        onTriggerEnterEvent = (BaseCollidable source, Collider other) => { };
        onTriggerStayEvent = (BaseCollidable source, Collider other) => { };
        onTriggerExitEvent = (BaseCollidable source, Collider other) => { };
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckAndInvoke(onTriggerEnterEvent, other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckAndInvoke(onTriggerStayEvent, other);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckAndInvoke(onTriggerStayEvent, other);
    }

    /// <summary>
    /// Checks if the collided object is in the collision layer
    /// and then dispacthes the right event
    /// </summary>
    /// <param name="eventDelegate">event to dispatch</param>
    /// <param name="other">Collided object</param>
    public void CheckAndInvoke(ColliderDelegate eventDelegate, Collider other)
    {
        if (1 << other.gameObject.layer == _collisionMask.value)
        {
            eventDelegate.Invoke(this, other);
        }
    }
}
