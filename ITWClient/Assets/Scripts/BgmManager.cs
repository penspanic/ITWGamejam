using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BgmType
{
    Main,
    Select,
    InGame1,
    InGame2,
    GameOver,
}

public class BgmManager : Singleton<BgmManager>
{
    public bool Initialized { get; private set; }
    private AudioSource bgmSource;
    private Dictionary<BgmType, AudioClip> bgmClips = new Dictionary<BgmType, AudioClip>();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        SoundManager.OnBgmVolumeChanged += OnBgmVolumeChanged;
        SoundManager.MasterVolume = 1; // 임시
        SoundManager.BgmVolume = 0; // 임시
    }

    public void LoadClips()
    {
        bgmClips.Add(BgmType.Main,      Resources.Load<AudioClip>("Sounds/Bgm/BGM_MainMenu"));
        bgmClips.Add(BgmType.Select,    Resources.Load<AudioClip>("Sounds/Bgm/BGM_Select_1"));
        bgmClips.Add(BgmType.InGame1,   Resources.Load<AudioClip>("Sounds/Bgm/BGM_InGame_1"));
        bgmClips.Add(BgmType.InGame2,   Resources.Load<AudioClip>("Sounds/Bgm/BGM_InGame_2"));
        bgmClips.Add(BgmType.GameOver,  Resources.Load<AudioClip>("Sounds/Bgm/BGM_GameOver"));
        Initialized = true;
    }

    private void OnBgmVolumeChanged(float value)
    {
        bgmSource.volume = value;
    }

    public void Play(BgmType type, bool loop = true)
    {
        bgmSource.Stop();
        bgmSource.clip = bgmClips[type];
        bgmSource.Play();
    }
}