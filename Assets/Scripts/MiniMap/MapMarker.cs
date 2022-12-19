using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[AddComponentMenu("MiniMap/Map marker")]
public class MapMarker : MonoBehaviour
{
    public Sprite markerSprite;
    public float markerSize = 6.5f;
    public bool isActive = true;

    public Image MarkerImage
    {
        get
        {
            return markerImage;
        }
    }
  
    Image markerImage;


    void Start () 
    {
        GameObject markerImageObject = new GameObject("Marker");
        markerImageObject.AddComponent<Image>();
        markerImageObject.transform.SetParent(MapCanvasController.inst.MarkerGroup.MarkerGroupRect);
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.sprite = markerSprite;
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);
	}


	void Update () 
    {
        MapCanvasController.inst.RefreshMap(this);
        markerImage.rectTransform.rotation = Quaternion.identity;
	}

    void OnDestroy()
    {
        if (markerImage)
        {
            Destroy(markerImage.gameObject);
        }
    }

    public void Show()
    {
        markerImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        markerImage.gameObject.SetActive(false);
    }

    public bool IsVisible()
    {
        return markerImage.gameObject.activeSelf;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public void SetLocalPosition(Vector3 pos)
    {
        markerImage.rectTransform.localPosition = pos;

    }

    public void SetOpacity(float opacity)
    {
        markerImage.color = new Color(1.0f, 1.0f, 1.0f, opacity);
    }
}
