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
            OnMasterVolumeChanged(_masterVolume);
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
            OnBgmVolumeChanged(_bgmVolume);
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
            OnSfxVolumeChanged(_sfxVolume);
        }
    }
    private static float _sfxVolume;

    #region Event
    public static event System.Action<float> OnMasterVolumeChanged;
    public static event System.Action<float> OnBgmVolumeChanged;
    public static event System.Action<float> OnSfxVolumeChanged;
    #endregion


}