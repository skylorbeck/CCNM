using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [field:SerializeField] public Slider musicSlider { get; private set; }
    [field:SerializeField] public Slider effectSlider { get; private set; }
    [field:SerializeField] public Slider touchSensitivitySlider { get; private set; }
    [field:SerializeField] public Toggle leftToggle { get; private set; }
    [field:SerializeField] public Toggle stickyMenuToggle { get; private set; }
    private async void Start()
    {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1)*10);
        effectSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("EffectVolume", 1)*10);
        leftToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("LeftHanded", 0) == 1);
        stickyMenuToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("StickyMenu", 0) == 1);
        touchSensitivitySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("TouchSensitivity", 1)*10);
        await Task.Delay(10);
        
        GameManager.Instance.inputReader.Back+=Back;
        // GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

    }

    public void UpdateSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value*0.1f);
        PlayerPrefs.SetFloat("EffectVolume", effectSlider.value*0.1f);
        PlayerPrefs.SetInt("LeftHanded", leftToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("TouchSensitivity", touchSensitivitySlider.value*0.1f);
        PlayerPrefs.SetInt("StickyMenu", stickyMenuToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        MusicManager.Instance.UpdateVolume();
        SoundManager.Instance.PlayUiClick();
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu","Settings");
    }

    public void NukeSave()
    {
        GameManager.Instance.saveManager.DeleteSave();
    }
}
