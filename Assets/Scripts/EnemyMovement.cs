using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [Tooltip("List of Waypoints (Empty Game objects) that the Enemy will move towards")]
    [SerializeField] private GameObject[] waypoints;
    private int _currentWaypointIndex = 0;

    [FormerlySerializedAs("speed")]
    [Header("Stats")]
    [Tooltip("The speed the Enemy will be moving at")]
    public float Speed = 5f;
    // You can adjust the rotation speed here
    [Tooltip("The speed the Enemy will be rotating at")]
    [SerializeField] private float rotationSpeed = 5f;
    
    
    public Vector3 Position => transform.position;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].transform.position) < 0.1f)
        { 
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }
        }
       
        transform.position =
            Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].transform.position, Speed * Time.deltaTime);
        // Rotate towards the waypoint
        RotateTowardsWaypoint();
    }

    void RotateTowardsWaypoint()
    {
        // Calculate the rotation needed to look at the waypoint
        Vector3 direction = waypoints[_currentWaypointIndex].transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Smoothly rotate towards the waypoint
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime); 
    }
}