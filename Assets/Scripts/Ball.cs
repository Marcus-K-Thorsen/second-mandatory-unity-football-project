using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ball : MonoBehaviour
{
    
    public Rigidbody body;
    [FormerlySerializedAs("Football")] [SerializeField] GameObject football;

    // ReSharper disable Unity.PerformanceAnalysis
    private Vector3 GetFootballLocalPosition() => football.transform.localPosition;

    // ReSharper disable Unity.PerformanceAnalysis
    private void SetFootballLocalPosition(Vector3 value)
    {
        value.y += 0.5f;
        football.transform.localPosition = value;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var localPosition = GetFootballLocalPosition();
        if (localPosition.y <= 0.3f || localPosition.y >= 1f)
        {
            SetFootballLocalPosition(Vector3.zero);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            WallBouncer wall = other.gameObject.GetComponent<WallBouncer>();
            if (wall is not null)
            {
                // By subtracting the two vectors, we get a delta that
                // 'points' from player towards this object, basically
                // The direction we need to move forwards
                var deltaPosition = body.transform.position - wall.Position;
                // Make sure y is flattened since we do not
                // move up or down
                deltaPosition.y = 0;
                // Make it a unit direction (magnitude of 1)
                var forward = deltaPosition.normalized;
                // Add an impulse forward, using the speed of the player
                body.AddForce(forward * wall.Speed, ForceMode.Impulse);
            }
        }
        else if (other.gameObject.layer == 8)
        {
            EnemyMovement enemyPlayer = other.gameObject.GetComponent<EnemyMovement>(); 
            if (enemyPlayer is not null) 
            { 
                // By subtracting the two vectors, we get a delta that
                // // 'points' from player towards this object, basically
                // The direction we need to move forwards
                var deltaPosition = body.transform.position - enemyPlayer.Position;
                // Make sure y is flattened since we do not
                // move up or down
                deltaPosition.y = 0;
                // Make it a unit direction (magnitude of 1)
                var forward = deltaPosition.normalized;
                // Add an impulse forward, using the speed of the player
                body.AddForce(forward * (enemyPlayer.Speed * 0.5f), ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.layer == 7) 
         {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player is not null)
            {
                // By subtracting the two vectors, we get a delta that
                // 'points' from player towards this object, basically
                // The direction we need to move forwards
                var deltaPosition = body.transform.position - player.Position;
                // Make sure y is flattened since we do not
                // move up or down
                deltaPosition.y = 0;
                // Make it a unit direction (magnitude of 1)
                var forward = deltaPosition.normalized;
                // Add an impulse forward, using the speed of the player
                body.AddForce(forward * player.Speed, ForceMode.Impulse);
            }
         } 
         
    }
}
