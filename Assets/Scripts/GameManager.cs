using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject startScreen, gamePlayScreen, gameOverScreen, modeScreen,GrassModeScreen,SnowModeScreen,CityModeScreen;
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

    // Main restart function called from Game Over screen
    public void RestartCurrentMode()
    {
        // Reset countdown first
        countDown = 4;
        
        // Cancel all invokes and reset time
        CancelInvoke();
        Time.timeScale = 1;
        AudioListener.pause = false;

        // Hide game over screen
        gameOverScreen.SetActive(false);

        // Clear all obstacles from all modes
        ClearAllObstacles();

        // Reset based on current mode
        switch (currentMode)
        {
            case "Grass":
                RestartGrassMode();
                break;
            case "City":
                RestartCityMode();
                break;
            case "Snow":
                RestartSnowMode();
                break;
            default:
                // If no mode is set, go back to mode selection
                GoToModeSelection();
                break;
        }
    }

    private void ClearAllObstacles()
    {
        // Clear grass mode obstacles
        foreach (Transform child in bgHolder) 
            if (child != null) Destroy(child.gameObject);

        // Clear city mode obstacles
        if (CityManager.instance != null && CityManager.instance.buildingsHolder != null)
        {
            foreach (Transform child in CityManager.instance.buildingsHolder) 
                if (child != null) Destroy(child.gameObject);
            
            foreach (Transform child in CityManager.instance.birdHolder) 
                if (child != null) Destroy(child.gameObject);
        }

        // Clear snow mode obstacles
        if (SnowManager.instance != null && SnowManager.instance.snowTreeHolder != null)
        {
            foreach (Transform child in SnowManager.instance.snowTreeHolder) 
                if (child != null) Destroy(child.gameObject);
            
            foreach (Transform child in SnowManager.instance.snowBallHolder) 
                if (child != null) Destroy(child.gameObject);
        }
    }

    private void RestartGrassMode()
    {
        // Hide all other mode screens
        CityModeScreen.SetActive(false);
        SnowModeScreen.SetActive(false);
        CityManager.instance.gameplayScreen.SetActive(false);
        SnowManager.instance.gameplayScreen.SetActive(false);

        // Cancel other managers' invokes
        if (CityManager.instance != null) CityManager.instance.CancelInvoke();
        if (SnowManager.instance != null) SnowManager.instance.CancelInvoke();

        // Reset grass mode values
        score = 0;
        scoreTxt.text = "0";
        liveCount = 0;

        // Reset high score display
        highScore = PlayerPrefs.GetInt("HighrstScore");
        highestScoreTxt.text = "Highest  " + highScore.ToString();

        // Reset lives
        foreach (var life in livesList)
            life.gameObject.SetActive(true);

        // Reset enemy speeds
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyPrefabs[i].treeSpeed = 10f;
        }

        // Show grass mode UI
        GrassModeScreen.SetActive(true);
        gamePlayScreen.SetActive(true);
        enviornment.SetActive(true);
        bgHolder.gameObject.SetActive(true);

        // Hide pause button initially (will show after countdown)
        pausebtnImage.gameObject.SetActive(false);

        // Stop audio and restart countdown
        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Stop();

        // Start countdown
        countDownTxt.gameObject.SetActive(true);
        countDownTxt.text = countDown.ToString();
        InvokeRepeating(nameof(CountDownOn), 0f, 1f);
    }

    private void RestartCityMode()
    {
        // Hide all other mode screens
        GrassModeScreen.SetActive(false);
        SnowModeScreen.SetActive(false);
        gamePlayScreen.SetActive(false);
        SnowManager.instance.gameplayScreen.SetActive(false);

        // Cancel other managers' invokes
        CancelInvoke();
        if (SnowManager.instance != null) SnowManager.instance.CancelInvoke();

        // Reset city mode
        CityManager.instance.ResetCityMode();

        // Show city mode UI
        CityModeScreen.SetActive(true);
        CityManager.instance.gameplayScreen.SetActive(true);

        // Stop audio
        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Stop();

        // Start city countdown
        CityManager.instance.countDown = 4;
        CityManager.instance.countDownTxt.gameObject.SetActive(true);
        CityManager.instance.countDownTxt.text = CityManager.instance.countDown.ToString();
        CityManager.instance.InvokeRepeating(nameof(CityManager.CountDownOns), 0f, 1f);
    }

    private void RestartSnowMode()
    {
        // Hide all other mode screens
        GrassModeScreen.SetActive(false);
        CityModeScreen.SetActive(false);
        gamePlayScreen.SetActive(false);
        CityManager.instance.gameplayScreen.SetActive(false);

        // Cancel other managers' invokes
        CancelInvoke();
        if (CityManager.instance != null) CityManager.instance.CancelInvoke();

        // Reset snow mode
        SnowManager.instance.ResetSnowMode();

        // Show snow mode UI
        SnowModeScreen.SetActive(true);
        SnowManager.instance.gameplayScreen.SetActive(true);

        // Stop audio
        SoundManager.instance.startAudioSource.Stop();
        SoundManager.instance.bgAudioSource.Stop();

        // Start snow countdown
        SnowManager.instance.countDown = 4;
        SnowManager.instance.countDownTxt.gameObject.SetActive(true);
        SnowManager.instance.countDownTxt.text = SnowManager.instance.countDown.ToString();
        SnowManager.instance.InvokeRepeating(nameof(SnowManager.CountDownOns), 0f, 1f);
    }

    private void GoToModeSelection()
    {
        // Hide all screens
        gamePlayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        GrassModeScreen.SetActive(false);
        SnowModeScreen.SetActive(false);
        CityModeScreen.SetActive(false);
        
        if (CityManager.instance != null) CityManager.instance.gameplayScreen.SetActive(false);
        if (SnowManager.instance != null) SnowManager.instance.gameplayScreen.SetActive(false);

        // Show mode selection
        modeScreen.SetActive(true);
        
        // Reset current mode
        currentMode = "";
    }

    public void GoToHome()
    {
        CancelInvoke();
        if (CityManager.instance != null) CityManager.instance.CancelInvoke();
        if (SnowManager.instance != null) SnowManager.instance.CancelInvoke();
        
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CharacterSelected(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }

    // Public method to get current mode (useful for other scripts)
    public string GetCurrentMode()
    {
        return currentMode;
    }

    // Method to set current mode (can be called by other managers)
    public void SetCurrentMode(string mode)
    {
        currentMode = mode;
    }
}