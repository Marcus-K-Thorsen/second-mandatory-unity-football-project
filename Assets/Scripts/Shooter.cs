using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("References")] 
    public Transform spawnPoint;
    public GameObject projectilePrefab;
    public GameObject target;
    
    [Header("Stats")]
    [Tooltip("Time in seconds, between firing two projectiles")]
    public float fireRate = 3f;
    [Tooltip("The speed the Enemy will be rotating at")]
    [SerializeField] private float rotationSpeed = 5f;
    [Tooltip("The sound that plays when the shooter shoots")]
    [SerializeField] private AudioSource shootingSound;
    private float _lastFireTime = 0;
    
    
    void Update()
    {
        if (target is not null)
        {
            RotateTowardsWaypoint();

            if (Time.time >= _lastFireTime + fireRate)
            {
                _lastFireTime = Time.time;
                // Instantiate a prefab, use position and rotation from spawnpoint
                shootingSound.Play();
                Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
    
    
    void RotateTowardsWaypoint()
    {
        // Calculate the rotation needed to look at the waypoint
        Vector3 direction = target.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            
        // Smoothly rotate towards the waypoint
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime); 
        
        
    }
}
