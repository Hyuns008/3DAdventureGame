using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("���� ����")]
    [SerializeField] private bool gamePause;

    private float playerExp; //�÷��̾�� ������ ����ġ

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
    /// �ٸ� ��ũ��Ʈ�� ���� ������ ���߰� �ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_gamePause"></param>
    public void SetGamePause(bool _gamePause)
    {
        gamePause = _gamePause;

        if (gamePause == true)
        {
            Time.timeScale = 0.0f;
        }
        else 
        {
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ������ ������� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetGamePause()
    {
        return gamePause;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ����ġ�� �޾ƿ� �Լ�
    /// </summary>
    /// <param name="_exp"></param>
    public void SetExp(float _exp)
    {
        playerExp += _exp;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ����ġ�� ������ ����
    /// </summary>
    /// <returns></returns>
    public float GetExp()
    {
        return playerExp;
    }
}
