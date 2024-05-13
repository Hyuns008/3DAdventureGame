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
    private float screenWidth; //��ũ���� ���� ���̸� ����ϱ� ���� ����
    private float screenHeight; //��ũ���� ���� ���̸� ����ϱ� ���� ����

    private int slotIndex = 12; //������ ������ �ε���

    private List<int> itemSlotIndex = new List<int>(); // �������� �����ϴ� ��ġ
    private List<int> itemIndex = new List<int>(); //������ �ε���
    private List<int> itemType = new List<int>(); //������ Ÿ��
    private List<int> itemQuantity = new List<int>(); //������ ����
    private List<float> weaponDamage = new List<float>(); //���� ���ݷ�
    private List<float> weaponAttackSpeed = new List<float>(); //���� ���ݼӵ�

    private List<int> swapItem = new List<int>(); //�������� ������ ������ ��ȣ�� �޾ƿ� ����

    private List<GameObject> itemParent = new List<GameObject>(); //������ �θ� ������ ����

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
        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveData(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            int slotNumber = 0;

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
    }

    /// <summary>
    /// �κ��丮�� ���� Ű�� �Լ�
    /// </summary>
    private void inventoyOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            if (InventoryObj.transform.position.x >= screenWidth ||
                InventoryObj.transform.position.x <= 0 ||
                InventoryObj.transform.position.y >= screenHeight ||
                InventoryObj.transform.position.y <= 0)
            {
                InventoryObj.transform.position = new Vector3((screenWidth * 0.5f) - 400f, (screenHeight * 0.5f) + 350f, 0f);
            }

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

        int slotNumber = 0;

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
    /// �����۳��� ��ġ�� �ٲٱ� ���� �Լ�
    /// </summary>
    /// <param name="_slotNumber"></param>
    private void itemSwapCheck(int _slotNumber)
    {
        GameObject itemObj = itemList[swapItem[0]];
        itemList[swapItem[0]] = itemList[_slotNumber];
        itemList[_slotNumber] = itemObj;

        itemList[swapItem[0]].transform.SetParent(itemParent[0].transform);
        itemList[_slotNumber].transform.SetParent(itemParent[1].transform);

        itemList[swapItem[0]].transform.position = itemParent[0].transform.position;
        itemList[_slotNumber].transform.position = itemParent[1].transform.position;

        int itemSlotIdx = itemSlotIndex[swapItem[0]];
        itemSlotIndex[swapItem[0]] = itemSlotIndex[_slotNumber];
        itemSlotIndex[_slotNumber] = itemSlotIdx;

        int itemIdx = itemIndex[swapItem[0]];
        itemIndex[swapItem[0]] = itemIndex[_slotNumber];
        itemIndex[_slotNumber] = itemIdx;

        int itemTp = itemType[swapItem[0]];
        itemType[swapItem[0]] = itemType[_slotNumber];
        itemType[_slotNumber] = itemTp;

        int itemQuant = itemQuantity[swapItem[0]];
        itemQuantity[swapItem[0]] = itemQuantity[_slotNumber];
        itemQuantity[_slotNumber] = itemQuant;

        float weaponDmg = weaponDamage[swapItem[0]];
        weaponDamage[swapItem[0]] = weaponDamage[_slotNumber];
        weaponDamage[_slotNumber] = weaponDmg;

        float weaponAttSpd = weaponAttackSpeed[swapItem[0]];
        weaponAttackSpeed[swapItem[0]] = weaponAttackSpeed[_slotNumber];
        weaponAttackSpeed[_slotNumber] = weaponAttSpd;
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

                    Weapon weaponSc = itemSc.GetComponent<Weapon>();
                    weaponDamage[i] = weaponSc.WeaponDamage();
                    weaponAttackSpeed[i] = weaponSc.WeaponAttackSpeed();

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
            else if (itemSc.GetItemType() >= 20) //��� �ƴ϶�� 99������ �������� ������
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
    /// <param name="_slotNumber">������ ��ġ</param>
    /// <returns></returns>
    public bool ItemInCheck(int _slotNumber)
    {
        if (itemIndex[swapItem[0]] == itemIndex[_slotNumber]) //������ �ε����� ���� ��
        {
            if (itemQuantity[swapItem[0]] < 99 && itemIndex[_slotNumber] < 99) //���� ������ �����۰� ���� ��ġ�� �������� 99�� �̸��̶�� ��ħ
            {
                itemList[swapItem[0]] = null;

                itemSlotIndex[swapItem[0]] = 0;

                itemIndex[swapItem[0]] = 0;


                itemType[swapItem[0]] = 0;

                itemQuantity[_slotNumber] += itemQuantity[swapItem[0]];
                itemQuantity[swapItem[0]] = 0;

                swapItem[0] = 0;
                swapItem[1] = 0;

                setSaveItem();
            }
            else //�� ���ǿ� ���� ������ ���� ��ġ�� ���ư�
            {
                return true;
            }
        }
        else if (itemList[_slotNumber] != null &&
            itemIndex[swapItem[0]] != itemIndex[_slotNumber]) //���Կ� �������� �ְų�, ������ �ε����� ���� ������ ������ ��ġ�� �ٲ� ��
        {
            itemSwapCheck(_slotNumber);
        }
        else
        {
            GameObject itemObj = itemList[swapItem[0]];
            itemList[swapItem[0]] = itemList[_slotNumber];
            itemList[_slotNumber] = itemObj;

            int itemSlotIdx = itemSlotIndex[swapItem[0]];
            itemSlotIndex[swapItem[0]] = itemSlotIndex[_slotNumber];
            itemSlotIndex[_slotNumber] = itemSlotIdx;

            int itemIdx = itemIndex[swapItem[0]];
            itemIndex[swapItem[0]] = itemIndex[_slotNumber];
            itemIndex[_slotNumber] = itemIdx;

            int itemTp = itemType[swapItem[0]];
            itemType[swapItem[0]] = itemType[_slotNumber];
            itemType[_slotNumber] = itemTp;

            int itemQuant = itemQuantity[swapItem[0]];
            itemQuantity[swapItem[0]] = itemQuantity[_slotNumber];
            itemQuantity[_slotNumber] = itemQuant;

            float weaponDmg = weaponDamage[swapItem[0]];
            weaponDamage[swapItem[0]] = weaponDamage[_slotNumber];
            weaponDamage[_slotNumber] = weaponDmg;

            float weaponAttSpd = weaponAttackSpeed[swapItem[0]];
            weaponAttackSpeed[swapItem[0]] = weaponAttackSpeed[_slotNumber];
            weaponAttackSpeed[_slotNumber] = weaponAttSpd;
        }

        return false;
    }

    /// <summary>
    /// ���� ������ �������� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool WearItemCheck()
    {
        if (itemType[swapItem[0]] < 20)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// �������� ������ �κ��丮���� �������� ����
    /// </summary>
    public void WearItemDropCheck()
    {
        itemList[swapItem[0]] = null;

        itemSlotIndex[swapItem[0]] = 0;

        itemIndex[swapItem[0]] = 0;

        itemType[swapItem[0]] = 0;

        itemQuantity[swapItem[0]] = 0;

        weaponDamage[swapItem[0]] = 0;

        weaponAttackSpeed[swapItem[0]] = 0;

        swapItem[0] = 0;
        swapItem[1] = 0;

        itemParent[0] = null;

        setSaveItem();
    }

    public void ItemInstantaite(int _slotNumber, GameObject _itemParent, int _itemType, int _itemIndex,
        float _weaponDamage, float _weaponAttackSpeed)
    {
        GameObject itemObj = Instantiate(itemPrefab, _itemParent.transform);
        itemObj.transform.position = _itemParent.transform.position;

        ItemUIData itemUIDataSc = itemObj.GetComponent<ItemUIData>();
        itemUIDataSc.SetItemImage(_itemIndex, _itemType, 1, _weaponDamage, _weaponAttackSpeed);

        itemList[_slotNumber] = itemObj;

        itemSlotIndex[_slotNumber] = 1;

        itemType[_slotNumber] = _itemType;

        itemIndex[_slotNumber] = _itemIndex;

        itemQuantity[_slotNumber] = 1;

        weaponDamage[_slotNumber] = _weaponDamage;

        weaponAttackSpeed[_slotNumber] = _weaponAttackSpeed;

        swapItem[0] = 0;
        swapItem[1] = 0;

        itemParent[0] = null;

        setSaveItem();
    }
}
