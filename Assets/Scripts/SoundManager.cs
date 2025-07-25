using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioSource startAudioSource;
    public AudioSource bgAudioSource;
    
    public AudioClip buttonClick;
    
    public bool Sound = true;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ButtonPress(AudioClip clip)
    {
        if (Sound && clip != null)
        {
            // Play button sound effect
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}