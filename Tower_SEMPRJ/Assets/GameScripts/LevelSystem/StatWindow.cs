using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatWindow : MonoBehaviour
{
    [SerializeField] private GameObject _statWindow;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _experiencePanel;

    [SerializeField] private TextMeshProUGUI _abilityTextList;

    // reference to level system
    [SerializeField] private LevelSystemManager _levelSystemManager;
    private LevelSystem _levelSystem;

    // Start is called before the first frame update
    void Start()
    {
        _levelSystem = _levelSystemManager.GetLevelSystem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpStatWindow()
    {
        _levelText.text = "Level" + _levelSystem.GetLevelNumber().ToString();
        // setup the experience bar

        foreach(string ability in _levelSystem.GetAbilityList()) 
        {
            AddAbilityToDisplayList(ability);
        }
    }

    private void UpdateStatWindow()
    {

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
