using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InGameOption : MonoBehaviour
{
    public bool IsShowing { get; private set; }

    [SerializeField]
    private Slider masterVolumeSlider;
    [SerializeField]
    private GuidePanel guidePanel;

    public void Show()
    {
        Time.timeScale = 0f;
        IsShowing = true;
        masterVolumeSlider.value = SoundManager.MasterVolume;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        IsShowing = false;
        guidePanel.Hide();
        this.gameObject.SetActive(false);
    }

    public void OnMasterVolumeSliderChanged()
    {
        SoundManager.MasterVolume = masterVolumeSlider.value;
    }

    public void OnGuideButtonDown()
    {
        guidePanel.Show();
    }

    public void OnReturnButtonDown()
    {
        Hide();
    }

    public void OnMenuButtonDown()
    {
        Hide();
        SceneUtil.LoadScene("MainMenu");
    }
}