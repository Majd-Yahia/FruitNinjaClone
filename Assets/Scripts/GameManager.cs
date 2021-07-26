using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using TMPro;

public enum Mode
{
    NULL = 0,
    Points,
    Timer,
    Count
}

public class GameManager : MonoBehaviour
{

#if UNITY_IOS
    string gameId = "4230254";
#else
    string gameId = "4230255";
#endif

    public bool isGameDone;
    public int tries;
    public float m_TimerMinutes;
    public float m_TimerSeconds;
    public Mode mode;

    // UI
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI modeText;
    // End 
    [SerializeField] private TextMeshProUGUI GoalText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI PointsText;
    [SerializeField] private TextMeshProUGUI BestText;
    [SerializeField] private int PointsToWin;
    private Blade blade;
    private bool isWon = false;
    private void SetBestText() { BestText.text = PlayerPrefs.GetInt("Best", 0).ToString(); }
    private void SetPointsText() { PointsText.text = blade.Points.ToString(); }
    private void SetTriesText() { TimerText.text = tries.ToString().PadLeft(2, '0'); }
    private void GoalReached() { isWon = blade.Points >= PointsToWin ? true : false; }
    private void SetGoalText() { GoalText.text = PointsToWin.ToString(); }

    private void Awake()
    {
        Advertisement.Initialize(gameId);
        blade = FindObjectOfType<Blade>();
        Setup();
    }

    private void Start()
    {
        SwitchMode();
    }

    private void Setup()
    {
        CloseMenu();
        SetTimerAndPointsRandom();

        int currentMode = PlayerPrefs.GetInt("Mode", 1);
        if (currentMode == (int)Mode.Timer)
        {
            SetTimerText();
        }
        else
        {
            SetTriesText();
        }

        SetPointsText();
        SetGoalText();
        SetBestText();
    }

    private void SetTimerAndPointsRandom()
    {
        PointsToWin = Random.Range(50, 1000);
        if (PointsToWin < 150)
        {
            m_TimerMinutes = Random.Range(0, 2);
            if (m_TimerMinutes == 0)
            {
                m_TimerSeconds = Random.Range(25, 60);
            }
            else
            {
                m_TimerSeconds = 0;
            }
        }
        else if (PointsToWin < 250)
        {
            m_TimerMinutes = Random.Range(2, 4);
            m_TimerSeconds = 0;
        }
        else
        {
            m_TimerMinutes = Random.Range(4, 6);
            m_TimerSeconds = 0;
        }

    }


    private void SwitchMode()
    {
        switch (PlayerPrefs.GetInt("Mode", 1))
        {
            case 1:
                mode = Mode.Points;
                StartCoroutine(PointsMode());
                break;
            case 2:
                mode = Mode.Timer;
                StartCoroutine(TimerMode());
                break;
        }
    }

    private void TimerCountDown()
    {
        if (m_TimerSeconds > 0)
        {
            m_TimerSeconds--;
        }
        else
        {
            if (m_TimerMinutes > 0)
            {
                m_TimerMinutes--;
                m_TimerSeconds = 60;
            }
            else
            {
                isGameDone = true;
            }
        }
    }

    private void SetTimerText()
    {
        if (m_TimerSeconds < 0)
        {
            m_TimerSeconds = 0;
        }

        string seconds = m_TimerSeconds.ToString().PadLeft(2, '0');
        TimerText.text = string.Format($"{m_TimerMinutes}:{seconds}");
    }

    private IEnumerator TimerMode()
    {
        Debug.Log("TimerMode Called");

        while (!isGameDone && !isWon)
        {
            yield return new WaitForSeconds(1f);                // wait one second
            SetTimerText();                                     // set up timer on UI.
            SetPointsText();                                    // set up points on UI.
            TimerCountDown();                                   // count down the timer.
            GoalReached();                                      // check if goal reached.
        }

        GameEndSequance();
    }

    private IEnumerator PointsMode()
    {
        while (blade.Points < PointsToWin && !isWon && tries > 0)
        {
            yield return new WaitForSeconds(0.5f);
            SetPointsText();                                    // set up points on UI.
            SetTriesText();                                     // set up tries on UI.
            GoalReached();                                      // check if goal reached.
        }

        GameEndSequance();
    }

    private void GameEndSequance()
    {
        isGameDone = true;
        PlayerPrefs.SetInt("Best", blade.Points);

        if (mode == Mode.Timer)
        {
            modeText.text = "Timer";
        }
        else
        {
            modeText.text = "Points";
        }

        PauseGame();

        PlayAd();

        OpenMenu();
    }

    private void CloseMenu() { gameOverPanel.SetActive(false); }
    private void OpenMenu() { gameOverPanel.SetActive(true); }
    public void Exit() { Application.Quit(); }
    public void Repeat() { UnpauseGame(); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    private void UnpauseGame() { Time.timeScale = 1f; }
    void PauseGame()
    {
        Time.timeScale = 0f;
        Rigidbody[] objects3D = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rigid in objects3D)
        {
            rigid.useGravity = false;
        }

        Rigidbody2D[] objects2D = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rigid in objects2D)
        {
            rigid.gravityScale = 0;
        }
    }

    public void ChangeMode()
    {
        int currentMode;
        if (modeText.text == "Timer")
        {
            modeText.text = "Points";
            currentMode = 1;
        }
        else
        {
            modeText.text = "Timer";
            currentMode = 2;
        }

        PlayerPrefs.SetInt("Mode", currentMode);
    }

    private void PlayAd()
    {
        if (Advertisement.IsReady("Interstitial_Android"))
        {
            Advertisement.Show("Interstitial_Android");
        }
    }
}
