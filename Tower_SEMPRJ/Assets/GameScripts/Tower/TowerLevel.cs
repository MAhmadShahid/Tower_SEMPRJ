using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;


namespace Tower
{
    [System.Serializable]
    public class TowerLevel
    {
        public string _sceneName;
        public int _sceneIndex;
        public bool _locked;
        public bool _isLevelCompleted = false;

        public TowerLevel(string sceneName, int sceneIndex)
        {
            this._sceneName = sceneName;
            this._sceneIndex = sceneIndex;
        }

        public static void SetLevelCompleted(TowerLevel towerLevel)
        {
            towerLevel._isLevelCompleted = true;
        }

        public void UnlockLevel()
        {
            if (_locked) { _locked = false; }

        }

        public void LoadLevel()
        {
            SceneManager.LoadSceneAsync(_sceneIndex);
        }
    }
}

