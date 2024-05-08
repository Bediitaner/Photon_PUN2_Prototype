using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float maxVelocityChange = 4f;
    public float sprintSpeed = 14f;
    
    [Space] 
    public float jumpHeight = 30f;
    
    [Space] 
    public float airControl = 0.5f;
    
    private Vector2 _input;
    private Rigidbody _rb;
    private bool _isSprinting;
    private bool _isGrounded;
    private bool _isJumping;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb =GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _input.Normalize();

        _isSprinting = Input.GetButton("Sprint");
        _isJumping = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            if (_isJumping)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, jumpHeight, _rb.velocity.z);
            }
            else if (_input.magnitude > 0.5f)
            {
                _rb.AddForce(CalculateMovement(_isSprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = _rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                _rb.velocity = velocity1;
            }
        }
        else
        {
            if (_input.magnitude > 0.5f)
            {
                _rb.AddForce(CalculateMovement(_isSprinting ? sprintSpeed * airControl: walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = _rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                _rb.velocity = velocity1;
            }
        }
        
        _isGrounded = false;
    }

    private void OnTriggerStay(Collider other)
    {
        _isGrounded = true;
    }


    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(_input.x,0, _input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;
        
        Vector3 velocity = _rb.velocity;

        if (_input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            
            velocityChange.y = 0;

            return velocityChange;
        }
        else
        {
            return new Vector3();
        }
    }
}
