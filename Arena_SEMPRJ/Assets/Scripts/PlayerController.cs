using Arena;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private GameInput _gameInput;

    [Header("Player")]
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

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleWalking();
        HandleJumping();
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

    }
}
