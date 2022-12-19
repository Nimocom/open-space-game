using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShieldCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public Sprite defaultSprite;
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
        if (UIManager.currentHull.shield)
            UIManager.inst.ShowInfoPanel(UIManager.currentHull.shield.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.inst.CloseInfoPanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UIManager.currentHull.shield)
        {
            UIManager.draggedItem = Instantiate<MovableCell>(UIManager.inst.movableCell, transform.position, Quaternion.identity, transform.root);
            UIManager.draggedItem.module = UIManager.currentHull.shield;
            UIManager.draggedItem.module.DeinstallModule();
            RefreshCell();
        }
    }

    public void OnDrag(PointerEventData ev){}

    public void OnEndDrag(PointerEventData eventData)
    { 
        if (UIManager.draggedItem)
        {
            UIManager.draggedItem.module.InstallModule(UIManager.currentHull);
            UIManager.inst.InitializeStatusBars(UIManager.currentHull);
            DestroyDraggedObject();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UIManager.draggedItem && UIManager.draggedItem.module.GetType() == typeof(Shield))
        if (!UIManager.currentHull.shield)
        {
            UIManager.draggedItem.module.InstallModule(UIManager.currentHull);
            UIManager.inst.InitializeStatusBars(UIManager.currentHull);
            DestroyDraggedObject();
        }
    }

    void RefreshCell()
    {
        if (UIManager.currentHull.shield)
            cellImage.sprite = UIManager.currentHull.shield.sprite;
        else
            cellImage.sprite = defaultSprite;
    }

    void DestroyDraggedObject()
    {
        Destroy(UIManager.draggedItem.gameObject);
        UIManager.draggedItem = null;
        RefreshCell();
    }
}
