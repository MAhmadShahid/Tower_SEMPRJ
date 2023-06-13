using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Tower;
using UnityEngine.UI;

public class LevelOneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Checkpoint _startingCheckpoint;

    [SerializeField] private List<Checkpoint> _checkpointList;
    private Checkpoint _currentActiveCheckpoint;

    [SerializeField] private TowerGateScript _gateScript;
    [SerializeField] private GameObject _exitPromptUI;
    bool _promptUIStatus;


    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _lobbyButton;

    private TowerStateManager _towerStateManager;

    // Start is called before the first frame update
    void Start()
    {
        _towerStateManager = GameObject.Find("TowerStateManager").GetComponent<TowerStateManager>();


        _playerTransform.position = _startingCheckpoint._checkpointTransform.position;
        _currentActiveCheckpoint = _startingCheckpoint;
        Debug.Log(_currentActiveCheckpoint._checkpointTransform + $" {_currentActiveCheckpoint._checkpointPriority.ToString()}");

        _nextLevelButton.onClick.AddListener(_towerStateManager.LoadNextLevel);
        _lobbyButton.onClick.AddListener(_towerStateManager.LoadLobbyScene);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayer();
        // HandleCheckpoints();
        HandleUIPrompt();
    }

    void HandlePlayer()
    {
        if (_playerTransform.position.y < 15)
            _playerTransform.position = _currentActiveCheckpoint._checkpointTransform.position;
    }

    void HandleCheckpoints()
    {
        // some other needed implementation over here
    }

    public void UpdateCheckpoint(int p_checkpointIndex)
    {
        if (p_checkpointIndex < _currentActiveCheckpoint._checkpointPriority) return;

        _currentActiveCheckpoint = _checkpointList.Find(checkpoint => checkpoint._checkpointPriority == p_checkpointIndex);
    }

    //private void OnTriggerEnter(Collider collidedWith)
    //{
    //    if(collidedWith.gameObject.name.StartsWith("Checkpoint"))
    //    {

    //    }
    //}

    void HandleUIPrompt()
    {
        if (_gateScript.isTriggered && !_promptUIStatus)
            TriggerPromptOn();
        else if (!_gateScript.isTriggered && _promptUIStatus)
            TriggerPromptOff();
    }


    public void TriggerPromptOn()
    {
        float goalY = 0;
        LeanTween.moveLocalY(_exitPromptUI, goalY, 0.5f).setEaseOutElastic();
        _promptUIStatus = true;
        Debug.Log("On");
    }

    public void TriggerPromptOff()
    {
        float goalY = 695.0f;
        LeanTween.moveLocalY(_exitPromptUI, goalY, 0.5f).setEaseOutBack();
        _promptUIStatus = false;
        Debug.Log("Off");
    }



}
