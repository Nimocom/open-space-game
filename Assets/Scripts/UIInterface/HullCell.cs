using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HullCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    Image cellImage;    

    void Awake()
    {
        cellImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        RefreshCell();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            UIManager.inst.ShowInfoPanel(UIManager.currentHull.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.inst.CloseInfoPanel();
    }


    public void OnDrop(PointerEventData eventData)
    {

    }

    void RefreshCell()
    {
            cellImage.sprite = UIManager.currentHull.sprite;
    }

    void DestroyDraggedObject()
    {
        Destroy(UIManager.draggedItem.gameObject);
        UIManager.draggedItem = null;
        RefreshCell();
    }
}
