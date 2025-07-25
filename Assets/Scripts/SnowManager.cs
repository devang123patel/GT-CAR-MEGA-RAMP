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

    public void BuildGenrator()
    {
        int randomCity = Random.Range(0, snowPrefabs.Count);
        GameObject clone = Instantiate(snowPrefabs[randomCity], snowTreeHolder);

        if (score >= 50)
        {
            Debug.Log("10 Pass");
            for (int i = 0; i < snowPrefabs.Count; i++)
            {
                snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 13;
            }
        }

        if (score >= 100)
        {
            Debug.Log("20 Pass");
            for (int i = 0; i < snowPrefabs.Count; i++)
            {
                snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 15;
            }
        }
    }

    public void Ball1()
    {
        GameObject bird1clone = Instantiate(ball1, snowBallHolder);
    }
    public void Ball2()
    {
        GameObject bird1clone = Instantiate(ball2, snowBallHolder);
    }
    public void Ball3()
    {
        GameObject bird1clone = Instantiate(ball3, snowBallHolder);
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

    public void ResetSnowMode()
    {
        score = 0;
        liveCount = 0;
        scoreTxt.text = "Score: 0";

        // Reset snow tree speeds
        for (int i = 0; i < snowPrefabs.Count; i++)
        {
            snowPrefabs[i].GetComponentInChildren<SnowTreeHandlers>().snowTreeSpeed = 10;
        }

        foreach (Image life in livesList)
        {
            life.gameObject.SetActive(true);
        }

        CancelInvoke();

        // Stop particle system if it's playing
        if (particalSystem != null)
            particalSystem.Stop();

        player.SetActive(true);
        pausebtnImage.gameObject.SetActive(true);
        uiCollections.SetActive(true);
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

        for (int i = 0; i < snowPrefabs.Count; i++)
        {
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

    // This method can be called from the game over screen's restart button for Snow mode
    public void RestartSnowMode()
    {
        GameManager.instance.RestartCurrentMode();
    }
}