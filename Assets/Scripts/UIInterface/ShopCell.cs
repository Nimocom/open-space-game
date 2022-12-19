using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler//, IDropHandler
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
        if (PlanetManager.shopItems[cellIndex])
            UIManager.inst.ShowInfoPanel(PlanetManager.shopItems[cellIndex].ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.inst.CloseInfoPanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PlanetManager.shopItems[cellIndex] && PlanetManager.characterData.credits >= PlanetManager.shopItems[cellIndex].cost)
        {
            UIManager.draggedItem = Instantiate<MovableCell>(UIManager.inst.movableCell, transform.position, Quaternion.identity, transform.root);
            UIManager.draggedItem.module = PlanetManager.shopItems[cellIndex];
            PlanetManager.inst.DecreaseCredits(PlanetManager.shopItems[cellIndex].cost);
            PlanetManager.shopItems[cellIndex] = null;
            UIManager.isShopItem = true;
            RefreshCell();
        }
    }

    public void OnDrag(PointerEventData ev){}

    public void OnEndDrag(PointerEventData eventData)
    { 
        if (UIManager.draggedItem)
        {
            PlanetManager.shopItems[cellIndex] = UIManager.draggedItem.module;    
            PlanetManager.inst.IncreaseCredits(PlanetManager.shopItems[cellIndex].cost);
            DestroyDraggedObject();
        }
        UIManager.isShopItem = false;
    }

    public void RefreshCell()
    {
        if (PlanetManager.shopItems[cellIndex])
            cellImage.sprite = PlanetManager.shopItems[cellIndex].sprite;
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
