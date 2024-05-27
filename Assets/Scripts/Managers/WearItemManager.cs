using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearItemManager : MonoBehaviour
{
    public static WearItemManager Instance;

    public class WearItem
    {
        public int wearWeaponType;
        public int wearWeaponIndex;
        public float wearWeaponDamage;
        public float wearWeaponAttackSpeed;
        public int wearWeaponUpgrade;

        public List<int> wearGearType = new List<int>();
        public List<int> wearGearIndex = new List<int>();
    }

    private WearItem wearItem = new WearItem();

    private TutorialManager tutorialManager;

    [Header("���� ������ ����")]
    [SerializeField] private List<GameObject> weapons;

    private int weaponType; //������ Ÿ���� �޾ƿ� ����
    private int weaponIndex; //������ �����Ϳ� �޾ƿ� �ε���
    [SerializeField] private float weaponDamage; //������ �����Ϳ� �޾ƿ� ���ݷ�
    [SerializeField] private float weaponAttackSpeed; //������ �����Ϳ� �޾ƿ� ���ݼӵ�
    private int wearWeaponUpgrade; //������ �����Ϳ� �޾ƿ� ��ȭȽ��

    private List<int> wearGearType = new List<int>();
    private List<int> wearGearIndex = new List<int>();

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

        for (int i = 0; i < 7; i++)
        {
            wearGearType.Add(0);
            wearGearIndex.Add(0);
        }

        if (PlayerPrefs.GetString("wearItemSaveKey") != string.Empty)
        {
            string getWearItem = PlayerPrefs.GetString("wearItemSaveKey");
            wearItem = JsonConvert.DeserializeObject<WearItem>(getWearItem);
            getSaveWearItem(wearItem);
        }
        else
        {
            wearItem.wearWeaponType = 0;
            wearItem.wearWeaponIndex = 0;
            wearItem.wearWeaponDamage = 0;
            wearItem.wearWeaponAttackSpeed = 0;
            wearItem.wearWeaponUpgrade = 0;

            for (int i = 0; i < 7; i++)
            {
                wearItem.wearGearType.Add(0);
                wearItem.wearGearIndex.Add(0);
            }
        }
    }

    private void Start()
    {
        tutorialManager = TutorialManager.Instance;
    }

    /// <summary>
    /// ����� �����͸� �ҷ����� ���� �Լ�
    /// </summary>
    /// <param name="_wearItem"></param>
    private void getSaveWearItem(WearItem _wearItem)
    {
        weaponType = _wearItem.wearWeaponType;
        weaponIndex = _wearItem.wearWeaponIndex;
        weaponDamage = _wearItem.wearWeaponDamage;
        weaponAttackSpeed = _wearItem.wearWeaponAttackSpeed;
        wearWeaponUpgrade = _wearItem.wearWeaponUpgrade;

        for (int i = 0; i < 7; i++)
        {
            wearGearType[i] = _wearItem.wearGearType[i];
            wearGearIndex[i] = _wearItem.wearGearIndex[i];
        }
    }

    public void SetWearItem(int _itemType, int _itemIndex, float _weaponDamage, float _weaponAttackSpeed, int _weaponUpgrade)
    {
        if (_itemType == 10)
        {
            weaponType = _itemType;
            wearItem.wearWeaponType = _itemType;

            weaponIndex = _itemIndex;
            wearItem.wearWeaponIndex = _itemIndex;

            weaponDamage = _weaponDamage;
            wearItem.wearWeaponDamage = _weaponDamage;

            weaponAttackSpeed = _weaponAttackSpeed;
            wearItem.wearWeaponAttackSpeed = _weaponAttackSpeed;

            wearWeaponUpgrade = _weaponUpgrade;
            wearItem.wearWeaponUpgrade = _weaponUpgrade;
        }
        else if (_itemType > 10)
        {
            if (_itemType == 11)
            {
                wearGearType[0] = _itemType;
                wearGearIndex[0] = _itemIndex;

                wearItem.wearGearType[0] = _itemType;
                wearItem.wearGearIndex[0] = _itemIndex;
            }
            else if (_itemType == 12)
            {
                wearGearType[1] = _itemType;
                wearGearIndex[1] = _itemIndex;

                wearItem.wearGearType[1] = _itemType;
                wearItem.wearGearIndex[1] = _itemIndex;
            }
            else if (_itemType == 13)
            {
                wearGearType[2] = _itemType;
                wearGearIndex[2] = _itemIndex;

                wearItem.wearGearType[2] = _itemType;
                wearItem.wearGearIndex[2] = _itemIndex;
            }
            else if (_itemType == 14)
            {
                wearGearType[3] = _itemType;
                wearGearIndex[3] = _itemIndex;

                wearItem.wearGearType[3] = _itemType;
                wearItem.wearGearIndex[3] = _itemIndex;
            }
        }

        if (tutorialManager != null && tutorialManager.TutorialTrue() == true)
        {
            return;
        }

        string setWearItem = JsonConvert.SerializeObject(wearItem);
        PlayerPrefs.SetString("wearItemSaveKey", setWearItem);
    }

    /// <summary>
    /// ������ ���Ⱑ �������� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetWearWeapon()
    {
        switch (weaponIndex)
        {
            case 100:
                return weapons[0];
            case 101:
                return weapons[1];
            case 102:
                return weapons[2];
            case 103:
                return weapons[3];
            case 104:
                return weapons[4];
        }

        return null;
    }

    /// <summary>
    /// ������ Ÿ��
    /// </summary>
    /// <returns></returns>
    public int GetWeaponType()
    {
        return weaponType;
    }

    /// <summary>
    /// ������ �ε����� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetWeaponIndex()
    {
        return weaponIndex;
    }

    /// <summary>
    /// ������ ���ݷ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    /// <summary>
    /// ������ ���ݼӵ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    /// <summary>
    /// ������ ��ȭ Ƚ��
    /// </summary>
    /// <returns></returns>
    public int GetWeaponUpgrade()
    {
        return wearWeaponUpgrade;
    }

    /// <summary>
    /// ������ Ÿ��
    /// </summary>
    /// <returns></returns>
    public int GetItemType(int _type)
    {
        return wearGearType[_type];
    }

    /// <summary>
    /// ������ �ε����� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex(int _index)
    {
        return wearGearIndex[_index];
    }

    /// <summary>
    /// ������ �������� �κ��丮�� ������ �� ����� ���� �������ֱ� ���� �Լ�
    /// </summary>
    public void WearWeaponDisarm()
    {
        weaponType = 0;
        wearItem.wearWeaponType = 0;

        weaponIndex = 0;
        wearItem.wearWeaponIndex = 0;

        weaponDamage = 0;
        wearItem.wearWeaponDamage = 0;

        weaponAttackSpeed = 0;
        wearItem.wearWeaponAttackSpeed = 0;

        wearWeaponUpgrade = 0;
        wearItem.wearWeaponUpgrade = 0;

        for (int i = 0; i < 7; i++)
        {
            wearGearType[i] = 0;
            wearGearIndex[i] = 0;

            wearItem.wearGearType[i] = 0;
            wearItem.wearGearIndex[i] = 0;
        }

        if (tutorialManager != null && tutorialManager.TutorialTrue() == true)
        {
            return;
        }

        string setWearItem = JsonConvert.SerializeObject(wearItem);
        PlayerPrefs.SetString("wearItemSaveKey", setWearItem);
    }
}
