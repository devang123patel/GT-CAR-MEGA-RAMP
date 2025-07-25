using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnowManager : MonoBehaviour
{
    public static SnowManager instance;

    public GameObject gameplayScreen, gameOverScreen;

    public GameObject player;
    public GameObject snowCityHolder;
    public GameObject uiCollections;

    public Transform snowLivesHolder;
    public GameObject snowLivePrefab;

    public Transform snowTreeHolder;
    public List<GameObject> snowPrefabs;

    public Transform snowBallHolder;
    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;

    public int score;
    public Text scoreTxt;

    public int highScore;
    public Text highestScoreTxt;

    public int liveCount = 5;
    public List<Image> livesList;

    public Text countDownTxt;

    public Button tapToPlaybtn;

    public ParticleSystem particalSystem;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Play();

        highScore = PlayerPrefs.GetInt("HighestSnow");
        highestScoreTxt.text = ("Highest  " + highScore.ToString());
    }

    public void ResetSnowMode()
    {
        // Reset game values
        score = 0;
        liveCount = 0;
        scoreTxt.text = "Score: 0";

        // Reset high score display
        highScore = PlayerPrefs.GetInt("HighestSnow");
        highestScoreTxt.text = "Highest  " + highScore.ToString();

        // Reset snow tree speeds
        for (int i = 0; i < snowPrefabs.Count; i++)
        {
            if (snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>() != null)
                snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 10;
        }

        // Reset all lives to active
        foreach (Image life in livesList)
        {
            life.gameObject.SetActive(true);
        }

        // Reset countdown
        countDown = 4;

        // Stop particle system
        if (particalSystem != null)
            particalSystem.Stop();

        // Make sure player is active
        player.SetActive(true);

        // Hide UI initially (will show after countdown)
        uiCollections.SetActive(false);
        pausebtnImage.gameObject.SetActive(false);

        // Cancel all invokes
        CancelInvoke();
    }

    public void BuildGenrator()
    {
        int randomTree = Random.Range(0, snowPrefabs.Count);
        GameObject clone = Instantiate(snowPrefabs[randomTree], snowTreeHolder);

        if (score >= 50)
        {
            Debug.Log("50 Score Passed - Increasing Snow Tree Speed");
            for (int i = 0; i < snowPrefabs.Count; i++)
            {
                if (snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>() != null)
                    snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 13;
            }
        }

        if (score >= 100)
        {
            Debug.Log("100 Score Passed - Increasing Snow Tree Speed");
            for (int i = 0; i < snowPrefabs.Count; i++)
            {
                if (snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>() != null)
                    snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 15;
            }
        }
    }

    public void Ball1()
    {
        GameObject ball1clone = Instantiate(ball1, snowBallHolder);
    }

    public void Ball2()
    {
        GameObject ball2clone = Instantiate(ball2, snowBallHolder);
    }

    public void Ball3()
    {
        GameObject ball3clone = Instantiate(ball3, snowBallHolder);
    }

    public void LivesHandlers()
    {
        if (liveCount < livesList.Count)
        {
            livesList[liveCount].gameObject.SetActive(true);
            liveCount++;
        }
    }

    public void LiveGenrator()
    {
        GameObject liveClone = Instantiate(snowLivePrefab, snowLivesHolder);
    }

    public int countDown = 4;
    public void CountDownOns()
    {
        countDown--;
        countDownTxt.text = countDown.ToString();
        if (countDown == 0)
        {
            CancelInvoke(nameof(CountDownOns));
            Time.timeScale = 0;
            AudioListener.pause = true;
            tapToPlaybtn.gameObject.SetActive(true);
            countDownTxt.gameObject.SetActive(false);
        }
    }

    public void Tap()
    {
        if (particalSystem != null)
            particalSystem.Play();

        Time.timeScale = 1;
        AudioListener.pause = false;
        tapToPlaybtn.gameObject.SetActive(false);

        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);

        if (SoundManager.instance.Sound)
        {
            SoundManager.instance.startAudioSource.Stop();
            SoundManager.instance.bgAudioSource.mute = false;
            SoundManager.instance.bgAudioSource.Play();
        }

        // Reset snow tree speeds
        for (int i = 0; i < snowPrefabs.Count; i++)
        {
            if (snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>() != null)
                snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 10;
        }

        uiCollections.SetActive(true);
        pausebtnImage.gameObject.SetActive(true);
        player.SetActive(true);
        InvokeRepeating(nameof(BuildGenrator), 0f, 1.5f);
        InvokeRepeating(nameof(LiveGenrator), 17f, 15f);
        InvokeRepeating(nameof(Ball1), 5f, 10f);
        InvokeRepeating(nameof(Ball2), 10f, 20f);
        InvokeRepeating(nameof(Ball3), 15f, 18f);
    }

    private bool isPaused = false;
    public Sprite pausebtnSprite;
    public Sprite resumebtnSprite;
    public Image pausebtnImage;

    public void PauseBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            pausebtnImage.sprite = resumebtnSprite;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            pausebtnImage.sprite = pausebtnSprite;
        }
    }

    // Method to trigger game over and switch to GameManager's game over screen
    public void TriggerGameOver()
    {
        // Save high score if needed
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighestSnow", score);
            PlayerPrefs.Save();
        }

        // Stop all snow mode activities
        CancelInvoke();
        Time.timeScale = 1;
        AudioListener.pause = false;

        // Stop particle system
        if (particalSystem != null)
            particalSystem.Stop();

        // Hide snow mode screens
        gameplayScreen.SetActive(false);
        uiCollections.SetActive(false);

        // Show main game over screen via GameManager
        GameManager.instance.gameOverScreen.SetActive(true);
    }
}