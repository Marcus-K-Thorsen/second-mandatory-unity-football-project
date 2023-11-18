using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    public Rigidbody body;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("Triggered layer 10");
            WallBouncer wall = other.gameObject.GetComponent<WallBouncer>();
            Debug.Log(wall);
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
        } else if (other.gameObject.layer == 7)
        {
            Debug.Log("Triggered layer 7");
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            Debug.Log(player);
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
        else if (other.gameObject.layer == 8)
        {
            Debug.Log("Triggered layer 8"); 
            EnemyMovement enemyPlayer = other.gameObject.GetComponent<EnemyMovement>(); 
            Debug.Log(enemyPlayer); 
            if (enemyPlayer is not null) 
            { 
                Debug.Log($"{enemyPlayer.name} kicked ball"); 
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
                body.AddForce(forward * enemyPlayer.Speed, ForceMode.Impulse);
            }
        }
    }
}
