using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject prevState;
    public GameObject currentState;

    public Transform canvas;

    public Text loadingText;

    static bool isDataLoaded;

    [SerializeField]
    Image avatarDisplay;

    [SerializeField]
    Dropdown difficultDropDown;
   
    int currentAvararIndex;

    void Awake()
    {
        if (!isDataLoaded)
        {
            DataBase.LoadData();
            isDataLoaded = true;
        }
    }

    void Start()
    {
        //LocaManager.OnLanguageChanged += ApplyNewLanguage;
        currentState = mainMenu;
        foreach (var option in difficultDropDown.options)
            option.text = LocaManager.textUnits[option.text];

        difficultDropDown.value = 2;
        avatarDisplay.sprite = DataBase.allCharacterAvatars[0];
    }

    public void Continue()
    {
        GameManager.LoadGame();
        loadingText.enabled = true;
        currentState.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();   
    }


    public void SwitchState(GameObject state)
    {
        prevState = currentState;
        currentState.SetActive(false);
        state.SetActive(true);
        currentState = state;
    }

    public void SetAvatar(bool isNext)
    {   
        currentAvararIndex = isNext ? currentAvararIndex + 1 : currentAvararIndex - 1;
        currentAvararIndex = Mathf.Clamp(currentAvararIndex, 0, DataBase.allCharacterAvatars.Length-1);
           
        avatarDisplay.sprite = DataBase.allCharacterAvatars[currentAvararIndex];
    }
}




