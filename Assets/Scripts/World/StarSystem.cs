using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class StarSystem : MonoBehaviour 
{
    public static StarSystem instance;

    [SerializeField]
    string systemName;
    [SerializeField]
    int systemIndex;
    [SerializeField]
    Material systemSkybox;
    [SerializeField]
    Transform star;
    [SerializeField]
    Character.Fraction mainFraction;

    AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        //GameManager.InitializeSpaceEnvironment(systemIndex);
    }

    void Start()
    {
        audioSource.Play();
        Camera.main.GetComponent<SunShafts>().sunTransform = star;
    }

    public int GetSystemIndex()
    {
        return systemIndex;
    }

    public string GetSystemName()
    {
        return systemName;
    }

    public void SetMainFraction(Character.Fraction fraction)
    {
        mainFraction = fraction;
    }

    public Character.Fraction GetMainFraction()
    {
        return mainFraction;
    }

    public Material GetSkyboxMaterial()
    {
        return systemSkybox;
    }

    public static Character GenerateNPC(int level = -1, Character.Fraction fraction = 0)
    {
        return null;
    }
}
