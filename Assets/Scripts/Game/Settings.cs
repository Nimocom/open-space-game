using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityStandardAssets.ImageEffects;

public class Settings : MonoBehaviour 
{
    public static Settings inst;
    public static SettingsData settingsData;
    public Dropdown resolutions;
    public Dropdown antialiasing;
    public Dropdown language;
    public Slider gameVolume;
    public Toggle imageEffects;
    public Toggle vsync;
    public Toggle windowMode;

    BlurOptimized cameraBlur;

    List<Resolution> resolutionsList;

    void Awake()
    {
        inst = this;
        cameraBlur = Camera.main.GetComponent<BlurOptimized>();
        InitializaSettings();
    }

    void InitializaSettings()
    {
        settingsData  = JsonUtility.FromJson<SettingsData>(File.ReadAllText(Application.streamingAssetsPath + "/Settings.json"));

        resolutionsList = new List<Resolution>();
        foreach (var res in Screen.resolutions)
        {
            if (!resolutionsList.Any(x => x.ToStringWH() == res.ToStringWH()))
            {
                resolutionsList.Add(res);
                resolutions.options.Add(new Dropdown.OptionData(res.ToStringWH()));
            }
        }
        
        foreach (var path in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Localization")))
            language.options.Add(new Dropdown.OptionData(Path.GetFileNameWithoutExtension(path)));

        foreach(var field in typeof(CKeys).GetFields())
            field.SetValue(typeof(CKeys), typeof(KeysSettings).GetField(field.Name).GetValue(settingsData.inputSettings.keysSettings));

        for (int i = 0; i < resolutions.options.Count; i++)
        {
            if (resolutions.options[i].text == Screen.currentResolution.ToStringWH())
            {
                resolutions.value = i;
                break;
            }
        }

        for (int i = 0; i < antialiasing.options.Count; i++)
        {
            if (antialiasing.options[i].text.Contains(settingsData.graphicSettings.antiAliasing.ToString()))
            {
                antialiasing.value = i;
                break;
            }
        }

        for(int i = 0; i < language.options.Count; i++)
        {
            if (language.options[i].text == settingsData.gameSettings.language)
            {
                language.value = i;
                break;
            }
        }

        windowMode.isOn = Screen.fullScreen;
        vsync.isOn = settingsData.graphicSettings.vsync == 1;
        imageEffects.isOn = settingsData.graphicSettings.imageEffects;
        cameraBlur.enabled = settingsData.graphicSettings.imageEffects;
        gameVolume.value = settingsData.audioSettings.gameVolume;
        LocaManager.SetLanguage(settingsData.gameSettings.language);
        AudioListener.volume = settingsData.audioSettings.gameVolume;
        QualitySettings.vSyncCount = vsync.isOn ? 1 : 0;
        QualitySettings.antiAliasing = settingsData.graphicSettings.antiAliasing;
    }

    public void ApplyGraphicSettings()
    {
        ApplyResolution();
        ApplyAntialiasing();
        ApplyVSync();
        ApplyImageEffects();
        ApplyScreenMode();
        File.WriteAllText(Application.streamingAssetsPath + "/Settings.json", JsonUtility.ToJson(settingsData));
    }

    public void ApplyGameSettings()
    {
        ApplyLanguage();
        File.WriteAllText(Application.streamingAssetsPath + "/Settings.json", JsonUtility.ToJson(settingsData));
    }

    void ApplyResolution()
    {
        Screen.SetResolution(resolutionsList[resolutions.value].width, resolutionsList[resolutions.value].height, true);
    }

    void ApplyAntialiasing()
    {
        QualitySettings.antiAliasing = int.Parse(antialiasing.captionText.text.Replace(" MSAA", ""));
        settingsData.graphicSettings.antiAliasing = QualitySettings.antiAliasing;
    }

    void ApplyVSync()
    {
        QualitySettings.vSyncCount = vsync.isOn ? 1 : 0;
        settingsData.graphicSettings.vsync = QualitySettings.vSyncCount;
    }

    void ApplyImageEffects()
    {
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = imageEffects.isOn;
        settingsData.graphicSettings.imageEffects = imageEffects.isOn;
    }

    void ApplyScreenMode()
    {
        Screen.fullScreen = windowMode.isOn;
    }

    void ApplyLanguage()
    {
        LocaManager.SetLanguage(language.captionText.text);
        settingsData.gameSettings.language = language.captionText.text;
       
    }

    void ApplyGameVolume()
    {
        AudioListener.volume = gameVolume.value;
    }

    public void ApplyInputSettings()
    {
        foreach(var field in typeof(CKeys).GetFields())
        {
            field.SetValue(typeof(CKeys), typeof(KeysSettings).GetField(field.Name).GetValue(settingsData.inputSettings.keysSettings));
        }
        File.WriteAllText(Application.streamingAssetsPath + "/Settings.json", JsonUtility.ToJson(settingsData));
    }

    public void ApplyAudioSettings()
    {
        ApplyGameVolume();
        settingsData.audioSettings.gameVolume = gameVolume.value;
        File.WriteAllText(Application.streamingAssetsPath + "/Settings.json", JsonUtility.ToJson(settingsData));
    }

    public static void SetKeyKode(string keyName, KeyCode key)
    {
        typeof(KeysSettings).GetField(keyName).SetValue(settingsData.inputSettings.keysSettings, key);
    }
}

[System.Serializable]
public class SettingsData
{
    public GraphicSettings graphicSettings = new GraphicSettings();
    public GameSettings gameSettings = new GameSettings();
    public InputSettings inputSettings = new InputSettings();
    public AudioSettings audioSettings = new AudioSettings();
}

[System.Serializable]
public class GraphicSettings
{
    public int antiAliasing;
    public int vsync;
    public bool imageEffects;
}

[System.Serializable]
public class GameSettings
{
    public string language;
}

[System.Serializable]
public class InputSettings
{
    public KeysSettings keysSettings = new KeysSettings();
}

[System.Serializable]
public class AudioSettings
{
    public float gameVolume;
}

[System.Serializable]
public class KeysSettings
{
    public  KeyCode moveForward;
    public  KeyCode moveBack;
    public  KeyCode moveLeft;
    public  KeyCode moveRight;
    public  KeyCode rotateShip;
    public  KeyCode attack;
    public  KeyCode launchRocket;
    public  KeyCode inventorySwitch;
    public  KeyCode captureContainer;
    public  KeyCode weapOne;
    public  KeyCode weapTwo;
    public  KeyCode weapThree;
    public  KeyCode autoRotateSwitch;
    public  KeyCode allowCameraMoving;
    public  KeyCode selectTarget;
    public  KeyCode landingMode;
    public  KeyCode saveGame;
    public  KeyCode loadGame;
    public  KeyCode getNearestEnemy;
    public KeyCode communication;
}
