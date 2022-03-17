using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 direction;
    public float distanceToTravel;
    [SerializeField] private float currentDistanceTravelled;
    [SerializeField] private bool currentMovementForward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision) {
        collision.gameObject.transform.position += direction * Time.deltaTime * (currentMovementForward ? 1 : -1);
    }
    // Update is called once per frame
    void Update()
    {
        currentDistanceTravelled += Vector3.Distance(direction * Time.deltaTime, Vector3.zero) * (currentMovementForward ? 1 : -1);
        transform.position += direction * Time.deltaTime * (currentMovementForward ? 1 : -1);
        if (Mathf.Abs(currentDistanceTravelled) >= distanceToTravel) {
            currentMovementForward = !currentMovementForward;
            currentDistanceTravelled += Vector3.Distance(direction * Time.deltaTime, Vector3.zero) * (currentMovementForward ? 1 : -1);
            transform.position += direction * Time.deltaTime * (currentMovementForward ? 1 : -1);
        }
    }
}
