using UnityEngine;
using System.Collections;

public enum SoundConfigType
{
    MasterVolume,
    BgmVolume,
    SfxVolume,
}

public class SoundManager : Singleton<SoundManager>
{
    /* 볼륨 크기
     * 
     * UI           = 1
     * Object       = 0.85
     * BGM(!InGame) = 0.8
     * Character    = 0.75 
     * BGM(InGame)  = 0.6
     * 
     * 적용 후 변동사항 생기면 여기에 적어둘께. -요한
     * 
     */

    public float MasterVolume
    {
        get
        {
            return _masterVolume;
        }
        set
        {
            _masterVolume = value;

            OnMasterVolumeChanged?.Invoke(_masterVolume);
            OnBgmVolumeChanged?.Invoke(_masterVolume * _bgmVolume);
            OnSfxVolumeChanged?.Invoke(_masterVolume * _sfxVolume);
        }
    }
    private float _masterVolume;

    public float BgmVolume
    {
        get
        {
            return _bgmVolume;
        }
        set
        {
            _bgmVolume = value;
            OnBgmVolumeChanged?.Invoke(_masterVolume * _bgmVolume);
        }
    }
    private float _bgmVolume;

    public float SfxVolume
    {
        get
        {
            return _sfxVolume;
        }
        set
        {
            _sfxVolume = value;
            OnSfxVolumeChanged?.Invoke(_masterVolume * _sfxVolume);
        }
    }
    private float _sfxVolume;

    #region Event
    public event System.Action<float> OnMasterVolumeChanged;
    public event System.Action<float> OnBgmVolumeChanged;
    public event System.Action<float> OnSfxVolumeChanged;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        MasterVolume = 1f;
        BgmVolume = 1f;
        SfxVolume = 1f;

        if(PlayerPrefs.HasKey(nameof(SoundConfigType.MasterVolume)) == true)
        {
            MasterVolume = PlayerPrefs.GetFloat(nameof(SoundConfigType.MasterVolume));
        }
        if(PlayerPrefs.HasKey(nameof(SoundConfigType.BgmVolume)) == true)
        {
            BgmVolume = PlayerPrefs.GetFloat(nameof(SoundConfigType.BgmVolume));
        }
        if(PlayerPrefs.HasKey(nameof(SoundConfigType.SfxVolume)) == true)
        {
            SfxVolume = PlayerPrefs.GetFloat(nameof(SoundConfigType.SfxVolume));
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(nameof(SoundConfigType.MasterVolume), MasterVolume);
        PlayerPrefs.SetFloat(nameof(SoundConfigType.BgmVolume), BgmVolume);
        PlayerPrefs.SetFloat(nameof(SoundConfigType.SfxVolume), SfxVolume);
        PlayerPrefs.Save();
    }
}