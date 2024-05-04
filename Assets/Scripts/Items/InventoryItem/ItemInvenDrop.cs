using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInvenDrop : MonoBehaviour, IDropHandler
{
    private InventoryManger inventoryManger;

    private RectTransform rectTrs; //������ ��ƮƮ������

    private int slotNumber; //������ ��ȣ

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item" && inventoryManger.ItemInCheck(slotNumber) == false)
        {
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform eventRect = eventData.pointerDrag.GetComponent<RectTransform>();
            eventRect.position = rectTrs.position;

            inventoryManger.ItemSwapB(slotNumber);
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
