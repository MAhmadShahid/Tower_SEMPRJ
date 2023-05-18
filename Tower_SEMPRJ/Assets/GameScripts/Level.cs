using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public string _sceneName { get; protected set; }
    public bool _isLevelCompleted { get; private set; }
    public int _levelIndex { get; private set; }

    public Level(string sceneName, bool isLevelCompleted, int levelIndex)
    {
        _sceneName = sceneName;
        _isLevelCompleted = isLevelCompleted;
        _levelIndex = levelIndex;
    }

    public void SetLevelCompleted() { _isLevelCompleted = true; }
}
