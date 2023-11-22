using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Mortality : MonoBehaviour
{
    [SerializeField] public float respawnWaitTime = 3.0f;

    private Vector3 _spawnPoint;

    private Quaternion _spawnRotation;
    private bool _isAlive = true;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject character;

    private bool IsPlayer()
    {
        return player is not null;
    }
    
    
    
    
    public AudioSource deathSound;
    
    // When colliding with something
    private void OnTriggerEnter(Collider other)
    {
        // This check is mostly sanity, only the player should be in layer 7
        if (other.gameObject.layer == 13 && _isAlive)
        { 
            Die();
            Destroy(other.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = IsPlayer() ? player.movingPoint.transform.position : character.transform.position;
        _spawnRotation = IsPlayer() ? player.rotationPoint.rotation : character.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Die()
    {
        _isAlive = false;
        deathSound.Play();
        if (IsPlayer())
        {
            // Stop our movement
            player.movement = Vector3.zero;
            // Stop calling Update
            player.enabled = false;
            // Stop registering collisions
            player.movingPoint.enabled = false;
            // Stop drawing the model
            player.rotationPoint.gameObject.SetActive(false);
                
        }
        else
        {
            // Stop registering collisions
            // Stop drawing the model
            character.SetActive(false);
        }
        // Prepare to wake us up again
        Invoke(nameof(Respawn), respawnWaitTime);
    }

    private void Respawn()
    {
        _isAlive = true;
        if (IsPlayer())
        {
            // Set initial position and rotation
            player.movingPoint.transform.position = _spawnPoint;
            player.rotationPoint.rotation = _spawnRotation;
            // Enable the update script again
            player.enabled = true;
            // Enable collision detection
            player.movingPoint.enabled = true; 
            // Start drawing the model again
            player.rotationPoint.gameObject.SetActive(true);
        }
        else
        {
            // Set initial position and rotation
            character.transform.position = _spawnPoint;
            character.transform.rotation = _spawnRotation;
            character.SetActive(true);
        }
        
    }
}
