using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Bullet")] 
    public float speed=24;
    public float range = 70;
    private Vector3 startPoint;
    private Transform trans;
    
    
    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        startPoint = trans.position;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        // Move the projectile
        trans.Translate(0,0,speed * Time.deltaTime, Space.Self);
        // Check how far we have travelled
        if (Vector3.Distance(trans.position, startPoint) > range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
