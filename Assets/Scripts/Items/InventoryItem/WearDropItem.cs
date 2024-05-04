using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WearDropItem : MonoBehaviour, IDropHandler
{
    private InventoryManger inventoryManger;

    private RectTransform rectTrs; //������ ��ƮƮ������

    [Header("���� ������ ����")]
    [SerializeField, Tooltip("���� ������")] private GameObject itemPrefab;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item" && inventoryManger.WearItemCheck() == false)
        {
            ItemUIData itemUIData = eventData.pointerDrag.GetComponent<ItemUIData>();

            if (itemUIData != null)
            {
                GameObject itemObj = Instantiate(itemPrefab, transform);
                WearItemData wearItemData = itemObj.GetComponent<WearItemData>();
                wearItemData.SetItemImage(itemUIData.GetItemIndex(),
                    itemUIData.GetWeaponDamage(), itemUIData.GetWeaponAttackSpeed());

                Destroy(eventData.pointerDrag.gameObject);
            }
        }
    }

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;
    }
}
