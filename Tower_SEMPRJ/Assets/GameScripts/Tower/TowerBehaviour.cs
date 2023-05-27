using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    [SerializeField] private TowerStateManager _towerStateManager;

    // the notification part can be refactored and implemented as a different notification
    [SerializeField] private PlayerHUDScript _playerHUD;
    [SerializeField] private GameObject _directionArrow;
    
    // Start is called before the first frame update
    void Start()
    {
        if(_towerStateManager.IsLevelLocked("Level1") || !_towerStateManager.IsLevelCompleted("Level1"))
        {
            InvokeRepeating("CallForPlayer", 5.0f, 25.0f);
            Debug.Log("Level1 Locked!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CallForPlayer()
    {
        _playerHUD.PlayNotification("The tower is calling for you");
        SummonDirectionArrow();
    }

    void SummonDirectionArrow()
    {
        Instantiate(_directionArrow);
    }
}
