using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatWindow : MonoBehaviour
{
    [SerializeField] private GameObject _statWindow;
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private Transform _experienceBar;

    [SerializeField] private TextMeshProUGUI _abilityTextList;

    // reference to level system
    [SerializeField] private LevelSystemManager _levelSystemManager;
    private LevelSystem _levelSystem;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelSystem = _levelSystemManager.GetLevelSystem();
        SetUpStatWindow();

        _levelSystem.OnLevelChanged += _levelSystem_OnLevelChanged;
        _levelSystem.OnExperienceChanged += _levelSystem_OnExperienceChanged;
    }

    private void _levelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        UpdateExperienceVisual();
    }

    private void _levelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        UpdateLevelVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpStatWindow()
    {
        UpdateLevelVisual();
        UpdateExperienceVisual();

        foreach(string ability in _levelSystem.GetAbilityList()) 
        {
            AddAbilityToDisplayList(ability);
        }
    }


    private void UpdateLevelVisual()
    {
        int currentLevel = _levelSystem.GetLevelNumber();
        _levelNumber.text = currentLevel.ToString();
    }

    private void UpdateExperienceVisual()
    {
        Tuple<int, int> experience = _levelSystem.GetExperienceTuple();
        float experienceNormalized = (float)experience.Item1 / experience.Item2;

        _experienceBar.LeanScaleX(experienceNormalized, 0.0f);
    }

    private void AddAbilityToDisplayList(string p_ability)
    {
        _abilityTextList.text += '\n' + p_ability;
    }

    public void PlayOpeningAnimation()
    {
        float goalY = 0;
        LeanTween.moveLocalY(_statWindow, goalY, 0.5f).setEaseOutCubic();
    }

    public void PlayClosingAnimation()
    {
        float goalY = -1034;
        LeanTween.moveLocalY(_statWindow, goalY, 0.5f).setEaseInBack();
    }


    // private void SetExperienceBarSize(float p_experienceNormalized)
    // private void SetLevelNumber(int p_levelNumber)

}
