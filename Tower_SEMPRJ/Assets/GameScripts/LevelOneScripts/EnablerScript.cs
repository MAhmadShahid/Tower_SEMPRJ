using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

public class EnablerScript : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        _playerController = gameObject.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider collidedWith)
    {
        string colliderName = collidedWith.gameObject.name;

        switch (colliderName)
        {
            case "JumpEnabler":
                if (!_playerController._canJump)
                {
                    _playerController._canJump = true;
                }
                break;
            case "DoubleJumpEnabler":
                if(!_playerController._enableDoubleJump)
                {
                    _playerController._enableDoubleJump = true;
                }
                break;
            default: 
                break;
        }
    }
}
