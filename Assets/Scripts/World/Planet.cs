using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{
    public string planetName;
    public Texture[] planetTextures;
    public string population;
    public string mass;
    public float radius;
    public float periodOfRotation;
    public float orbitalVelocity;
   
    public int planetIndex;

    void OnMouseEnter()
    {
        UIManager.inst.ShowInfoPanel
        (
            LocaManager.textUnits["planetname"] + 
            LocaManager.textUnits[planetName.ToLower()] + "\n" +
            LocaManager.textUnits["population"] + population + "\n" +
            LocaManager.textUnits["mass"] + mass + "\n" +
            LocaManager.textUnits["radius"] + radius + "\n" +
            LocaManager.textUnits["periodofrotation"] + periodOfRotation + "\n" +
            LocaManager.textUnits["orbitalvelocity"] + orbitalVelocity
        );
    }

    void OnMouseExit()
    {
        UIManager.inst.CloseInfoPanel();
    }
}

