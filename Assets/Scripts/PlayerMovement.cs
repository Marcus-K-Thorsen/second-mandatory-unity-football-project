using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [Tooltip("This is the Game Object that contains the whole player, as in this the whole player that moves around.")]
    public CharacterController movingPoint;
    
    [Tooltip("This is the Game Object that contains the model of the player, as in this the body of the player that turns around.")]
    public Transform rotationPoint;
    
    [Header("Stats")]
    [Tooltip("The maximum speed a player can move at.")]
    [SerializeField] float maxSpeed = 15f;
    
    [Tooltip("The momentum of the player's speed, if 0.5 then the player accelerates by half of its max speed every second.")]
    [SerializeField] float timeToMax = .55f;
    
    [Tooltip("The slowing of the player's speed, if 0.5 then the player de-accelerates by half of its max speed every second.")]
    [SerializeField] float timeToStop = .85f;
    
    [Tooltip("The momentum of the player, when going down a slope, but not certain of that or if needed at all.")]
    [SerializeField] float reverseMomentum = 2.2f;
    
    [Tooltip("The speed at which the player's model/body turns around.")]
    [SerializeField] float rotationSpeed = .1f;

    [Tooltip("The amount a player falls down when its local y position is higher than 0.")] 
    [SerializeField] float fallSpeed = .05f;
    
    
    public Vector3 Position => transform.position;
    
    public float Speed => Mathf.Abs(movement.x) + Mathf.Abs(movement.z);
    
    
    private float GainPerSecond => maxSpeed * timeToMax;
    private float LossPerSecond => maxSpeed * timeToStop;
    public Vector3 movement = Vector3.zero;

    // Set ud fra Main Kameraets vinkel
    private const KeyCode MovingLeftKey = KeyCode.LeftArrow;
    private const KeyCode MovingRightKey = KeyCode.RightArrow;
    private const KeyCode MovingUpKey = KeyCode.UpArrow;
    private const KeyCode MovingDownKey = KeyCode.DownArrow;
    
    
    
    private float _fallingHeight;

    private float FallingHeight
    {
        
        get => _fallingHeight;
        set
        {
            var localPosition = transform.localPosition;
            if (value < 0f)
            { 
                _fallingHeight = 0f;
            } else if (value > _fallingHeight)
            {
                _fallingHeight = value;
            } else if (localPosition.y <= 0)
            {
                _fallingHeight = 0f;
                _isFalling = false;
                ChangeLocalPosition(localPosition.x, 0f, localPosition.z);
            }
        }
    }

    private bool _isFalling;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var localPosition = transform.localPosition;
        float localPositionY = localPosition.y;
        if (localPositionY < 0f)
        {
            localPositionY = 0f;
            ChangeLocalPosition(localPosition.x, localPositionY, localPosition.z);
        }
        
        
        // Up/Down (X-axis)
        if (Input.GetKey(MovingLeftKey))
        {
            // Up-slope Positive
            if (movement.x >= 0)
            {
                movement.x += GainPerSecond * Time.deltaTime;
                if (movement.x > maxSpeed) movement.x = maxSpeed;
            }
            else
            {
                // Break-slope Negative
                movement.x += GainPerSecond * reverseMomentum * Time.deltaTime;
                if (movement.x > 0) movement.x = 0;
            }
        }
        else if (Input.GetKey(MovingRightKey))
        {
            // Down-slope Negative
            if (movement.x <= 0)
            {
                movement.x -= GainPerSecond * Time.deltaTime;
                if (movement.x < -maxSpeed) movement.x = -maxSpeed;
            }
            else
            {
                // Break-slope Positive
                movement.x -= GainPerSecond * reverseMomentum * Time.deltaTime;
                if (movement.x < 0) movement.x = 0;
            }
        }
        else
        {
            if (movement.x > 0)
            {
                // Fadeout from Positive
                movement.x -= LossPerSecond * Time.deltaTime;
                if (movement.x < 0) movement.x = 0;
            }
            else if (movement.x < 0)
            {
                // Fadeout from Negative
                movement.x += LossPerSecond * Time.deltaTime;
                if (movement.x > 0) movement.x = 0;
            }
        }

        // Left/Right (Y-axis)
        if (Input.GetKey(MovingDownKey))
        {
            // Up-slope Positive
            if (movement.z >= 0)
            {
                movement.z += GainPerSecond * Time.deltaTime;
                if (movement.z > maxSpeed) movement.z = maxSpeed;
            }
            else
            {
                // Break-slope Negative
                movement.z += GainPerSecond * reverseMomentum * Time.deltaTime;
                if (movement.z > 0) movement.z = 0;
            }
        }
        else if (Input.GetKey(MovingUpKey))
        {
            // Down-slope Negative
            if (movement.z <= 0)
            {
                movement.z -= GainPerSecond * Time.deltaTime;
                if (movement.z < -maxSpeed) movement.z = -maxSpeed;
            }
            else
            {
                // Break-slope Positive
                movement.z -= GainPerSecond * reverseMomentum * Time.deltaTime;
                if (movement.z < 0) movement.z = 0;
            }
        }
        else
        {
            if (movement.z > 0)
            {
                // Fadeout from Positive
                movement.z -= LossPerSecond * Time.deltaTime;
                if (movement.z < 0) movement.z = 0;
            }
            else if (movement.z < 0)
            {
                // Fadeout from Negative
                movement.z += LossPerSecond * Time.deltaTime;
                if (movement.z > 0) movement.z = 0;
            }
        }
        
        

        if (movement.x != 0 || movement.z != 0)
        {
            // Only move when necessary
            movingPoint.Move(movement * Time.deltaTime);
            rotationPoint.rotation = Quaternion.Slerp(
                rotationPoint.rotation,
                Quaternion.LookRotation(movement),
                rotationSpeed
                ); 
        }
        
        FallGentle();
    }

    

    private void FallGentle()
    {
        bool isNotGrounded = !movingPoint.isGrounded;
        
        if (isNotGrounded && !_isFalling)
        {
            _isFalling = true;
        }
        var localPosition = transform.localPosition;
        float localPositionY = localPosition.y;
        float newFallingHeight = localPosition.y;
         
        FallingHeight = newFallingHeight;
        // isNotGrounded 
         
        if (_isFalling && FallingHeight > 0f) 
        { 
            if (localPositionY > 0f) 
            { 
                localPositionY -= FallingHeight * fallSpeed;
                if (localPositionY < 0f)
                {
                     localPositionY = 0f;
                }
                ChangeLocalPosition(localPosition.x, localPositionY, localPosition.z); 
            }
            else 
            {
                ChangeLocalPosition(localPosition.x, 0f, localPosition.z); 
                FallingHeight = 0f; 
            } 
        }
    }

    private void ChangeLocalPosition(float localPositionX, float localPositionY, float localPositionZ)
    {
        transform.localPosition = new Vector3(localPositionX, localPositionY, localPositionZ);
    }
}
