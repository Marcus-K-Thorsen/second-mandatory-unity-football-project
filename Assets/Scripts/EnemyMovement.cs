using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int _currentWaypointIndex = 0;

    [SerializeField] float speed = 1f;
    // You can adjust the rotation speed here
    [SerializeField] private float rotationSpeed = 5f;
    
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
            Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].transform.position, speed * Time.deltaTime);
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