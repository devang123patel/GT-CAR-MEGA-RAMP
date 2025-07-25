using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    public static CityManager instance;

    public GameObject gameplayScreen, gameOverScreen;

    public GameObject player;
    public GameObject cityHolder;
    public GameObject uiCollections;

    public Transform livesHolder;
    public GameObject livePrefab;

    public Transform buildingsHolder;
    public List<GameObject> prefabs;

    public Transform birdHolder;
    public GameObject bird1;
    public GameObject bird2;

    public int score;
    public Text scoreTxt;

    public int highScore;
    public Text highestScoreTxt;

    public int liveCount = 5;
    public List<Image> livesList;

    public Text countDownTxt;

    public Button tapToPlaybtn;

    CityBuildingsHandlers cityBuildingsHandlers;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Play();

        highScore = PlayerPrefs.GetInt("HighestCity");
        highestScoreTxt.text = ("Highest  " + highScore.ToString());
    }

    public void ResetCityMode()
    {
        score = 0;
        scoreTxt.text = "Score: 0";
        highScore = PlayerPrefs.GetInt("HighestCity");
        highestScoreTxt.text = "Highest  " + highScore.ToString();

        foreach (Image life in livesList)
            life.gameObject.SetActive(true);

        liveCount = 0;
        
        // Reset building speeds
        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i].GetComponentInChildren<CityBuildingsHandlers>().towerSpeed = 10;
        }

        player.SetActive(true);
        pausebtnImage.gameObject.SetActive(true);
        uiCollections.SetActive(true);
        CancelInvoke();
    }

    public void BuildGenrator()
    {
        int randomCity = Random.Range(0, prefabs.Count);
        Instantiate(prefabs[randomCity], buildingsHolder);

        if (score >= 50)
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                prefabs[i].GetComponentInChildren<CityBuildingsHandlers>().towerSpeed = 13;
            }
        }

        if (score >= 100)
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                prefabs[i].GetComponentInChildren<CityBuildingsHandlers>().towerSpeed = 15;
            }
        }
    }

    public void Bird1() => Instantiate(bird1, birdHolder);
    public void Bird2() => Instantiate(bird2, birdHolder);

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
        Instantiate(livePrefab, livesHolder);
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

        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i].GetComponentInChildren<CityBuildingsHandlers>().towerSpeed = 10;
        }

        uiCollections.SetActive(true);
        pausebtnImage.gameObject.SetActive(true);
        player.SetActive(true);
        InvokeRepeating(nameof(BuildGenrator), 0f, 1.3f);
        InvokeRepeating(nameof(LiveGenrator), 17f, 15f);
        InvokeRepeating(nameof(Bird1), 5f, 20f);
        InvokeRepeating(nameof(Bird2), 10f, 25f);
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

    // This method can be called from the game over screen's restart button for City mode
    public void RestartCityMode()
    {
        GameManager.instance.RestartCurrentMode();
    }
}