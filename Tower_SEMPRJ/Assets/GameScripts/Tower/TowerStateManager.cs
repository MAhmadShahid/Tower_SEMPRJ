using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class TowerStateManager : MonoBehaviour
    {
        [SerializeField] private TowerLevel _mainMenuLevel = new TowerLevel("MainMenuScene", 0);
        [SerializeField] private TowerLevel _lobbyLevel = new TowerLevel("LobbyScene", 1);
        [SerializeField] private List<TowerLevel> _levelList;

        // will store the current level, its index and completed status
        private TowerLevel _currentLevel;
        private TowerLevel _latestUnlockedLevel;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentLevel = _lobbyLevel;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadMainMenu()
        {
            _currentLevel = _mainMenuLevel;
            _currentLevel.LoadLevel();
        }

        public void LoadLobbyScene()
        {
            _currentLevel = _lobbyLevel;
            _currentLevel.LoadLevel();
        }


        public void LoadNextLevel()
        {
            int indexOfNextLevel = _currentLevel._sceneIndex + 1;

            if (_levelList[indexOfNextLevel]._locked && !_currentLevel._isLevelCompleted)
            {
                Debug.Log("Next level is locked! OR Current level not completed");
                return;
            }

            _currentLevel = _levelList[indexOfNextLevel];
            _currentLevel.LoadLevel();
        }

        public void LoadLevelByName(string sceneName)
        {
            _levelList.Find(level => level._sceneName == sceneName).LoadLevel();
        }

        public void LoadLevelByIndex(int sceneIndex)
        {
            _levelList.Find(level => level._sceneIndex == sceneIndex).LoadLevel();
        }

        public List<int> ReturnUnlockedLevels()
        {
            List<int> unlockedLevelList = new List<int>();
            for (int counter = 2; counter < _levelList.Count; counter++)
            {
                if (_levelList[counter]._isLevelCompleted)
                    unlockedLevelList.Add(_levelList[counter]._sceneIndex);
            }

            return unlockedLevelList;
        }

        public void AddLevel(TowerLevel levelToAdd)
        {
            _levelList.Add(levelToAdd);
        }

        public void PrepareNextLevelAndLoadNextLevel()
        {
            _currentLevel._isLevelCompleted = true;
            _levelList[_currentLevel._sceneIndex + 1]._locked = false;
            LoadNextLevel();
        }

        public bool IsLevelCompleted(string levelName) 
        {
            return _levelList.Find(x => x._sceneName == levelName)._isLevelCompleted;
        }

        public bool IsLevelLocked(string levelName)
        {
            return _levelList.Find(x => x._sceneName == levelName)._locked;
        }
    }
}

