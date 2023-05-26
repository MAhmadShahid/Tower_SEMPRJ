using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemManager : MonoBehaviour
{
    private LevelSystem _levelSystem;

    private void Awake()
    {
        _levelSystem = new LevelSystem(0, 0, 100);
        DontDestroyOnLoad(gameObject);
    }

   
    public LevelSystem GetLevelSystem() => _levelSystem;
}
