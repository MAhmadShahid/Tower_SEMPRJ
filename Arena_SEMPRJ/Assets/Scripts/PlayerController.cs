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
    [SerializeField] private float _walkSpeed = 1.0f;

    private Rigidbody _rigidbody;

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
        Vector3 directionVector = new Vector3(inputVector.x, 0, inputVector.y);

        _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, directionVector * _walkSpeed, Time.deltaTime * 100);

    }

    private void HandleJumping()
    {

    }
}
