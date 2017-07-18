using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuOption : MonoBehaviour
{

    public bool IsShowing { get; private set; }
    [SerializeField]
    Slider masterVolumeSlider;
    [SerializeField]
    Slider bgmVolumeSlider;
    [SerializeField]
    Slider sfxVolumeSlider;

    public void Show()
    {
        if(IsShowing == true)
        {
            return;
        }

        IsShowing = true;
        this.gameObject.SetActive(true);

        masterVolumeSlider.value = SoundManager.MasterVolume;
        bgmVolumeSlider.value = SoundManager.BgmVolume;
        sfxVolumeSlider.value = SoundManager.SfxVolume;
    }
    
    public void Hide()
    {
        if(IsShowing == false)
        {
            return;
        }

        IsShowing = false;
        this.gameObject.SetActive(false);
    }

    public void OnOkButtonDown()
    {
        Hide();
    }

    public void OnMasterVolumeSliderChanged()
    {
        SoundManager.MasterVolume = masterVolumeSlider.value;
    }

    public void OnBgmVolumeSliderChanged()
    {
        SoundManager.BgmVolume = bgmVolumeSlider.value;
    }

    public void OnSfxVolumeSliderChanged()
    {
        SoundManager.SfxVolume = sfxVolumeSlider.value;
    }
}
