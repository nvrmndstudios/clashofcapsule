using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip clickSFX;
    public AudioClip bgMusic;
    public AudioClip gameSuccessMusic;
    public AudioClip gameFailMusic;

    private bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(bgMusic);
    }

    // --- Public Methods ---

    public void PlayClick()
    {
        PlaySFX(clickSFX);
    }

    public void PlayGameSuccess()
    {
        PlayMusic(gameSuccessMusic);
    }

    public void PlayGameFail()
    {
        PlayMusic(gameFailMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetVolume(float musicVol, float sfxVol)
    {
        if(musicSource)
            musicSource.volume = musicVol;
        if(sfxSource)
            sfxSource.volume = sfxVol;
    }

    public bool ToggleSound()
    {
        isMuted = !isMuted;
        if(musicSource)
            musicSource.mute = isMuted;
        if(sfxSource)
            sfxSource.mute = isMuted;
        return isMuted;
    }
}