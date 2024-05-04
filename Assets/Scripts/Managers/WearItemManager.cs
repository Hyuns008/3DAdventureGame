using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearItemManager : MonoBehaviour
{
    public static WearItemManager Instance;

    [Header("���� ������ ����")]
    [SerializeField] private List<GameObject> weapons;

    private int itemIndex; //������ �����Ϳ� �޾ƿ� �ε���
    private float weaponDamage; //������ �����Ϳ� �޾ƿ� Ÿ��
    private float weaponAttackSpeed; //������ �����Ϳ� �޾ƿ� ����

    private bool weaponCheck = false;

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
    }

    public void SetWearItem(int _itemIndex, float _weaponDamage, float _weaponAttackSpeed)
    {
        itemIndex = _itemIndex;
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;
        weaponCheck = true;
    }

    /// <summary>
    /// ������ ���Ⱑ �������� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetWearWeapon()
    {
        switch (itemIndex)
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
    /// ���⸦ �����ϰ� �ִ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetWeaponCheck()
    {
        return weaponCheck;
    }
}
