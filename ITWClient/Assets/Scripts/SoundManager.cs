using UnityEngine;
using System.Collections;

public static class SoundManager
{

    public static float MasterVolume
    {
        get
        {
            return _masterVolume;
        }
        set
        {
            _masterVolume = value;
            
            if(OnMasterVolumeChanged != null)
                OnMasterVolumeChanged(_masterVolume);
            if(OnBgmVolumeChanged != null)
                OnBgmVolumeChanged(_masterVolume * _bgmVolume);
            if(OnSfxVolumeChanged != null)
                OnSfxVolumeChanged(_masterVolume * _sfxVolume);
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
            if(OnBgmVolumeChanged != null)
                OnBgmVolumeChanged(_masterVolume * _bgmVolume);
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
            if(OnSfxVolumeChanged != null)
                OnSfxVolumeChanged(_masterVolume * _sfxVolume);
        }
    }
    private static float _sfxVolume;

    #region Event
    public static event System.Action<float> OnMasterVolumeChanged;
    public static event System.Action<float> OnBgmVolumeChanged;
    public static event System.Action<float> OnSfxVolumeChanged;
    #endregion


}