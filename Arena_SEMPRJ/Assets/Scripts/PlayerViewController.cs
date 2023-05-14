using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Arena
{
    public class PlayerViewController : MonoBehaviour
    {
        [Header("References")]
        public Transform _orientation;
        public GameObject _player;
        public GameObject _playerObject;
        public GameInput _gameInput;

        private Rigidbody _playerRigidBody;
        // Start is called before the first frame update
        void Start()
        {
            _playerRigidBody = _player.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            // get the normalized vector of where the camera is looking at in the x-z plane, excluding the y axis
            Vector3 cameraViewDirection = _player.transform.position - new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
            Vector3 cameraViewDirectionNormalized = cameraViewDirection.normalized;
            _orientation.forward = cameraViewDirectionNormalized;

            Vector3 inputVector2D = _gameInput.GetMovementVectorNormalized();


        }
    }
}

