using Tower;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace Tower
{
    public class PlayerController : MonoBehaviour
    {

        //[Header("TestingVariables")]
        //public GameObject _inputDirectionArrow;
        //public GameObject _orientationDirectionArrow;
        //public GameObject _resultantDirectionArrow;

        [Header("Input")]
        [SerializeField] private GameInput _gameInput;

        [Header("Animation & Effects")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private GameObject _playerTrail;
        [SerializeField] private ParticleSystem[] _dashParticles;
        [SerializeField] private ParticleSystem _jumpParticles;
        [SerializeField] private ParticleSystem _dustParticles;
        private const string IS_WALKING = "isWalking";
        private const string JUMP = "jump";
        private const string LANDING = "isGoingToLand";
        private const string LANDED = "isLanded";

        [Header("Player")]
        [Space]

        // Input & Look Section
        [Header("Input & Look")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _playerVisualObject;
        [SerializeField] private Transform _orientation;
        [SerializeField] private float _rotationSpeed = 2.0f;


        private Vector3 _inputVector3D;
        private Vector3 _moveDirection;
        

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
        public bool _canJump = true;
        [Tooltip("The impulse force with which the player jumps")]
        [SerializeField] private float _jumpForce = 10.0f;
        [SerializeField] private float _secondJumpForce = 5.0f;
        [Tooltip("Is the player allowed to double jump ?")]
        public bool _enableDoubleJump = true;
        [Tooltip("The factor by which the character will fall towards the ground while airborne")]
        [SerializeField] private float _fallMultiplier = 2.5f;
        [Tooltip("At which point of its ascend will the player experience extra gravity to fall down quickly")]
        [SerializeField] private float _fallOffVelocity = 6.0f;
        [Tooltip("The time range during which, after leaving the ground, the player will be able to jump")]
        [SerializeField] private float _coyoteTime = 0.2f;

        private bool _inCoyoteZone;
        private bool _hasJumped;
        private bool _hasDoubleJumped;
        private float _lastTimeJumped;
        private float jumpTimerStart;
        private float _timeLeftGrounded = -10.0f; // arbitrary value

        // Grounding Section

        [SerializeField] private Transform _playerBase;
        [SerializeField] private LayerMask _floorMask;
        private bool _isGrounded;

        // Dashing Section
        [Header("Dashing")]
        public bool _canDash = true;
        [Tooltip("The impulse force added to the player controller in the direction of input")]
        [SerializeField] private float _dashingForce = 2.0f;
        [SerializeField] private int _consecutiveAllowedDashes;
        [SerializeField] private Cooldown _dashCoolDown;
        [SerializeField] private Cooldown _resetDashCoolDown;

        private bool _inDashState;
        private int _currentDashIndex;
        


        // Testing && Debugging Variables
        private float _maxJumpHeightReached = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            // locking away the cursor
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;

            _playerAnimator.SetBool(IS_WALKING, false);

            //_playerBase = transform.Find("Base");

            //if(_playerBase == null)
            //{
            //    Debug.LogError("Player base object not added!");
            //}
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            HandleGrounding();
            HandleWalking();
            HandleJumping();
            HandleDashing();
            HandleAnimations();
            HandlePlayerTrail();
            HandleParticles();
            //Debug.Log($"Velocity: {_rigidbody.velocity}");

            /*Code to check the maximum hieght reached by the player*/
            //var currentJumpHeight = transform.position.y;
            //if(currentJumpHeight > _maxJumpHeightReached ) _maxJumpHeightReached = currentJumpHeight;
            //Debug.Log($"Max hieght: {_maxJumpHeightReached}");
        }

        private void HandleInput()
        {
            // our move input will control movment along the x-z plane
            Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
            // projecting the 2D vector onto the 3D plane
            _inputVector3D = new Vector3(inputVector.x, 0.0f, inputVector.y);
            //_inputDirectionArrow.transform.forward = _inputVector3D.normalized;


            // for look direction extracted from the camera
            Vector3 cameraViewDirection = transform.position - new Vector3(_cameraTransform.position.x, transform.position.y, _cameraTransform.position.z);
            _orientation.forward = cameraViewDirection.normalized;
            //_orientationDirectionArrow.transform.forward = _orientation.forward;

            // Raycast downward to get the terrain normal at the player's position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    Vector3 terrainNormal = hit.normal;
                    cameraViewDirection = Vector3.ProjectOnPlane(cameraViewDirection, terrainNormal).normalized;
                }
            }

            // scale the orientation unit vector to the input vector
            _moveDirection = _inputVector3D.z * _orientation.forward + _inputVector3D.x * _orientation.right;
            //_resultantDirectionArrow.transform.forward = _moveDirection;

            if(_moveDirection != Vector3.zero)
                _playerVisualObject.forward = Vector3.Slerp(_playerVisualObject.forward, _moveDirection, Time.deltaTime * _rotationSpeed);


            // Debug.Log($"Camera Forward: {_cameraTransform.forward}\nPlayer Forward: {_playerVisualObject.transform.forward}");
        }

        private void HandleWalking()
        {

            Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

            // taking the input vector direction and setting its magnitude to the walk speed
            Vector3 directionVector = _moveDirection * _walkSpeed;

            // calculate the current walking penalty
            // i.e. what is the max speed our character is able to reach this frame because of the penalty
            if (directionVector != Vector3.zero && _isGrounded)
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

            _playerAnimator.SetBool(IS_WALKING, directionVector != Vector3.zero && _isGrounded); 

        }

        private void HandleJumping()
        {
            if(!_canJump) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Pressed Space: Jump!");

                _inCoyoteZone = Time.time < _timeLeftGrounded + _coyoteTime;
                // perform single jump
                if ((_isGrounded || _inCoyoteZone) && !_hasJumped)
                {
                    _playerAnimator.SetTrigger(JUMP);
                    _playerAnimator.SetBool("isLanded", false);
                    // add an impulse force for the jump
                    _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                    _jumpParticles.Play();
                    _hasJumped = true;
                    _lastTimeJumped = Time.time;
                    Debug.Log("First Jump");
                    
                }
                // perform double jump
                else if (!_isGrounded && _hasJumped && !_hasDoubleJumped && _enableDoubleJump)
                {
                    // reset the y velocity of the object before doing the second jump
                    // it will nullify the momentum of the first jump
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                    _rigidbody.AddForce(Vector3.up * _secondJumpForce, ForceMode.Impulse);
                    _hasDoubleJumped = true;
                    _jumpParticles.Play();

                    _playerAnimator.SetTrigger("DoubleJump");
                }

            }
            //else
            //{
            //    _playerAnimator.SetBool(JUMP, false);
            //}

            // Revert the jump states if the player has landed on the ground
            // if the object is grounded after jumping once or twice
            if (_isGrounded && _hasJumped)
            {
                // This will start checking after some time n after jumping
                // this helps to prevent reverting the state for a breif moment where the check sphere is still in contact with the ground after jumping
                float deltaTimeCheck = 0.25f;
                var elapsedTimeAfterJumping = Time.time - _lastTimeJumped;
                if (elapsedTimeAfterJumping > deltaTimeCheck)
                {
                    _hasJumped = false;
                    _hasDoubleJumped = false;
                    _playerAnimator.SetBool("isLanded", true);
                }

            }


            if ((_rigidbody.velocity.y < _fallOffVelocity) && !_isGrounded) //(_hasJumped || _hasDoubleJumped))
            {
                _rigidbody.velocity += Vector3.up * Physics.gravity.y * _fallMultiplier * Time.deltaTime;
            }
        }

        //private void HandleJumpingAfterDelay()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        jumpTimerStart = Time.time;
        //        Debug.Log("Pressed Space: Jump!");

        //        // perform single jump
        //        if ((_isGrounded || Time.time < _timeLeftGrounded + _coyoteTime) && !_hasJumped)
        //        {
        //            _playerAnimator.SetBool(JUMP, true);
        //            // add an impulse force for the jump
        //            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        //            _hasJumped = true;
        //            _lastTimeJumped = Time.time;
        //            Debug.Log("First Jump");

        //        }
        //        // perform double jump
        //        else if (!_isGrounded && _hasJumped && !_hasDoubleJumped && _enableDoubleJump)
        //        {
        //            // reset the y velocity of the object before doing the second jump
        //            // it will nullify the momentum of the first jump
        //            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        //            _rigidbody.AddForce(Vector3.up * _secondJumpForce, ForceMode.Impulse);
        //            _hasDoubleJumped = true;

        //            _playerAnimator.SetTrigger("DoubleJump");
        //            //_playerAnimator.SetBool(LANDED, _rigidbody.velocity.y > -10);
        //        }

        //    }
        //    else
        //    {
        //        _playerAnimator.SetBool(JUMP, false);
        //    }

        //    // Revert the jump states if the player has landed on the ground
        //    // if the object is grounded after jumping once or twice
        //    if (_isGrounded && _hasJumped)
        //    {
        //        // This will start checking after some time n after jumping
        //        // this helps to prevent reverting the state for a breif moment where the check sphere is still in contact with the ground after jumping
        //        float deltaTimeCheck = 0.25f;
        //        var elapsedTimeAfterJumping = Time.time - _lastTimeJumped;
        //        if (elapsedTimeAfterJumping > deltaTimeCheck)
        //        {
        //            _hasJumped = false;
        //            _hasDoubleJumped = false;
        //            _playerAnimator.SetBool("isLanded", true);
        //        }

        //    }


        //    if ((_rigidbody.velocity.y < _fallOffVelocity) && !_isGrounded) //(_hasJumped || _hasDoubleJumped))
        //    {
        //        _rigidbody.velocity += Vector3.up * Physics.gravity.y * _fallMultiplier * Time.deltaTime;
        //    }
        //}

        private void HandleGrounding()
        {
            bool nowGrounded = Physics.CheckSphere(_playerBase.transform.position, 0.5f, _floorMask);

            // if in this frame it starts being grounded
            if (!_isGrounded && nowGrounded)
            {

            }
            // if in this frame it starts being not grounded
            else if (_isGrounded && !nowGrounded)
            {
                _timeLeftGrounded = Time.time;
            }

            _isGrounded = nowGrounded;

            //if (_isGrounded) 
            //{ 
            //    Debug.Log($"Grounded = true, hasJumped = {_hasJumped}, hasDoubleJumped = {_hasDoubleJumped}"); 
            //}
            //else 
            //    Debug.Log($"Grounded = false, hasJumped = {_hasJumped}, hasDoubleJumped = {_hasDoubleJumped}");
        }

        private void HandleDashing()
        {

            if (!_canDash) return;

            if (!_resetDashCoolDown.IsCoolingDown())
            {
                // reset the dash index
                _currentDashIndex = 0;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !_dashCoolDown.IsCoolingDown())
            {
                
                // this if-else is to implement consecutive dashes and the cooldown once all dashes are utilized
                if (_currentDashIndex < _consecutiveAllowedDashes - 1)
                {
                    _currentDashIndex++;
                    // this is to reset the cooldown if the player only uses one dash, rather than consecutive dash
                    // start this cooldown for every dash other than the final dash
                    _resetDashCoolDown.InitiateCooldown();
                }
                else
                {
                    _dashCoolDown.InitiateCooldown();
                    _currentDashIndex = (_currentDashIndex + 1) % (_consecutiveAllowedDashes);
                }

                Debug.Log("'E' Pressed ! Dashing .....");
                Vector3 directionVector = _moveDirection;
                _rigidbody.AddForce(directionVector * _dashingForce, ForceMode.Impulse);

                _playerAnimator.SetBool("isDashing", true);
                _inDashState = true;

            }
            else
                _inDashState = false;
        }

        private void onPlayerFalling()
        {
            _playerAnimator.SetBool(LANDING, !_isGrounded && _rigidbody.velocity.y <= -10);
        }

        private void HandleAnimations()
        {
            _playerAnimator.SetBool(LANDED, _isGrounded);
            onPlayerFalling();
        }

        private void PlayDashAnimation()
        {
            
        }

        private void HandlePlayerTrail()
        {
            TrailRenderer trailRenderer = _playerTrail.GetComponent<TrailRenderer>();
            float trailTime = 0.0f;

            if (_rigidbody.velocity.magnitude > 11)
            {
                trailTime = 0.25f;
            }
            else
                trailTime = 0.0f;

            trailRenderer.time = Mathf.Lerp(trailRenderer.time, trailTime, 0.05f);
        }

        private void HandleParticles()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _rigidbody.velocity != Vector3.zero && _inDashState)
            {
                for(int counter = 0; counter < _dashParticles.Length; counter++)
                {
                    _dashParticles[counter].transform.forward = -_moveDirection.normalized;
                    _dashParticles[counter].transform.position = transform.position + _moveDirection.normalized * 1.5f;
                    _dashParticles[counter].Play();
                }
                //_playerDashLine.transform.forward = - _moveDirection.normalized;
                //_playerDashLine.transform.position = transform.position + _moveDirection.normalized * 1.5f;
                //_playerDashLine.Play();
            }

            //if (_rigidbody.velocity.x + _rigidbody.velocity.x != 0 && _isGrounded)
            //{
            //    _dustParticles.transform.forward = -_moveDirection.normalized;
            //    _dustParticles.transform.position = transform.position;
            //    _dustParticles.Play();
            //}
               
        }
    }

}

