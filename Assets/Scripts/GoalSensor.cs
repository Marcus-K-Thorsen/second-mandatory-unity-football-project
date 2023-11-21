using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GoalSensor : MonoBehaviour
{
    [Header("References")] 
    [Tooltip("The GameObject.")]
    public GameObject gameController;
    [SerializeField] private AudioSource cheeringAtGoal;

    private string GoalOwner
    {
        get
        {
            if (gameObject.layer == 11) // layer 11 er lig med Player's goal sensor
            {
                return "player";
            } else if (gameObject.layer == 12) // layer 12 er lig med Enemies' goal sensor
            {
                return "enemy";
            }
            else // Ellers er det en goal sensor der ikke er blevet sat til et valid layer
            {
                return "default";
            }
        }
    }

    private bool _goalWasNotScored;

    // Start is called before the first frame update
    void Start()
    {
        _goalWasNotScored = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Først tjekker vi om det er bolden, som er på layer 9, der har Triggered vores GoalSensor.
        if (other.gameObject.layer == 9 && _goalWasNotScored)
        {
            _goalWasNotScored = false;
            GameController controller = gameController.GetComponent<GameController>();
            
            if (GoalOwner == "player")
            {
                cheeringAtGoal.Play();
                controller.GoalWasScored(false);
            }

            if (GoalOwner == "enemy")
            {
                cheeringAtGoal.Play();
                controller.GoalWasScored(true);
            }
        }
    }
}
