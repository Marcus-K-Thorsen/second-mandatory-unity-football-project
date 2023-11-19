using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WallBouncer : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("The speed the Wall will be kicking the ball with")]
    public float Speed = 5f;

    public float DirectionX = 0f;
    public float DirectionZ = 0f;

    [SerializeField] GameObject ParentWall;
    
    private Vector3 _position;
    public Vector3 Position
    {
        get
        {
            _position = ParentWall.transform.position;
            _position.x += DirectionX;
            _position.z += DirectionZ;
            return _position;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
