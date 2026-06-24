using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip pauseMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip fruitSliceSound;
    [SerializeField] private AudioClip bladeSwingSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip smallBombSound;
    [SerializeField] private AudioClip buttonClickSound;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameOverMusic()
    {
        if (gameOverMusic != null)
        {
            PlayMusic(gameOverMusic);
        }
        else
        {
            PlayMusic(menuMusic);
        }
    }

    public void PlayPauseMusic()
    {
        if (pauseMusic != null)
        {
            PlayMusic(pauseMusic);
        }
        else
        {
            PlayMusic(menuMusic);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayFruitSlice()
    {
        PlaySound(fruitSliceSound);
    }

    public void PlayBladeSwing()
    {
        PlaySound(bladeSwingSound);
    }

    public void PlayBomb()
    {
        PlaySound(bombSound);
    }

    public void PlaySmallBomb()
    {
        PlaySound(smallBombSound);
    }

    public void PlayButtonClick()
    {
        PlaySound(buttonClickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}