using UnityEngine;
using System.Collections;

public static class SoundManager
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
    public static float MasterVolume
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
    private static float _masterVolume;

    public static float BgmVolume
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
    private static float _bgmVolume;

    public static float SfxVolume
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
    private static float _sfxVolume;

    static SoundManager()
    {
        MasterVolume = 1f;
        BgmVolume = 1f;
        SfxVolume = 1f;
    }

    #region Event
    public static event System.Action<float> OnMasterVolumeChanged;
    public static event System.Action<float> OnBgmVolumeChanged;
    public static event System.Action<float> OnSfxVolumeChanged;
    #endregion


}