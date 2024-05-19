using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [Header("ĵ����")]
    [SerializeField] private Canvas canvas;

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

    /// <summary>
    /// ĵ���� �Ŵ����� ���� ĵ������ �ٸ� ���� �����ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public Canvas GetCanvas()
    {
        return canvas;
    }
}
