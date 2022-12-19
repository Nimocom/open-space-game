using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GatewayCell : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public GameObject gatewayGO;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.draggedItem)
            gatewayGO.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.draggedItem)
            gatewayGO.SetActive(true);
    }
        
    public void OnDrop(PointerEventData eventData)
    {
        if (UIManager.draggedItem)
        {
            UIManager.currentHull.ThrowOutModule(UIManager.draggedItem.module);
            Destroy(UIManager.draggedItem.gameObject);
            UIManager.draggedItem = null;
            gatewayGO.SetActive(true);
        }
    }
}
