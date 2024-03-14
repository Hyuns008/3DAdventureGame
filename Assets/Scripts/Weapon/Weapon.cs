using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythology
    }

    [Header("���� ����")]
    [SerializeField] private int weaponNumber;
    [SerializeField] private WeaponType type;
    [SerializeField] private int weaponLevel;
    [SerializeField] private float weaponDamage;

    /// <summary>
    /// ������ ��ȣ�� �ٸ� ��ũ��Ʈ���� ������ �� �ְ� �ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int WeaponNumber()
    {
        return weaponNumber;
    }

    /// <summary>
    /// ������ ������ �ٸ� ��ũ��Ʈ���� ������ �� �ְ� �ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int WeaponLevel()
    {
        return weaponLevel;
    }

    /// <summary>
    /// ������ ���ݷ��� �ٸ� ��ũ��Ʈ���� ������ �� �ְ� �ϴ� �Լ�
    /// </summary>
    public float WeaponDamage()
    {
        return weaponDamage;
    }
}
