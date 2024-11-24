using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPopupController : BasePopup
{
    [SerializeField] private SwitchToggle music, sound, vibrate;

    private void Start()
    {
        DataMananger.instance.LoadData();
        music.GetToggle().isOn = DataMananger.instance.gameSave.music;
        sound.GetToggle().isOn = DataMananger.instance.gameSave.sounds;
        vibrate.GetToggle().isOn = DataMananger.instance.gameSave.vibrate;

        music.GetToggle().onValueChanged.AddListener((isOn) =>
        {
            music.OnSwitch(isOn);
            DataMananger.instance.gameSave.music = isOn;
            DataMananger.instance.SaveGame();
        });
        sound.GetToggle().onValueChanged.AddListener((isOn) =>
        {
            sound.OnSwitch(isOn);
            DataMananger.instance.gameSave.sounds = isOn;
            DataMananger.instance.SaveGame();
        });
        vibrate.GetToggle().onValueChanged.AddListener((isOn) =>
        {
            vibrate.OnSwitch(isOn);
            DataMananger.instance.gameSave.vibrate = isOn;
            DataMananger.instance.SaveGame();
        });
    }
}
