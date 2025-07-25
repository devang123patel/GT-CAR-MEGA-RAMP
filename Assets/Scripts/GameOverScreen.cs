using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [Header("UI References")]
    public Button restartButton;
    public Button homeButton;
    public Text finalScoreText;
    public Text highScoreText;

    private void Start()
    {
        // Ensure buttons are connected to proper methods
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (homeButton != null)
            homeButton.onClick.AddListener(GoHome);
    }

    private void OnEnable()
    {
        // Update score display when game over screen becomes active
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        string currentMode = GameManager.instance.GetCurrentMode();
        
        switch (currentMode)
        {
            case "Grass":
                if (finalScoreText != null)
                    finalScoreText.text = "Score: " + GameManager.instance.score.ToString();
                if (highScoreText != null)
                    highScoreText.text = "Highest: " + PlayerPrefs.GetInt("HighrstScore").ToString();
                break;
                
            case "City":
                if (finalScoreText != null)
                    finalScoreText.text = "Score: " + CityManager.instance.score.ToString();
                if (highScoreText != null)
                    highScoreText.text = "Highest: " + PlayerPrefs.GetInt("HighestCity").ToString();
                break;
                
            case "Snow":
                if (finalScoreText != null)
                    finalScoreText.text = "Score: " + SnowManager.instance.score.ToString();
                if (highScoreText != null)
                    highScoreText.text = "Highest: " + PlayerPrefs.GetInt("HighestSnow").ToString();
                break;
        }
    }

    public void RestartGame()
    {
        // Play button click sound
        if (SoundManager.instance != null)
            SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        
        // Call the improved restart function from GameManager
        GameManager.instance.RestartCurrentMode();
    }

    public void GoHome()
    {
        // Play button click sound
        if (SoundManager.instance != null)
            SoundManager.instance.ButtonPress(SoundManager.instance.buttonClick);
        
        // Go to home screen
        GameManager.instance.GoToHome();
    }
}