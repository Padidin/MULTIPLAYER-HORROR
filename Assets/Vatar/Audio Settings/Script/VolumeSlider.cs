using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType { BGM, SFX }
    public VolumeType type;

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        if (type == VolumeType.BGM)
        {
            float saved = PlayerPrefs.GetFloat("BGMVolume", 1f);
            slider.value = saved;
            slider.onValueChanged.AddListener((value) => AudioManager.Instance.SetBGMVolume(value));
        }
        else
        {
            float saved = PlayerPrefs.GetFloat("SFXVolume", 1f);
            slider.value = saved;
            slider.onValueChanged.AddListener((value) => AudioManager.Instance.SetSFXVolume(value));
        }
    }


}
