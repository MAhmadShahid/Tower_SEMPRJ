using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

public class TowerGateKeepingScript : MonoBehaviour
{

    [SerializeField] private TowerGateScript _towerGate;
    [SerializeField] private TowerStateManager _towerStateManager;
    [SerializeField] private GameObject _enterPromptUI;
    [SerializeField] private AudioManager _audioManager;

    bool _promptUIStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleTowerEnterPrompt();
    }

    void HandleTowerEnterPrompt()
    {
        if (_towerGate.isTriggered && !_promptUIStatus)
            TriggerPromptOn();
        else if (!_towerGate.isTriggered && _promptUIStatus)
            TriggerPromptOff();

    }

    public void TriggerPromptOn()
    {
        float goalY = 0;
        LeanTween.moveLocalY(_enterPromptUI, goalY, 0.5f).setEaseOutElastic();
        _promptUIStatus = true;
        _audioManager.Play("notification2");
    }

    public void TriggerPromptOff()
    {
        float goalY = 435.0f;
        LeanTween.moveLocalY(_enterPromptUI, goalY, 0.5f).setEaseOutBack();
        _promptUIStatus = false;
    }

    
}
