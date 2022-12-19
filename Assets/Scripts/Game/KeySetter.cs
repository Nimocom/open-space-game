using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Linq;

public class KeySetter : MonoBehaviour
{
    bool waitingForKey;
    [SerializeField]
    string keyString;
    [SerializeField]
    Text currentKey;

    void Awake()
    {
        //currentKey = transform.GetChild(0).GetComponent<Text>();
       // keyString = transform.parent.GetComponent<Text>().text;
        GetComponent<Button>().onClick.AddListener(SetKey);
    }

    void OnEnable()
    {
        //keyString = transform.parent.GetComponent<Text>().text;
        currentKey.text = typeof(CKeys).GetField(keyString).GetValue(typeof(CKeys)).ToString();
    }

    public void SetKey()
    {
        waitingForKey = true;
        currentKey.text = " - - ";
        StartCoroutine(RefreshKey());
    }

    IEnumerator RefreshKey()
    {
        while (waitingForKey)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(key))
                {
                    Settings.SetKeyKode(keyString, key);
                    currentKey.text = key.ToString();
                    waitingForKey = false;  
                    break;
                }
            }
            yield return null;
        }
    }
}
