using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public Slider timerSlider;
    public Text timerText;
    public float gameTime;

    private bool stopTimer;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    private void Update()
    {
        if (stopTimer)
            return;

        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            stopTimer = true;
            Level2Manager.Instance.RestartLevel();
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60f);
        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = textTime;
        timerSlider.value = gameTime;
    }

    public void IncreaseTime(float amount)
    {
        gameTime += amount;
    }


}
