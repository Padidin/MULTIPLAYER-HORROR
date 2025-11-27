using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] bgmSounds;
    public Sound[] sfxSounds;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private static bool hasBGMStarted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // Load saved volume
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
        else
        {
            Destroy(gameObject);
        }

        ApplyVolume();
    }

    void Start()
    {
        if (!hasBGMStarted)
        {
            PlayBGM("BGM");
            hasBGMStarted = true;
        }
    }

    public void PlayBGM(string name)
    {
        Sound s = System.Array.Find(bgmSounds, sound => sound.name == name);
        if (s != null)
        {
            bgmSource.clip = s.clip;
            bgmSource.volume = s.volume * bgmVolume;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip, s.volume * sfxVolume);
        }
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        ApplyVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        ApplyVolume();
    }

    void ApplyVolume()
    {
        if (bgmSource != null && bgmSource.clip != null)
        {
            Sound currentBGM = System.Array.Find(bgmSounds, s => s.clip == bgmSource.clip);
            if (currentBGM != null)
                bgmSource.volume = currentBGM.volume * bgmVolume;
            else
                bgmSource.volume = bgmVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
}
