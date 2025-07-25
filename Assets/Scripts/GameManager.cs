using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject startScreen, gamePlayScreen, gameOverScreen, modeScreen, GrassModeScreen, SnowModeScreen, CityModeScreen;
    public GameObject settingPopup;

    public GameObject CityUiCollection;
    public GameObject GrassUiCollection;
    public GameObject SnowUiCollection;

    public GameObject enviornment;

    public GameObject hintPopup;
    public GameObject sliderPopup;

    public Transform livesHolder;
    public GameObject livePrefab;

    public Transform bgHolder;
    public List<EnemyHandlers> enemyPrefabs;

    public int score;
    public Text scoreTxt;

    public int highScore;
    public Text highestScoreTxt;

    public int liveCount;
    public List<Image> livesList;

    public Text countDownTxt;

    public Button tapToPlaybtn;

    private string currentMode = "";

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        highScore = PlayerPrefs.GetInt("HighrstScore");
        highestScoreTxt.text = ("Highest  " + highScore.ToString());

        SoundManager.instance.startAudioSource.Play();
        SoundManager.instance.bgAudioSource.Stop();

        if (startScreen != null)
        {
            startScreen.SetActive(true);
        }
    }

    public void TreeGenerator()
    {
        int randomTree = Random.Range(0, enemyPrefabs.Count);
        EnemyHandlers treeClone = Instantiate(enemyPrefabs[randomTree], bgHolder);

        if (score >= 50)
        {
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                enemyPrefabs[i].treeSpeed = 12f;
            }
        }

        if (score >= 100)
        {
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                enemyPrefabs[i].treeSpeed = 15f;
            }
        }
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
        GameObject liveClone = Instantiate(livePrefab, livesHolder);
    }

    int countDown = 4;
    public void CountDownOn()
    {
        countDown--;
        countDownTxt.text = countDown.ToString();
        if (countDown == 0)
        {
            CancelInvoke(nameof(CountDownOn));
            Time.timeScale = 0;
            AudioListener.pause = true;
            tapToPlaybtn.gameObject.SetActive(true);
            countDownTxt.gameObject.SetActive(false);
        }
    }

    public void ReloadScene()
    {
        RestartCurrentMode();
    }

    public void PlayBtn()
    {
        startScreen.SetActive(false);
        modeScreen.SetActive(true);
    }

    public void GrassModeBtn()
    {
        currentMode = "Grass";
        AdsManager.instance.DestroyAd();
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        modeScreen.SetActive(false);
        gamePlayScreen.SetActive(true);

        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Stop();
        InvokeRepeating(nameof(CountDownOn), 0f, 1f);
    }

    public void CityModeBtn()
    {
        currentMode = "City";
        AdsManager.instance.DestroyAd();
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        modeScreen.SetActive(false);
        CityManager.instance.gameplayScreen.SetActive(true);

        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Stop();
        CityManager.instance.InvokeRepeating(nameof(CityManager.CountDownOns), 0f, 1f);
    }

    public void SnowModeBtn()
    {
        currentMode = "Snow";
        AdsManager.instance.DestroyAd();
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        modeScreen.SetActive(false);
        SnowManager.instance.gameplayScreen.SetActive(true);

        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Stop();
        SnowManager.instance.InvokeRepeating(nameof(SnowManager.CountDownOns), 0f, 1f);
    }

    public void BackModeBtn()
    {
        modeScreen.SetActive(false);
        startScreen.SetActive(true);
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
    }

    public void TapStartBtn()
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

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyPrefabs[i].treeSpeed = 10f;
        }

        InvokeRepeating(nameof(TreeGenerator), 0f, 1f);
        bgHolder.gameObject.SetActive(true);
        pausebtnImage.gameObject.SetActive(true);
        enviornment.gameObject.SetActive(true);
        InvokeRepeating(nameof(LiveGenrator), 20f, 20f);
    }

    public void HintBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        hintPopup.SetActive(true);
    }

    public void HintCloseBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        hintPopup.SetActive(false);
    }

    public void SettingBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
    }

    public void SettingsPopupCloseBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
    }

    public void SoundSliderBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        sliderPopup.SetActive(true);
    }

    public void SoundSliderPopupCloseBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        sliderPopup.SetActive(false);
    }

    public void ExitBtn()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        Application.Quit();
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

    // This is the main restart function that restarts the current mode
    public void RestartCurrentMode()
    {
        CancelInvoke();
        Time.timeScale = 1;
        AudioListener.pause = false;

        // Hide all screens first
        gameOverScreen.SetActive(false);
        GrassModeScreen.SetActive(false);
        SnowModeScreen.SetActive(false);
        CityModeScreen.SetActive(false);

        gamePlayScreen.SetActive(false);
        if (CityManager.instance != null)
            CityManager.instance.gameplayScreen.SetActive(false);
        if (SnowManager.instance != null)
            SnowManager.instance.gameplayScreen.SetActive(false);

        // Clear all previous obstacles
        foreach (Transform child in bgHolder) Destroy(child.gameObject);
        if (CityManager.instance != null)
            foreach (Transform child in CityManager.instance.buildingsHolder) Destroy(child.gameObject);
        if (SnowManager.instance != null)
            foreach (Transform child in SnowManager.instance.snowTreeHolder) Destroy(child.gameObject);

        // Reset count down
        countDown = 4;
        if (CityManager.instance != null)
            CityManager.instance.countDown = 4;
        if (SnowManager.instance != null)
            SnowManager.instance.countDown = 4;

        // Reset based on current mode
        if (currentMode == "Grass")
        {
            score = 0;
            scoreTxt.text = "0";

            highScore = PlayerPrefs.GetInt("HighrstScore");
            highestScoreTxt.text = "Highest  " + highScore.ToString();

            foreach (var life in livesList)
                life.gameObject.SetActive(true);
            liveCount = 0;

            enviornment.SetActive(true);
            bgHolder.gameObject.SetActive(true);
            pausebtnImage.gameObject.SetActive(true);

            GrassModeScreen.SetActive(true);
            gamePlayScreen.SetActive(true);

            SoundManager.instance.startAudioSource.Stop();
            SoundManager.instance.bgAudioSource.Stop();

            InvokeRepeating(nameof(CountDownOn), 0f, 1f);
        }
        else if (currentMode == "City")
        {
            if (CityManager.instance != null)
            {
                CityManager.instance.ResetCityMode();

                CityModeScreen.SetActive(true);
                CityManager.instance.gameplayScreen.SetActive(true);

                SoundManager.instance.startAudioSource.Stop();
                SoundManager.instance.bgAudioSource.Stop();

                CityManager.instance.InvokeRepeating(nameof(CityManager.CountDownOns), 0f, 1f);
            }
        }
        else if (currentMode == "Snow")
        {
            if (SnowManager.instance != null)
            {
                SnowManager.instance.ResetSnowMode();

                SnowModeScreen.SetActive(true);
                SnowManager.instance.gameplayScreen.SetActive(true);

                SoundManager.instance.startAudioSource.Stop();
                SoundManager.instance.bgAudioSource.Stop();

                SnowManager.instance.InvokeRepeating(nameof(SnowManager.CountDownOns), 0f, 1f);
            }
        }
    }

    // This method should be called from the Game Over screen's restart button
    public void RestartButtonPressed()
    {
        SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        RestartCurrentMode();
    }

    public void GoToHome()
    {
        CancelInvoke();
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CharacterSelected(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }
}