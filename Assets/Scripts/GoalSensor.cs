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
    
    private int _goalLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        _goalLayer = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Først tjekker vi om det er bolden, som er på layer 9, der har Triggered vores GoalSensor.
        if (other.gameObject.layer == 9)
        {
            GameController controller = gameController.GetComponent<GameController>();
            controller.GoalWasScored();
        }
    }
}
