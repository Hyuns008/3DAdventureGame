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

    private void Start()
    {
        randomWeaponOption();
    }

    private void randomWeaponOption()
    {
        if (type.ToString() == "Common")
        {
            weaponLevel = 1;
            float randomDamage = Random.Range(1f, 5f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);
            weaponDamage = dmamge;
        }
        else if (type.ToString() == "Rare")
        {
            weaponLevel = 5;
            float randomDamage = Random.Range(8f, 15f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);
            weaponDamage = dmamge;
        }
        else if (type.ToString() == "Epic")
        {
            weaponLevel = 15;
            float randomDamage = Random.Range(20f, 30f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);
            weaponDamage = dmamge;
        }
        else if (type.ToString() == "Legendary")
        {
            weaponLevel = 30;
            float randomDamage = Random.Range(40f, 60f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);
            weaponDamage = dmamge;
        }
        else if (type.ToString() == "Mythology")
        {
            weaponLevel = 50;
            weaponDamage = 100f;
        }
    }

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
