using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Level _currentLevel;
    [SerializeField] private List<Level> _levelsList = new List<Level>();

    private Level _mainLevel = new Level("MainMenuScene", false, 0);
    private Level _lobbyLevel = new Level("LobbyScene", false, 0);


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddLevel(Level levelToAdd)
    {
        _levelsList.Add(levelToAdd);
    }
}
