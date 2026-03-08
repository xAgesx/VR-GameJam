using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    public enum SliderType { SFX, BGM }
    public SliderType type;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        RegisterWithPlayerSettings();
    }
    private void RegisterWithPlayerSettings()
    {
        if (slider == null) slider = GetComponent<Slider>();
        if (PlayerSettings.Instance == null) return;

        PlayerSettings.Instance.RegisterSlider(slider, type);
    }
}
