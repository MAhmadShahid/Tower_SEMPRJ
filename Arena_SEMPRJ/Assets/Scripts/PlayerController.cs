using Arena;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private GameInput _gameInput;

    [Header("Player")]
    [Space]

    // Walking Section

    [Header("Walking")]
    [Tooltip("Hieght of the player model")]
    [SerializeField] private float _playerHeight = 1.0f;
    [Tooltip("The speed with which the player walks")]
    [SerializeField] private float _walkSpeed = 1.0f;
    [Tooltip("The acceleration of the player controller")]
    [SerializeField] private float _acceleration = 2.0f;
    [Tooltip("The speed at which the player reaches its required velocity.")]
    [SerializeField] private float _walkLerpSpeed = 100.0f;
    [Tooltip("The amount of penaly in percentage that the character will suffer for not moving")]
    [SerializeField] private float _maxWalkingPenalty = 0.5f;

    private Rigidbody _rigidbody;
    private float _currentWalkingPenalty = 1.0f;

    // Jumping Section

    [Header("Jumping")]
    [Tooltip("The impulse force with which the player jumps")]
    [SerializeField] private float _jumpForce = 10.0f;
    [SerializeField] private float _secondJumpForce = 5.0f;
    [Tooltip("Is the player allowed to double jump ?")]
    [SerializeField] private bool _enableDoubleJump = true;
    [Tooltip("The factor by which the character will fall towards the ground while airborne")]
    [SerializeField] private float _fallMultiplier = 2.5f;
    [Tooltip("At which point of its ascend will the player experience extra gravity to fall down quickly")]
    [SerializeField] private float _fallOffVelocity = 6.0f;

    private bool _hasJumped;
    private bool _hasDoubleJumped;
    private float _lastTimeJumped;

    // Grounding Section

    [SerializeField] private Transform _playerBase;
    [SerializeField] private LayerMask _floorMask;
    private bool _isGrounded;

    // Testing && Debugging Variables
    private float _maxJumpHeightReached = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_playerBase = transform.Find("Base");

        //if(_playerBase == null)
        //{
        //    Debug.LogError("Player base object not added!");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        HandleGrounding();
        HandleWalking();
        HandleJumping();

        Debug.Log($"Velocity: {_rigidbody.velocity}");

        /*Code to check the maximum hieght reached by the player*/
        //var currentJumpHeight = transform.position.y;
        //if(currentJumpHeight > _maxJumpHeightReached ) _maxJumpHeightReached = currentJumpHeight;
        //Debug.Log($"Max hieght: {_maxJumpHeightReached}");
    }

    private void HandleWalking()
    {
        // our move input will control movment along the x-z plane
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        // projecting the 2D vector onto the 3D plane
        // and setting its magnitude to the walk speed
        Vector3 directionVector = new Vector3(inputVector.x, 0, inputVector.y) * _walkSpeed;

        // calculate the current walking penalty
        // i.e. what is the max speed our character is able to reach this frame because of the penalty
        if (directionVector != Vector3.zero)
            _currentWalkingPenalty += _acceleration * Time.deltaTime;
        else
            _currentWalkingPenalty -= _acceleration * Time.deltaTime;
        
        // keep the penalty between the range of 0.5 and 1.
        _currentWalkingPenalty = Mathf.Clamp(_currentWalkingPenalty, _maxWalkingPenalty, 1);

        // multiply the direction vector with this penalty
        directionVector *= _currentWalkingPenalty;

        // calculate the velocity vector in the direction of input
        // leave the y component as it is
        Vector3 targetVelocity = new Vector3(directionVector.x, _rigidbody.velocity.y, directionVector.z);

        // smoothly transition between current velocity and the target velocity with steps equal to the walkLerpSpeed * Time.deltaTime
        _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, targetVelocity, _walkLerpSpeed * Time.deltaTime);

    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed Space: Jump!");

            if (_isGrounded && !_hasJumped)
            {
                // add an impulse force for the jump
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _hasJumped = true;
                _lastTimeJumped = Time.time;
                Debug.Log("First Jump");
            }
            else if (!_isGrounded && _hasJumped && !_hasDoubleJumped && _enableDoubleJump)
            {
                // reset the y velocity of the object before doing the second jump
                // it will nullify the momentum of the first jump
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                _rigidbody.AddForce(Vector3.up * _secondJumpForce, ForceMode.Impulse);
                _hasDoubleJumped = true;
            }

        }

        // Revert the jump states if the player has landed on the ground
        // if the object is grounded after jumping once or twice
        if(_isGrounded && _hasJumped)
        {
            // This will start checking after some time n after jumping
            // this helps to prevent reverting the state for a breif moment where the check sphere is still in contact with the ground after jumping
            float deltaTimeCheck = 0.25f;
            var elapsedTimeAfterJumping = Time.time - _lastTimeJumped;
            if(elapsedTimeAfterJumping > deltaTimeCheck)
            {
                _hasJumped = false;
                _hasDoubleJumped = false;
            } 
        }


        if((_rigidbody.velocity.y < _fallOffVelocity) && !_isGrounded)//(_hasJumped || _hasDoubleJumped))
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * _fallMultiplier * Time.deltaTime;
        }

        
    }

    private void HandleGrounding()
    {
        _isGrounded = Physics.CheckSphere(_playerBase.transform.position, 0.30f, _floorMask);
        
        //if (_isGrounded) 
        //{ 
        //    Debug.Log($"Grounded = true, hasJumped = {_hasJumped}, hasDoubleJumped = {_hasDoubleJumped}"); 
        //}
        //else 
        //    Debug.Log($"Grounded = false, hasJumped = {_hasJumped}, hasDoubleJumped = {_hasDoubleJumped}");
    }
}
