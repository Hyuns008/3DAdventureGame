using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNpc : MonoBehaviour
{
    private InventoryManger inventoryManger;

    [Header("��ȭ Npc ����")]
    [SerializeField] private GameObject upgradeUI;

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;
    }
}
