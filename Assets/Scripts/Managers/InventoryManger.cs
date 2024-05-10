using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public class InventoryData
    {
        public int slotIndex; //���� ����
        public List<int> itemSlotIndex = new List<int>(); //�������� ��ġ�� ����
        public List<int> itemIndex = new List<int>(); //�������� ��ȣ
        public List<int> itemType = new List<int>(); //������ Ÿ��
        public List<int> itemQuantity = new List<int>(); //������ ����
        public List<float> weaponDamage = new List<float>(); //���� ���ݷ�
        public List<float> weaponAttackSpeed = new List<float>(); //���� ���ݼӵ�
    }

    private InventoryData inventoryData = new InventoryData();

    private WearItemManager wearItemManager;

    [Header("�κ��丮 ����")]
    [SerializeField, Tooltip("ĵ����")] private Canvas canvas;
    [SerializeField, Tooltip("����")] private GameObject slotPrefab;
    [SerializeField, Tooltip("������")] private GameObject itemPrefab;
    private List<GameObject> itemList = new List<GameObject>(); //������ ����Ʈ
    [SerializeField, Tooltip("������ ������ ��ġ")] private Transform contentTrs;
    private List<Transform> slotTrs = new List<Transform>(); //�������� ������ �� �־� �� ��ġ
    [Space]
    [SerializeField, Tooltip("�κ��丮 �ݱ� ��ư")] private Button closeButton; 
    [Space]
    [SerializeField, Tooltip("�κ��丮")] private GameObject InventoryObj;
    private bool inventoryOnOffCheck = false; //�κ��丮�� �������� �������� Ȯ���ϱ� ���� ����

    private int slotIndex = 12; //������ ������ �ε���

    private List<int> itemSlotIndex = new List<int>(); // �������� �����ϴ� ��ġ
    private List<int> itemIndex = new List<int>(); //������ �ε���
    private List<int> itemType = new List<int>(); //������ Ÿ��
    private List<int> itemQuantity = new List<int>(); //������ ����
    private List<float> weaponDamage = new List<float>(); //���� ���ݷ�
    private List<float> weaponAttackSpeed = new List<float>(); //���� ���ݼӵ�

    private List<int>  swapItem = new List<int>(); //�������� ������ ������ ��ȣ�� �޾ƿ� ����

    [SerializeField] private List<GameObject> itemParent = new List<GameObject>(); //������ �θ� ������ ����

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

        closeButton.onClick.AddListener(() => 
        {
            InventoryObj.SetActive(false);
            inventoryOnOffCheck = false;
            Cursor.lockState = CursorLockMode.Locked;
        });

        for (int i = 0; i < 2; i++)
        {
            swapItem.Add(0);
            itemParent.Add(null);
        }

        InventoryObj.SetActive(false);
    }

    private void Start()
    {
        wearItemManager = WearItemManager.Instance;

        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveData(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            int slotNumber = 1;

            for (int i = 0; i < slotIndex; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, contentTrs);
                ItemInvenDrop dropSc = slotObj.GetComponent<ItemInvenDrop>();
                dropSc.SetNumber(slotNumber++);
                slotTrs.Add(slotObj.transform);
                itemList.Add(null);
                itemSlotIndex.Add(0);
                itemIndex.Add(0);
                itemType.Add(0);
                itemQuantity.Add(0);
                weaponDamage.Add(0);
                weaponAttackSpeed.Add(0);

                inventoryData.itemSlotIndex.Add(0);
                inventoryData.itemIndex.Add(0);
                inventoryData.itemType.Add(0);
                inventoryData.itemQuantity.Add(0);
                inventoryData.weaponDamage.Add(0);
                inventoryData.weaponAttackSpeed.Add(0);
            }

            string setSlot = JsonConvert.SerializeObject(inventoryData);
            PlayerPrefs.SetString("saveInventoryData", setSlot);
        }
    }

    private void Update()
    {
        inventoyOnOff();
        itemSwapCheck();
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

    /// <summary>
    /// ������ ������ �����Ű�� �Լ�
    /// </summary>
    /// <param name="_slotData"></param>
    private void setSaveData(InventoryData _slotData)
    {
        int count = _slotData.slotIndex;

        slotIndex = _slotData.slotIndex;

        int slotNumber = 1;

        for (int i = 0; i < count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, contentTrs);
            ItemInvenDrop dropSc = slotObj.GetComponent<ItemInvenDrop>();
            dropSc.SetNumber(slotNumber++);
            slotTrs.Add(slotObj.transform);
            itemSlotIndex.Add(_slotData.itemSlotIndex[i]);
            itemIndex.Add(_slotData.itemIndex[i]);
            itemType.Add(_slotData.itemType[i]);
            itemQuantity.Add(_slotData.itemQuantity[i]);
            weaponDamage.Add(_slotData.weaponDamage[i]);
            weaponAttackSpeed.Add(_slotData.weaponAttackSpeed[i]);

            if (itemSlotIndex[i] != 0)
            {
                GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i]);
                itemList.Add(itemObj);
            }
            else
            {
                itemList.Add(null);
            }
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// �κ��丮 �������� �����Ű�� �Լ�
    /// </summary>
    private void setSaveItem()
    {
        int count = slotIndex;

        for (int i = 0; i < count; i++)
        {
            inventoryData.itemSlotIndex[i] = itemSlotIndex[i];
            inventoryData.itemIndex[i] = itemIndex[i];
            inventoryData.itemType[i] = itemType[i];
            inventoryData.itemQuantity[i] = itemQuantity[i];
            inventoryData.weaponDamage[i] = weaponDamage[i];
            inventoryData.weaponAttackSpeed[i] = weaponAttackSpeed[i];
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// ������ ��ġ�� �ٲ���� üũ�ϱ� ���� �Լ�
    /// </summary>
    private void itemSwapCheck()
    {
        if ((swapItem[0] != swapItem[1]) && swapItem[0] != 0 && swapItem[1] != 0)
        {
            itemSwap();
        }
    }

    /// <summary>
    /// �������� ��ġ�� ���� �ٲٴ� �Լ�
    /// </summary>
    private void itemSwap()
    {
        GameObject itemObj = itemList[swapItem[0] - 1];
        itemList[swapItem[0] - 1] = itemList[swapItem[1] - 1];
        itemList[swapItem[1] - 1] = itemObj;

        int itemSlotIdx = itemSlotIndex[swapItem[0] - 1];
        itemSlotIndex[swapItem[0] - 1] = itemSlotIndex[swapItem[1] - 1];
        itemSlotIndex[swapItem[1] - 1] = itemSlotIdx;

        int itemIdx = itemIndex[swapItem[0] - 1];
        itemIndex[swapItem[0] - 1] = itemIndex[swapItem[1] - 1];
        itemIndex[swapItem[1] - 1] = itemIdx;

        int itemTp = itemType[swapItem[0] - 1];
        itemType[swapItem[0] - 1] = itemType[swapItem[1] - 1];
        itemType[swapItem[1] - 1] = itemTp;

        int itemQuant = itemQuantity[swapItem[0] - 1];
        itemQuantity[swapItem[0] - 1] = itemQuantity[swapItem[1] - 1];
        itemQuantity[swapItem[1] - 1] = itemQuant;

        float weaponDmg = weaponDamage[swapItem[0] - 1];
        weaponDamage[swapItem[0] - 1] = weaponDamage[swapItem[1] - 1];
        weaponDamage[swapItem[1] - 1] = weaponDmg;

        float weaponAttSpd = weaponAttackSpeed[swapItem[0] - 1];
        weaponAttackSpeed[swapItem[0] - 1] = weaponAttackSpeed[swapItem[1] - 1];
        weaponAttackSpeed[swapItem[1] - 1] = weaponAttSpd;

        swapItem[0] = 0;
        swapItem[1] = 0;

        setSaveItem();
    }

    /// <summary>
    /// �÷��̾ ���� �������� �ֱ� ���� �Լ�, ����X
    /// </summary>
    public void SetItem(GameObject _itemObj)
    {
        Item itemSc = _itemObj.GetComponent<Item>();

        for (int i = 0; i < slotIndex; i++)
        {
            if (itemSc.GetItemPickUpCheck() == true)
            {
                return;
            }

            if (itemSc.GetItemType() == 10) //������ Ÿ���� �������� �ƴ��� üũ
            {
                if (itemIndex[i] == 0)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i]);

                    Weapon weaponSc = itemSc.GetComponent<Weapon>();
                    weaponDamage[i] = weaponSc.WeaponDamage();
                    weaponAttackSpeed[i] = weaponSc.WeaponAttackSpeed();

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
            }
            else if (itemSc.GetItemType() != 10) //���Ⱑ �ƴ϶�� 99������ �������� ������
            {
                bool itemCheck = false;
                int count = itemSlotIndex.Count;
                for (int j = 0; j < count; j++)
                {
                    if (itemIndex[j] == itemSc.GetItemIndex() && itemQuantity[j] < 99)
                    {
                        itemCheck = true;
                    }
                }

                if (itemIndex[i] == itemSc.GetItemIndex() && itemQuantity[i] < 99)
                {
                    itemQuantity[i]++;
                    ItemUIData itemUISc = itemList[i].GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i]);

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
                else if (itemIndex[i] == 0 && itemCheck == false)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

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

    /// <summary>
    /// �������� �����ϱ� ���� �޾ƿ� ���� ��ȣ
    /// </summary>
    /// <param name="_swapItem"></param>
    public void ItemSwapA(int _swapItem)
    {
        swapItem[0] = _swapItem;
    }

    /// <summary>
    /// �������� �����ϱ� ���� �޾ƿ� ���� ��ȣ
    /// </summary>
    /// <param name="_swapItem"></param>
    public void ItemSwapB(int _swapItem)
    {
        swapItem[1] = _swapItem;
    }

    /// <summary>
    /// �巡���� �������� �θ� �޾ƿ� �Լ�
    /// </summary>
    public void ItemParentA(GameObject _itemParent)
    {
        itemParent[0] = _itemParent;
    }

    /// <summary>
    /// ����� �������� �θ� �޾ƿ� �Լ�
    /// </summary>
    public void ItemParentB(GameObject _itemParent)
    {
        itemParent[1] = _itemParent;
    }

    /// <summary>
    /// �������� ������ �� �������� �ִ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_slotNumber"></param>
    /// <returns></returns>
    public bool ItemInCheck(int _slotNumber)
    {
        if (itemIndex[swapItem[0] - 1] == itemIndex[_slotNumber - 1]) //������ �ε����� ���� ��
        {
            if (itemQuantity[swapItem[0] - 1] < 99 && itemIndex[_slotNumber - 1] < 99) //���� ������ �����۰� ���� ��ġ�� �������� 99�� �̸��̶�� ��ħ
            {
                itemList[swapItem[0] - 1] = null;

                itemSlotIndex[swapItem[0] - 1] = 0;

                itemIndex[swapItem[0] - 1] = 0;

                itemType[swapItem[0] - 1] = 0;

                itemQuantity[_slotNumber - 1] += itemQuantity[swapItem[0] - 1];
                itemQuantity[swapItem[0] - 1] = 0;

                swapItem[0] = 0;
                swapItem[1] = 0;

                setSaveItem();
            }
            else //�� ���ǿ� ���� ������ ���� ��ġ�� ���ư�
            {
                return true;
            }
        }
        else if (itemList[_slotNumber - 1] != null &&
            itemIndex[swapItem[0] - 1] != itemIndex[_slotNumber - 1]) //���Կ� �������� �ְų�, ������ �ε����� ���� ������ ������ ��ġ�� �ٲ� ��
        {
            GameObject itemObj = itemList[swapItem[0] - 1];
            itemList[swapItem[0] - 1] = itemList[_slotNumber - 1];
            itemList[_slotNumber - 1] = itemObj;

            itemList[swapItem[0] - 1].transform.SetParent(itemParent[0].transform);
            itemList[_slotNumber - 1].transform.SetParent(itemParent[1].transform);

            int itemSlotIdx = itemSlotIndex[swapItem[0] - 1];
            itemSlotIndex[swapItem[0] - 1] = itemSlotIndex[_slotNumber - 1];
            itemSlotIndex[_slotNumber - 1] = itemSlotIdx;

            int itemIdx = itemIndex[swapItem[0] - 1];
            itemIndex[swapItem[0] - 1] = itemIndex[_slotNumber - 1];
            itemIndex[_slotNumber - 1] = itemIdx;

            int itemTp = itemType[swapItem[0] - 1];
            itemType[swapItem[0] - 1] = itemType[_slotNumber - 1];
            itemType[_slotNumber - 1] = itemTp;

            int itemQuant = itemQuantity[swapItem[0] - 1];
            itemQuantity[swapItem[0] - 1] = itemQuantity[_slotNumber - 1];
            itemQuantity[_slotNumber - 1] = itemQuant;

            float weaponDmg = weaponDamage[swapItem[0] - 1];
            weaponDamage[swapItem[0] - 1] = weaponDamage[_slotNumber - 1];
            weaponDamage[_slotNumber - 1] = weaponDmg;

            float weaponAttSpd = weaponAttackSpeed[swapItem[0] - 1];
            weaponAttackSpeed[swapItem[0] - 1] = weaponAttackSpeed[_slotNumber - 1];
            weaponAttackSpeed[_slotNumber - 1] = weaponAttSpd;

            swapItem[0] = 0;

            setSaveItem();
        }

        return false;
    }

    /// <summary>
    /// ���� ������ �������� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool WearItemCheck()
    {
        if (itemType[swapItem[0] - 1] == 10)
        {
            itemList[swapItem[0] - 1] = null;

            itemSlotIndex[swapItem[0] - 1] = 0;

            itemIndex[swapItem[0] - 1] = 0;

            itemType[swapItem[0] - 1] = 0;

            itemQuantity[swapItem[0] - 1] = 0;

            weaponDamage[swapItem[0] - 1] = 0;

            weaponAttackSpeed[swapItem[0] - 1] = 0;

            swapItem[0] = 0;
            swapItem[1] = 0;

            setSaveItem();

            return false;
        }

        return true;
    }
}
