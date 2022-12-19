using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("MiniMap/Map canvas controller")]
[RequireComponent(typeof(RectTransform))]
public class MapCanvasController : MonoBehaviour
{
    public static MapCanvasController inst;

    public Transform playerTransform;
    public float radarDistance = 10;
    public bool hideOutOfRadius = true;
    public bool useOpacity = true;
    public float maxRadarDistance = 10;
    public float scale = 1.0f;
    public float minimalOpacity = 0.3f;

    public InnerMap InnerMapComponent
    {
        get
        {
            return innerMap;
        }
    }

    public MarkerGroup MarkerGroup
    {
        get
        {
            return markerGroup;
        }
    }
        
//    RectTransform mapRect;
    InnerMap innerMap;
    MapArrow mapArrow;
    MarkerGroup markerGroup;
    float innerMapRadius;

    void Awake()
    {
        inst = this;
//        mapRect = GetComponent<RectTransform>();
        innerMap = GetComponentInChildren<InnerMap>();
        mapArrow = GetComponentInChildren<MapArrow>();
        markerGroup = GetComponentInChildren<MarkerGroup>();
    }

    void Start()
    {
        innerMapRadius = innerMap.getMapRadius();
    }

    public void InitializeMap(Hull player)
    {
        playerTransform = player.transform;
        if (mapArrow.gameObject)
            mapArrow.gameObject.SetActive(true);
    }

	void LateUpdate ()
    {
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
            radarDistance = Mathf.Clamp(radarDistance -= 1, 60, 260);

        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            radarDistance = Mathf.Clamp(radarDistance += 1, 60, 260);
        
        if (playerTransform)
        {
                mapArrow.rotate(Quaternion.Euler(new Vector3(0, 0, playerTransform.eulerAngles.z)));
        }
    }
        

    public void RefreshMap(MapMarker marker)
    {
        if (!playerTransform)
        {
            playerTransform = CameraController.inst.transform;
            mapArrow.gameObject.SetActive(false);
        }

        float scaledRadarDistance = radarDistance * scale;
        float scaledMaxRadarDistance = maxRadarDistance * scale;

        if (marker.isActive)
        {
            float distance = GetDistanceToPlayer(marker.GetPosition());

            float opacity = 1.0f;

            if (distance > scaledRadarDistance)
            {
                if (hideOutOfRadius)
                {
                    if (marker.IsVisible()) 
                    { 
                        marker.Hide(); 
                    }
                    return;
                }
                else
                {
                    if (distance > scaledMaxRadarDistance)
                    {
                        if (marker.IsVisible()) 
                        { 
                            marker.Hide(); 
                        }
                        return;
                    }
                    else
                    {
                        if (useOpacity) 
                        {
                            float opacityRange = scaledMaxRadarDistance - scaledRadarDistance;
                            if (opacityRange <= 0)
                            {
                                Debug.LogError("Max radar distance should be bigger than radar distance");
                                return;
                            }
                            else
                            {
                                float distanceDiff = distance - scaledRadarDistance;
                                opacity = 1 - (distanceDiff / opacityRange);

                                if (opacity < minimalOpacity)
                                {
                                    opacity = minimalOpacity;
                                }
                            }
                        }
                        distance = scaledRadarDistance;
                    }
                }
            }

            if (!marker.IsVisible())
            {
                marker.Show();
            }

            Vector3 posDif = marker.GetPosition() - playerTransform.position;
            Vector3 newPos = new Vector3(posDif.x, posDif.y, 0);
            newPos.Normalize();

            float markerRadius = (marker.markerSize / 2);
            float newLen = (distance / scaledRadarDistance) * (innerMapRadius - markerRadius);

            newPos *= newLen;
            marker.SetLocalPosition(newPos);
            marker.SetOpacity(opacity);
        }
        else
        {
            if (marker.IsVisible())
            {
                marker.Hide();
            }
        }
    }

    private float GetDistanceToPlayer(Vector3 other)
    {   
        
        return Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.y), new Vector2(other.x, other.y));
    }
}
