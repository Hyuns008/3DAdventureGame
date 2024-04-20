using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private GameObject item;
    [SerializeField, Tooltip("���� ������ �ִ� ������")] private int itemIndex;
    [SerializeField, Tooltip("���� ������ �ִ� ������ Ÿ��")] private int itemType;
    [SerializeField, Tooltip("���� ������ �ִ� ������ ����")] private int slotQuantity;
    private bool itemCheck = false;
    private GameObject inItem;

    public void itemData(int _itemIndex, int _itemType, int _slotQuantity)
    {
        itemIndex = _itemIndex;
        itemType = _itemType;
        slotQuantity = _slotQuantity;
    }

    public GameObject SetItem()
    {
        if (inItem == null)
        {
            inItem = Instantiate(item, transform);
            return inItem;
        }

        return null;
    }
}
