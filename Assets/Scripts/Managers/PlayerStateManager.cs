using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    [Header("�÷��̾� ���¸� �����ִ� UI")]
    [SerializeField, Tooltip("ȭ�鿡 ����� ���� �ؽ�Ʈ")] private TMP_Text playerLevelText;
    [SerializeField, Tooltip("ȭ�鿡 ����� ü�¹�")] private Image playerHpBar;
    [SerializeField, Tooltip("ȭ�鿡 ����� ���׹̳ʹ�")] private Image playerStaminaBar;
    [SerializeField, Tooltip("ȭ�鿡 ����� ����ġ��")] private Image playerExpBar;

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
    /// �÷��̾��� ������ �޾ƿ� �ؽ�Ʈ�� ǥ�����ֱ� ���� �Լ�
    /// </summary>
    /// <param name="_playerLevel"></param>
    public void SetPlayerLevelText(float _playerLevel)
    {
        playerLevelText.text = $"{_playerLevel}";
    }

    /// <summary>
    /// �÷��̾��� ���� ���׹̳� �ִ� ���׹̳ʸ� ���� �̹����� ǥ���� �� �ְ� ���ִ� �Լ�
    /// </summary>
    /// <param name="_curStamina"></param>
    /// <param name="_maxStamina"></param>
    public void SetPlayerHpBar(float _curHp, float _maxHp)
    {
        playerHpBar.fillAmount = _curHp / _maxHp;
    }

    /// <summary>
    /// �÷��̾��� ���� ���׹̳� �ִ� ���׹̳ʸ� ���� �̹����� ǥ���� �� �ְ� ���ִ� �Լ�
    /// </summary>
    /// <param name="_curStamina"></param>
    /// <param name="_maxStamina"></param>
    public void SetPlayerStaminaBar(float _curStamina, float _maxStamina)
    {
        playerStaminaBar.fillAmount = _curStamina / _maxStamina;
    }

    /// <summary>
    /// �÷��̾��� ���� ����ġ�� �ִ� ����ġ�� ���� �̹����� ǥ���� �� �ְ� ���ִ� �Լ�
    /// </summary>
    /// <param name="_playerCurExp"></param>
    /// <param name="_playerMaxExp"></param>
    public void SetPlayerExpBar(float _playerCurExp, float _playerMaxExp)
    {
        playerExpBar.fillAmount = _playerCurExp / _playerMaxExp;
    }
}
