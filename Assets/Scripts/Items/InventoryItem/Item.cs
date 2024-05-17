using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("������ ����")]
    [SerializeField, Tooltip("�������� ��ȣ")] private int itemIndex;
    [SerializeField, Tooltip("�������� Ÿ��")] private int itemType;
    [Space]
    [SerializeField, Tooltip("�������� �ֿ� �� �ִ��� üũ")] private bool itemPickUpCheck = false;
    [Space]
    [SerializeField, Tooltip("�������� �������� �� �ݱ� ���� �ð�")] private float pickUpTime;
    private float pickUpTimer; //�Ⱦ� Ÿ�̸�
    private bool pickUp = false; //ó������ �������� �� �ֿ� �� �ִ��� üũ

    private void Start()
    {
        itemPickUpCheck = true;
        pickUpTimer = pickUpTime;
    }

    private void Update()
    {
        if (pickUp == false)
        {
            pickUpTimer -= Time.deltaTime;
            if (0 >= pickUpTimer)
            {
                pickUp = true;
                pickUpTimer = pickUpTime;
                itemPickUpCheck = false;
            }
        }
    }

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

    /// <summary>
    /// �������� �ֿ� �� �ִ��� üũ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetItemPickUpCheck()
    {
        return itemPickUpCheck;
    }

    /// <summary>
    /// �������� �ֿ� �� �ְų� ���� ����� �Լ�
    /// </summary>
    /// <param name="_pickUpCheck"></param>
    public void SetItemPickUpCheck(bool _pickUpCheck)
    {
        itemPickUpCheck = _pickUpCheck;
    }
}
