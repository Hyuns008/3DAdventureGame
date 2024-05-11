using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInvenDrop : MonoBehaviour, IDropHandler
{
    private InventoryManger inventoryManger;
    private WearItemManager wearItemManager;

    private RectTransform rectTrs; //������ ��ƮƮ������

    private int slotNumber; //������ ��ȣ

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item")
        {
            inventoryManger.ItemParentB(transform.gameObject);
            inventoryManger.ItemSwapB(slotNumber);

            WearItemData wearItemDataSc = eventData.pointerDrag.GetComponent<WearItemData>();

            if (inventoryManger.ItemInCheck(slotNumber) == false && wearItemDataSc == null)
            {
                eventData.pointerDrag.transform.SetParent(transform);

                RectTransform eventRect = eventData.pointerDrag.GetComponent<RectTransform>();
                eventRect.position = rectTrs.position;

                inventoryManger.ItemParentB(null);
            }
            else if (wearItemDataSc != null)
            {
                inventoryManger.ItemInstantaite(slotNumber, gameObject, wearItemDataSc.GetItemType(), 
                    wearItemDataSc.GetItemIndex(), wearItemDataSc.GetWeaponDamage(), wearItemDataSc.GetWeaponAttackSpeed());

                wearItemManager.WearWeaponDisarm();

                Destroy(eventData.pointerDrag.gameObject);

                inventoryManger.ItemParentB(null);
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

        wearItemManager = WearItemManager.Instance;
    }

    /// <summary>
    /// ������ �����Ǿ��� �� �޾ƿ� ��ȣ
    /// </summary>
    /// <param name="_slotNumber"></param>
    public void SetNumber(int _slotNumber)
    {
        slotNumber = _slotNumber;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ���� ��ȣ�� Ȯ���ϱ� ���� ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetNumber()
    {
        return slotNumber;
    }
}
