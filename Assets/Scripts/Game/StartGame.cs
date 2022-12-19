using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{ 
    void Start()
    { 
        GameManager.InitializeSpaceEnvironment();
        UIManager.inst.SetCursorToAim();
        System.GC.Collect();
    }
}


   
