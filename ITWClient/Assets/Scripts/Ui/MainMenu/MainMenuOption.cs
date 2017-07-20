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

        masterVolumeSlider.value = SoundManager.Instance.MasterVolume;
        bgmVolumeSlider.value = SoundManager.Instance.BgmVolume;
        sfxVolumeSlider.value = SoundManager.Instance.SfxVolume;
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
        SoundManager.Instance.MasterVolume = masterVolumeSlider.value;
    }

    public void OnBgmVolumeSliderChanged()
    {
        SoundManager.Instance.BgmVolume = bgmVolumeSlider.value;
    }

    public void OnSfxVolumeSliderChanged()
    {
        SoundManager.Instance.SfxVolume = sfxVolumeSlider.value;
    }
}
