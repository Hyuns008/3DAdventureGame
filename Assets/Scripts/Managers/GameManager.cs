using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("���� ����")]
    [SerializeField] private bool gamePause;

    [Header("����� ī�޶�")]
    [SerializeField] private GameObject cameraObj;

    private float playerExp; //�÷��̾�� ������ ����ġ

    private bool playerStop = false; //�÷��̾ ���߰� �ϴ� ����

    //UIâ�� ������ �� ���콺���� ����� Ǯ������ ������ֱ� ���� ������
    private bool inforCheck = false;
    private bool invenCheck = false;
    private bool storeCheck = false;
    private bool upgradeCheck = false;

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

    /// <summary>
    /// Ư�� â�� ������ �� ȭ���� �������� �ʰ� ���ִ� �Լ�
    /// </summary>
    /// <param name="_cameraOnOff"></param>
    public void SetCameraMoveStop(bool _cameraOnOff)
    {
        cameraObj.SetActive(_cameraOnOff);
    }

    /// <summary>
    /// �÷��̾��� �������� ���߰� bool ���� �ִ� �Լ�
    /// </summary>
    /// <param name="_moveStop"></param>
    public void SetPlayerMoveStop(bool _moveStop)
    {
        playerStop = _moveStop;
    }

    /// <summary>
    /// �÷��̾��� �������� ���߰� �ϴ� bool ���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerMoveStop()
    {
        return playerStop;
    }

    /// <summary>
    /// UI�� ���������� ���콺�� ����� Ǯ���ִ� �Լ�
    /// </summary>
    /// <param name="_number">1�� ����â, 2�� �κ�â, 3�� ����â, 4�� ��ȭâ, 5�� ����â</param>
    public void SetUIOpenCheck(int _number, bool _check)
    {
        if (_number == 1)
        {
            inforCheck = _check;
        }
        else if (_number == 2) 
        {
            invenCheck = _check;
        }
        else if (_number == 3)
        {
            storeCheck = _check;
        }
        else if (_number == 4)
        {
            upgradeCheck = _check;
        }
        else if (_number == 5)
        {
            return;
        }
    }

    /// <summary>
    /// UI�� ���ȴ��� �������� üũ�ϴ� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="_number">1�� ����â, 2�� �κ�â, 3�� ����â, 4�� ��ȭâ, 5�� ����â</param>
    public bool SetUIOpenCheck(int _number)
    {
        if (_number == 1)
        {
            return inforCheck;
        }
        else if (_number == 2)
        {
            return invenCheck;
        }
        else if (_number == 3)
        {
            return storeCheck;
        }
        else if (_number == 4)
        {
            return upgradeCheck;
        }
        else if (_number == 5)
        {

        }

        return false;
    }

    /// <summary>
    /// ���콺 �������� ����� üũ�ϴ� �Լ�
    /// </summary>
    public void MousePonterLockCheck()
    {
        if (inforCheck == false && invenCheck == false && storeCheck == false && upgradeCheck == false)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
