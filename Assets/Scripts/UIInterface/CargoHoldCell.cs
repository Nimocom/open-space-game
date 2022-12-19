using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CargoHoldCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public int cellIndex;
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
        if (UIManager.currentHull.cargoHoldModules[cellIndex])
            UIManager.inst.ShowInfoPanel(UIManager.currentHull.cargoHoldModules[cellIndex].ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.inst.CloseInfoPanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UIManager.currentHull.cargoHoldModules[cellIndex])
        {
            UIManager.draggedItem = Instantiate<MovableCell>(UIManager.inst.movableCell, transform.position, Quaternion.identity, transform.root);
            UIManager.draggedItem.module = UIManager.currentHull.cargoHoldModules[cellIndex];
            UIManager.currentHull.cargoHoldModules[cellIndex] = null;
            RefreshCell();
        }
    }

    public void OnDrag(PointerEventData ev){}

    public void OnEndDrag(PointerEventData eventData)
    { 
        if (UIManager.draggedItem)
        {
            UIManager.draggedItem.module.LoadToCargoHold(UIManager.currentHull, cellIndex);
            DestroyDraggedObject();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UIManager.draggedItem && !UIManager.currentHull.cargoHoldModules[cellIndex])
        {
            UIManager.draggedItem.module.LoadToCargoHold(UIManager.currentHull, cellIndex);
            DestroyDraggedObject();
        }
    }

    void RefreshCell()
    {
        if (UIManager.currentHull.cargoHoldModules[cellIndex])
            cellImage.sprite = UIManager.currentHull.cargoHoldModules[cellIndex].sprite;
        else
            cellImage.sprite = UIManager.inst.defaultSprite;
    }

    void DestroyDraggedObject()
    {
        Destroy(UIManager.draggedItem.gameObject);
        UIManager.draggedItem = null;
        RefreshCell();
    }
}
