using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ServiceCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public static ServiceCell inst;
    public static Module currentModule;
    static Image cellImage;    
    public Text repairCostText;
    public Text sellCostText;

    void Awake()
    {
        cellImage = GetComponent<Image>();
        inst = this;
    }

    void OnEnable()
    {
        RefreshCell();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentModule)
            UIManager.inst.ShowInfoPanel(currentModule.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.inst.CloseInfoPanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentModule)
        {
            UIManager.draggedItem = Instantiate<MovableCell>(UIManager.inst.movableCell, transform.position, Quaternion.identity, transform.root);
            UIManager.draggedItem.module = currentModule;
            currentModule = null;
            RefreshCell();
        }
    }

    public void OnDrag(PointerEventData ev){}

    public void OnEndDrag(PointerEventData eventData)
    { 
        if (UIManager.draggedItem)
        {
            currentModule = UIManager.draggedItem.module;
            DestroyDraggedObject();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UIManager.draggedItem && !UIManager.isShopItem && !currentModule)
        {
            currentModule = UIManager.draggedItem.module;
            DestroyDraggedObject();
        }
    }

    public void RefreshCell()
    {
        if (currentModule)
        {
            cellImage.sprite = currentModule.sprite;
            if (currentModule.health < 100f)
            {
                sellCostText.text = ((int)(currentModule.cost - (currentModule.cost * (1 - (ServiceCell.currentModule.health / 100f))))).ToString();
                repairCostText.text = ((int)(currentModule.cost * (1 - (ServiceCell.currentModule.health / 100f)))).ToString();
            }
            else
            {
                sellCostText.text = currentModule.cost.ToString();
                repairCostText.text = "--";
            }
        }
        else
        {
            cellImage.sprite = UIManager.inst.defaultSprite;
            sellCostText.text = "--";
            repairCostText.text = "--";
        }
    }

    void DestroyDraggedObject()
    {
        Destroy(UIManager.draggedItem.gameObject);
        UIManager.draggedItem = null;
        RefreshCell();
    }
}
