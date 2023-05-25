using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Tower;

public class LevelOneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Checkpoint _startingCheckpoint;

    [SerializeField] private List<Checkpoint> _checkpointList;
    private Checkpoint _currentActiveCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform.position = _startingCheckpoint._checkpointTransform.position;
        _currentActiveCheckpoint = _startingCheckpoint;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayer();
        HandleCheckpoints();
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
}
