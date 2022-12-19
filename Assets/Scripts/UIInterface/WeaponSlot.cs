using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]    
    Sprite defaultSprite;
    [SerializeField]    
    int weaponIndex;
    [SerializeField]
    bool isRocketLaucnherSlot;

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
        if (UIManager.currentHull.weapons[weaponIndex])
            UIManager.inst.ShowInfoPanel(UIManager.currentHull.weapons[weaponIndex].ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.inst.CloseInfoPanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UIManager.currentHull.weapons[weaponIndex])
        {
            UIManager.draggedItem = Instantiate<MovableCell>(UIManager.inst.movableCell, transform.position, Quaternion.identity, transform.root);
            UIManager.draggedItem.module = UIManager.currentHull.weapons[weaponIndex];
            UIManager.draggedItem.module.DeinstallModule();
            RefreshCell();
        }
    }

    public void OnDrag(PointerEventData ev){}

    public void OnEndDrag(PointerEventData eventData)
    { 
        if (UIManager.draggedItem)
        {
            UIManager.draggedItem.module.InstallModule(UIManager.currentHull, weaponIndex);
            DestroyDraggedObject();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UIManager.draggedItem && (UIManager.draggedItem.module.GetType() == typeof(RocketLauncher)) == isRocketLaucnherSlot)
        if (!UIManager.currentHull.weapons[weaponIndex])
        {
            UIManager.draggedItem.module.InstallModule(UIManager.currentHull, weaponIndex);
            DestroyDraggedObject();
        }
    }

    void RefreshCell()
    {
        if (UIManager.currentHull.weapons[weaponIndex])
            cellImage.sprite = UIManager.currentHull.weapons[weaponIndex].sprite;
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
