using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public static class LocaManager
{
    public static Dictionary<string, DialogueInstance> dialogues = new Dictionary<string, DialogueInstance>();
    public static Dictionary<string, string> textUnits = new Dictionary<string, string>();

    public static LocalizationData locaData;

    public delegate void del();
    public static event del OnLanguageChanged;

	static LocaManager () 
    {
        SetLanguage("English");
	}

    public static void SetLanguage(string newLanguage)
    {
        dialogues.Clear();
        textUnits.Clear();

        locaData = JsonUtility.FromJson<LocalizationData>(File.ReadAllText((Application.streamingAssetsPath + "/Localization/" + newLanguage + ".lc")));
        string str = JsonUtility.ToJson(locaData);
        File.WriteAllText(Application.streamingAssetsPath + "/Localization/" + newLanguage + ".lc", str);
        foreach (var dialogue in locaData.dialogues)
            dialogues.Add(dialogue.dialogueID, dialogue);
       
        foreach (var unit in locaData.textData)
            textUnits.Add(unit.key, unit.text);

        if(OnLanguageChanged != null)
        OnLanguageChanged(); 
    }
}

[System.Serializable]
public class LocalizationData
{
    public List<DialogueInstance> dialogues;
    public List<TextData> textData;
//
//    public List<string> fabricatorNames;
//
//    public List<string> rocketLauncherNames;
//    public List<string> vertexerNames;
//    public List<string> blasterNames;
//    public List<string> laserNames;
//
//    public List<string> energyGeneratorNames;
//    public List<string> shieldNames;
//    public List<string> repairDroidNames;
//    public List<string> engineNames;
//
//    public List<string> hullNames;

    public List<string> characterNames;
    public List<string> characterSurnames;
}

[System.Serializable]
public class TextData
{
    public string key;
    public string text;
}
