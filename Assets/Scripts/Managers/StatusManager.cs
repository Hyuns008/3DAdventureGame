using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance;

    [Header("�������ͽ� ����")]
    [SerializeField, Tooltip("�������ͽ� ���ݷ�")] private float damage;
    [SerializeField, Tooltip("�������ͽ� ���ݼӵ�")] private float attackSpeed;
    [SerializeField, Tooltip("�������ͽ� �̵��ӵ�")] private float speed;
    [SerializeField, Tooltip("�������ͽ� �ִ�ü��")] private float hp;
    [SerializeField, Tooltip("�������ͽ� ����")] private float armor;
    [SerializeField, Tooltip("�������ͽ� ġ��ŸȮ��")] private float criitical;
    [SerializeField, Tooltip("�������ͽ� ġ��Ÿ���ݷ�")] private float criiticalDamage;
    [SerializeField, Tooltip("�������ͽ� ���")] private float stamina;
    [Space]
    [SerializeField, Tooltip("�������ͽ��� ������ �� ī�޶� �������� ����")] private GameObject cameraObj;
    [Space]
    [SerializeField, Tooltip("�������ͽ�")] private GameObject statusObj;
    [SerializeField, Tooltip("���� ��� ��ư")] private List<Button> statUpButtons;
    private bool statUpCheck; //������ �ö����� üũ�ϴ� ����
    private List<int> statIndex = new List<int>(); //������ �󸶳� ����ߴ��� �����ϴ� ����Ʈ
    private int statPoint = 3; //���� ����Ʈ
    [Space]
    [SerializeField, Tooltip("��ų ��� ��ư")] private List<Button> skillUpButtons;
    [Space]
    [SerializeField, Tooltip("������ ǥ���� �ؽ�Ʈ")] private List<TMP_Text> statText;

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

        statusObj.SetActive(false);

        int count = statUpButtons.Count;
        for (int i = 0; i < count; i++)
        {
            statIndex.Add(0);
        }

        //���ݷ��� ��½�Ű�� ��ư
        statUpButtons[0].onClick.AddListener(() => 
        {
            if (statPoint == 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[0]++;
            damage += 1;
            statPoint--;
        });

        //���ݼӵ��� �̵��ӵ��� ��½�Ű�� ��ư
        statUpButtons[1].onClick.AddListener(() =>
        {
            if (statPoint == 0 || statIndex[1] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[1]++;
            attackSpeed += 0.003f;
            speed += 0.01f;
            statPoint--;
        });

        //ü���� ��½�Ű�� ��ư
        statUpButtons[2].onClick.AddListener(() =>
        {
            if (statPoint == 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[2]++;
            hp += 10;
            statPoint--;
        });

        //������ ��½�Ű�� ��ư
        statUpButtons[3].onClick.AddListener(() =>
        {
            if (statPoint <= 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[3]++;
            armor += 1;
            statPoint--;
        });

        //ũ��Ƽ�� ���� �ɷ�ġ�� ��½�Ű�� ��ư
        statUpButtons[4].onClick.AddListener(() =>
        {
            if (statPoint <= 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[4]++;
            criitical += 0.5f;
            criiticalDamage += 0.05f;
            statPoint--;
        });

        //���׹̳ʸ� ��½�Ű�� ��ư
        statUpButtons[5].onClick.AddListener(() =>
        {
            if (statPoint <= 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[5]++;
            stamina += 10;
            statPoint--;
        });
    }

    private void Start()
    {
        statUpCheck = true;
    }

    private void Update()
    {
        statusOnOff();
        statusStatUI();
    }

    /// <summary>
    /// �������ͽ��� ���� Ű�� ���ִ� �Լ�
    /// </summary>
    private void statusOnOff()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            bool onOffCheck = statusObj == statusObj.activeSelf ? false : true;
            statusObj.SetActive(onOffCheck);
            cameraObj.SetActive(!onOffCheck);

            if (onOffCheck == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    /// <summary>
    /// ������ �������ͽ�UI�� ǥ���ϱ� ���� �۵��ϴ� �Լ�
    /// </summary>
    private void statusStatUI()
    {
        if (statUpCheck == true)
        {
            statText[0].text = $"STR : {statIndex[0]}";
            statText[1].text = $"DEX : {statIndex[1]}";
            statText[2].text = $"HP : {statIndex[2]}";
            statText[3].text = $"AMR : {statIndex[3]}";
            statText[4].text = $"LUK : {statIndex[4]}";
            statText[5].text = $"SP : {statIndex[5]}";
            statUpCheck = false;
        }

        statText[6].text = $"���� ����Ʈ : {statPoint}";
    }

    /// <summary>
    /// ���ݷ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatDamage()
    {
        return damage;
    }

    /// <summary>
    /// ���ݼӵ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatAttackSpeed()
    {
        return attackSpeed - 1f;
    }

    /// <summary>
    /// �÷��̾� ���� �ִϸ��̼ǿ� ���ݼӵ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatAttackSpeedAnim()
    {
        return attackSpeed;
    }

    /// <summary>
    /// �̵��ӵ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatSpeed()
    {
        return speed;
    }

    /// <summary>
    /// ü���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatHp()
    {
        return hp;
    }

    /// <summary>
    /// ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatArmor()
    {
        return armor;
    }

    /// <summary>
    /// ġ��ŸȮ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCritical()
    {
        return criitical;
    }

    /// <summary>
    /// ġ��Ÿ���ݷ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCriticalDamage()
    {
        return criiticalDamage;
    }

    /// <summary>
    /// ���׹̳ʸ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatStamina()
    {
        return stamina;
    }

    /// <summary>
    /// ��������Ʈ�� �޾�
    /// </summary>
    /// <param name="_statPoint"></param>
    public void SetStatPoint(int _statPoint)
    {
        statPoint += _statPoint;
    }

    /// <summary>
    /// ������ ��½��״��� üũ�ϴ� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetBoolTest()
    {
        return statUpCheck;
    }
}
