using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField, Tooltip("������ ����")] private int coin;

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <returns></returns>
    public int SetCoin()
    {
        return coin;
    }
}
