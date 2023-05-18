using Tower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tower
{
    public class GameInput : MonoBehaviour
    {
        [Tooltip("The InputActionAsset we will be using to manage actions")]
        [SerializeField] private PlayerInputActions _playerInputActions;
        void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }


        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
            return inputVector.normalized;
        }

    }
}

