using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDScript : MonoBehaviour
{
    // level system
    [SerializeField] private LevelSystemManager _levelSystemManager;
    private LevelSystem _levelSystem;

    // level and experience
    [SerializeField] private GameObject _playerHUDContainer;
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private Transform _experienceBar;

    // buttons
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _volumeButton;
    [SerializeField] private Button _abilityButton;

    // notification bar
    [SerializeField] private Transform _notificationContainer;
    [SerializeField] private TextMeshProUGUI _notificationText;


    // audio manager reference
    [SerializeField] private AudioManager _audioManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        _levelSystem = _levelSystemManager.GetLevelSystem();
        _levelSystem.OnLevelChanged += _levelSystem_OnLevelChanged;
        _levelSystem.OnExperienceChanged += _levelSystem_OnExperienceChanged;

        UpdateExperienceVisual();
        UpdateLevelVisual();
    }

    private void _levelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        UpdateExperienceVisual();
    }

    private void _levelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        UpdateLevelVisual();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Level = {_levelSystem.GetLevelNumber()}");
        Debug.Log($"Experience = {_levelSystem.GetExperienceTuple().Item1}");
    }

    private void UpdateLevelVisual()
    {
        int currentLevel = _levelSystem.GetLevelNumber();
        _levelNumber.text = currentLevel.ToString();
    }


    private void UpdateExperienceVisual()
    {
        Tuple<int, int> experience = _levelSystem.GetExperienceTuple();
        float experienceNormalized = (float)experience.Item1/ experience.Item2;
        
        _experienceBar.LeanScaleX(experienceNormalized, 0.25f).setEaseOutExpo();
 
    }

    public void Add50Experience()
    {
        _levelSystem.AddExperience(50);
    }

    public void PlayNotification(string p_notificationMessage)
    {
        _notificationText.text = p_notificationMessage;
        LeanTween.moveLocalX(_notificationContainer.gameObject, 520, 1.0f).setEaseOutBounce();
        _audioManager.Play("notification");
        StartCoroutine(CloseNotificationAfterDelay(5.0f));
    }

    IEnumerator CloseNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LeanTween.moveLocalX(_notificationContainer.gameObject, 1400, 1.0f).setEaseOutQuad();
    }
}
