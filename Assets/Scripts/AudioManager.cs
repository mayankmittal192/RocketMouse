using UnityEngine;


public class AudioManager : MonoBehaviour
{

    public enum AudioType
    {
        Background, Jetpack, Footsteps, CoinCollect, LaserZap
    }


    private AudioSource laserZap;
    private AudioSource coinCollect;
    private AudioSource footsteps;
    private AudioSource jetpack;
    private AudioSource backgroundMusic;

    public AudioClip laserZapClip;
    public AudioClip coinCollectClip;
    public AudioClip footstepsClip;
    public AudioClip jetpackClip;
    public AudioClip backgroundMusicClip;


    void Start()
    {
        SetupAudioSource(AudioType.Background);
        SetupAudioSource(AudioType.Jetpack);
        SetupAudioSource(AudioType.Footsteps);
        SetupAudioSource(AudioType.CoinCollect);
        SetupAudioSource(AudioType.LaserZap);
    }

    
    void SetupAudioSource(AudioType type)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.volume = 1;
        source.loop = true;

        switch (type)
        {
            case AudioType.Background:
                source.clip = backgroundMusicClip;
                backgroundMusic = source;
                break;

            case AudioType.Jetpack:
                source.clip = jetpackClip;
                jetpack = source;
                break;

            case AudioType.Footsteps:
                source.clip = footstepsClip;
                footsteps = source;
                break;

            case AudioType.CoinCollect:
                source.clip = coinCollectClip;
                source.loop = false;
                coinCollect = source;
                break;

            case AudioType.LaserZap:
                source.clip = laserZapClip;
                source.loop = false;
                laserZap = source;
                break;
        }
    }


    public void StartAudio(AudioType type)
    {
        EnsureExistence(type);

        switch (type)
        {
            case AudioType.Background:
                backgroundMusic.PlayDelayed(0.6f);
                break;

            case AudioType.Jetpack:
                jetpack.Play();
                break;

            case AudioType.Footsteps:
                footsteps.Play();
                break;

            case AudioType.CoinCollect:
                coinCollect.Play();
                break;

            case AudioType.LaserZap:
                laserZap.Play();
                break;
        }
    }


    public void StopAudio(AudioType type)
    {
        EnsureExistence(type);

        switch (type)
        {
            case AudioType.Background:
                backgroundMusic.Stop();
                break;

            case AudioType.Jetpack:
                jetpack.Stop();
                break;

            case AudioType.Footsteps:
                footsteps.Stop();
                break;

            case AudioType.CoinCollect:
                coinCollect.Stop();
                break;

            case AudioType.LaserZap:
                laserZap.Stop();
                break;
        }
    }


    public void EnsureExistence(AudioType type)
    {
        AudioSource source = null;

        switch (type)
        {
            case AudioType.Background:
                source = backgroundMusic;
                break;

            case AudioType.Jetpack:
                source = jetpack;
                break;

            case AudioType.Footsteps:
                source = footsteps;
                break;

            case AudioType.CoinCollect:
                source = coinCollect;
                break;

            case AudioType.LaserZap:
                source = laserZap;
                break;
        }

        if (source == null)
        {
            SetupAudioSource(type);
        }
    }

}
