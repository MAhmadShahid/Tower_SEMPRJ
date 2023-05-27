using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem 
{
    // FIELDS


    private int _currentLevel;
    private int _currentExperience;
    private int _experienceToNextLevel;

    // abilites
    private List<string> _abilites;

    // events
    public event EventHandler OnLevelChanged;
    public event EventHandler OnExperienceChanged;
    

    public LevelSystem(int p_currentLevel, int p_currentExperience, int p_experienceToNextLevel)
    {
        _currentLevel = p_currentLevel;
        _currentExperience = p_currentExperience;
        _experienceToNextLevel = p_experienceToNextLevel;

        _abilites = new List<string>();
        _abilites.Add("Jump");
        _abilites.Add("Double Jump");

    }

    public void AddExperience(int experienceToAdd)
    {
        _currentExperience += experienceToAdd;
        while(_currentExperience >= _experienceToNextLevel) 
        {
            _currentLevel++;
            _currentExperience -= _experienceToNextLevel;

            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddAbility(string p_abilityToAdd)
    {
        _abilites.Add(p_abilityToAdd);
    }

    public int GetLevelNumber() => _currentLevel;
    public Tuple<int, int> GetExperienceTuple() => new Tuple<int, int>(_currentExperience, _experienceToNextLevel);
    public List<string> GetAbilityList() => _abilites;
}
