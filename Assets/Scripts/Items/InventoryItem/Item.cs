using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("������ ����")]
    [SerializeField, Tooltip("�������� ��ȣ")] private int itemIndex;
    [SerializeField, Tooltip("�������� Ÿ��")] private int itemType;

    /// <summary>
    /// �ٸ� ��ũ��Ʈ�� ������ ������ ��ȣ
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ�� ������ ������ Ÿ��
    /// </summary>
    /// <returns></returns>
    public int GetItemType() 
    {
        return itemType;
    }
}
