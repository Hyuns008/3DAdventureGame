using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public class InventoryData
    {
        public int slotIndex;
        public List<int> itemIndex;
        public List<int> itemType;
        public List<int> slotCheck;
        public List<float> weaponDamage;
        public List<float> weaponAttackSpeed;
    }

    private InventoryData inventoryData = new InventoryData();

    [Header("�κ��丮 ����")]
    [SerializeField, Tooltip("ĵ����")] private Canvas canvas;
    [SerializeField, Tooltip("����")] private GameObject itemSlot;
    [SerializeField, Tooltip("������ ������ ��ġ")] private Transform contentTrs;
    [SerializeField, Tooltip("������ ���� ����Ʈ")] private List<GameObject> slotList;
    [Space]
    [SerializeField, Tooltip("�κ��丮")] private GameObject InventoryObj;
    private bool inventoryOnOffCheck = false; //�κ��丮�� �������� �������� Ȯ���ϱ� ���� ����

    private int slotIndex = 12; //������ ������ �ε���

    [SerializeField] private List<int> itemIndex; //������ �ε���
    [SerializeField] private List<int> itemType; //������ Ÿ��
    [SerializeField] private List<int> slotQuantity; //������ ����
    [SerializeField] private List<float> weaponDamage; //���� ���ݷ�
    [SerializeField] private List<float> weaponAttackSpeed; //���� ���ݼӵ�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InventoryObj.SetActive(false);
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveSlot(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            for (int i = 0; i < slotIndex; i++)
            {
                slotList.Add(Instantiate(itemSlot, contentTrs));
                itemIndex.Add(0);
                itemType.Add(0);
                slotQuantity.Add(0);
            }

            string setSlot = JsonConvert.SerializeObject(inventoryData);
            PlayerPrefs.SetString("saveInventoryData", setSlot);
        }
    }

    private void Update()
    {
        inventoyOnOff();
    }

    /// <summary>
    /// �κ��丮�� ���� Ű�� �Լ�
    /// </summary>
    private void inventoyOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOnOffCheck = InventoryObj == InventoryObj.activeSelf ? false : true;
            InventoryObj.SetActive(inventoryOnOffCheck);
            InventoryObj.transform.SetAsLastSibling();

            if (inventoryOnOffCheck == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void setSaveSlot(InventoryData _slotData)
    {
        for (int i = 0; i < _slotData.slotIndex; i++)
        {
            slotList.Add(Instantiate(itemSlot, contentTrs));
            itemIndex.Add(0);
            itemType.Add(0);
            slotQuantity.Add(0);
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// �÷��̾ ���� �������� �ֱ� ���� �Լ�, ����X
    /// </summary>
    public void SetItem(GameObject _itemObj)
    {
        int count = slotList.Count;

        Item itemSc = _itemObj.GetComponent<Item>();

        for (int i = 0; i < count; i++)
        {
            if (itemSc.GetItemType() == 10)
            {
                if (itemIndex[i] == 0)
                {
                    Slot slotSc = slotList[i].GetComponent<Slot>();
                    slotSc.itemData(itemSc.GetItemIndex(), itemSc.GetItemType(), 1);

                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    slotQuantity[i] = 1;

                    Destroy(_itemObj);
                    return;
                }
            }
            else if (itemSc.GetItemType() != 10)
            {
                if (itemIndex[i] == itemSc.GetItemIndex() && slotQuantity[i] < 99)
                {
                    slotQuantity[i]++;

                    Destroy(_itemObj);
                    return;
                }
                else if (itemIndex[i] == 0)
                {
                    Slot slotSc = slotList[i].GetComponent<Slot>();
                    slotSc.itemData(itemSc.GetItemIndex(), itemSc.GetItemType(), 1);

                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    slotQuantity[i] = 1;

                    Destroy(_itemObj);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ�� ĵ������ ����� �� �ְ� �ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Canvas GetCanvas()
    {
        return canvas;
    }

    /// <summary>
    /// �κ��丮�� �����ִ��� �����ִ��� üũ�ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetInventoryOnOffCheck()
    {
        return inventoryOnOffCheck;
    }
}
