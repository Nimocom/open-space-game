using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoHoldIdexator : MonoBehaviour
{
    public Transform cargoHoldCells;
    public Transform shopCells;

	void Awake () 
    {
        if (cargoHoldCells)
        {
            for (int i = 0; i < cargoHoldCells.childCount; i++)
            {
                cargoHoldCells.GetChild(i).GetChild(0).GetComponent<CargoHoldCell>().cellIndex = i;
            }
        }

        if (shopCells)
        {
            for (int i = 0; i < shopCells.childCount; i++)
            {
                shopCells.GetChild(i).GetChild(0).GetComponent<ShopCell>().cellIndex = i;
            }
        }

        Destroy(this);
	}
}
