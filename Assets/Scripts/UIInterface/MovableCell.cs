using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovableCell : MonoBehaviour 
{
    public Module module;

	void Start () 
    {
        GetComponent<Image>().sprite = module.sprite;
	}

	void LateUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 12 * Time.unscaledDeltaTime);
	}
}
