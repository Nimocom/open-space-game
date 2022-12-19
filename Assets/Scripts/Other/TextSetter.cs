using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour 
{
    Text UIText;
    string key;

    void Awake()
    {
        LocaManager.OnLanguageChanged += ApplyLanguage;
        ApplyLanguage();
    }

    void ApplyLanguage()
    {
        if (key == null)
        {
            UIText = GetComponent<Text>();
            key = UIText.text.Trim();
        }

        UIText.text = LocaManager.textUnits[key];
    }
}
