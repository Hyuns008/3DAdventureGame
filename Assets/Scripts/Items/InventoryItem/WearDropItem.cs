using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WearDropItem : MonoBehaviour, IDropHandler
{
    private InventoryManger inventoryManger;
    private WearItemManager wearItemManager;

    [Header("���� ������ ����")]
    [SerializeField, Tooltip("���� ������")] private GameObject itemPrefab;
    [SerializeField, Tooltip("���� ���� ������")] private int wearItemTypeCheck;
    private int weaponIndex; //������ �ε����� �޾ƿ��� ���� ����

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item" && inventoryManger.WearItemCheck() == false)
        {
            ItemUIData itemUIData = eventData.pointerDrag.GetComponent<ItemUIData>();

            if (itemUIData != null && itemUIData.GetItemType() == wearItemTypeCheck)
            {
                if (weaponIndex == 0)
                {
                    GameObject itemObj = Instantiate(itemPrefab, transform);

                    WearItemData wearItemData = itemObj.GetComponent<WearItemData>();
                    wearItemData.SetItemImage(itemUIData.GetItemType(), itemUIData.GetItemIndex(),
                        itemUIData.GetWeaponDamage(), itemUIData.GetWeaponAttackSpeed(), itemUIData.GetWeaponUpgrade());
                    weaponIndex = itemUIData.GetItemIndex();

                    inventoryManger.WearItemDropCheck();

                    Destroy(eventData.pointerDrag.gameObject);
                }
            }
        }
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;

        wearItemManager = WearItemManager.Instance;

        weaponIndex = wearItemManager.GetWeaponIndex();

        if (weaponIndex != 0 && wearItemManager.GetWeaponType() == wearItemTypeCheck)
        {
            GameObject itemObj = Instantiate(itemPrefab, transform);

            WearItemData wearItemData = itemObj.GetComponent<WearItemData>();
            wearItemData.SetItemImage(wearItemManager.GetWeaponType(), wearItemManager.GetWeaponIndex(),
                wearItemManager.GetWeaponDamage(), wearItemManager.GetWeaponAttackSpeed(), wearItemManager.GetWeaponUpgrade());
            weaponIndex = wearItemManager.GetWeaponIndex();
        }
    }

    private void Update()
    {
        if (wearItemManager.GetWeaponType() == 0 && weaponIndex != 0)
        {
            weaponIndex = 0;
        }
    }
}
