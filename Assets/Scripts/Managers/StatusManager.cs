using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance;

    public class StatusData
    {
        public float damage;
        public float attackSpeed;
        public float speed;
        public float hp;
        public float armor;
        public float critical;
        public float criticalDamage;
        public float stamina;
        public int statPoint;
    }

    private StatusData statusData = new StatusData();

    [Header("�������ͽ� ����")]
    [SerializeField, Tooltip("�������ͽ� ���ݷ�")] private float damage;
    [SerializeField, Tooltip("�������ͽ� ���ݼӵ�")] private float attackSpeed;
    [SerializeField, Tooltip("�������ͽ� �̵��ӵ�")] private float speed;
    [SerializeField, Tooltip("�������ͽ� �ִ�ü��")] private float hp;
    [SerializeField, Tooltip("�������ͽ� ����")] private float armor;
    [SerializeField, Tooltip("�������ͽ� ġ��ŸȮ��")] private float critical;
    [SerializeField, Tooltip("�������ͽ� ġ��Ÿ���ݷ�")] private float criticalDamage;
    [SerializeField, Tooltip("�������ͽ� ���")] private float stamina;
    [SerializeField, Tooltip("���� ����Ʈ")] private int statPoint;
    [Space]
    [SerializeField, Tooltip("�������ͽ��� ������ �� ī�޶� �������� ����")] private GameObject cameraObj;
    [Space]
    [SerializeField, Tooltip("�������ͽ�")] private GameObject statusObj;
    private bool statusOnOffCheck = false; //�������ͽ��� �������� �������� üũ�ϱ� ���� ����
    [SerializeField, Tooltip("���� ��� ��ư")] private List<Button> statUpButtons;
    private bool statUpCheck; //������ �ö����� üũ�ϴ� ����
    private List<int> statIndex = new List<int>(); //������ �󸶳� ����ߴ��� �����ϴ� ����Ʈ
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
            if (statPoint == 0 || statIndex[0] >= 100)
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
            if (statPoint == 0 || statIndex[2] >= 100)
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
            if (statPoint <= 0 || statIndex[3] >= 100)
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
            if (statPoint <= 0 || statIndex[4] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[4]++;
            critical += 0.5f;
            criticalDamage += 0.05f;
            statPoint--;
        });

        //���׹̳ʸ� ��½�Ű�� ��ư
        statUpButtons[5].onClick.AddListener(() =>
        {
            if (statPoint <= 0 || statIndex[5] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[5]++;
            stamina += 10;
            statPoint--;
        });

        if (PlayerPrefs.GetString("saveStatusData") != string.Empty)
        {
            string getSaveStat = PlayerPrefs.GetString("saveStatusData");
            statusData = JsonConvert.DeserializeObject<StatusData>(getSaveStat);
            getSaveStatus(statusData);
        }
        else
        {
            statusData.damage = 1f;
            statusData.attackSpeed = 1f;
            statusData.speed = 4f;
            statusData.hp = 100f;
            statusData.armor = 0f;
            statusData.critical = 0f;
            statusData.criticalDamage = 0.5f;
            statusData.stamina = 100f;
            statusData.statPoint = 3;
        }
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
            statusOnOffCheck = statusObj == statusObj.activeSelf ? false : true;
            statusObj.SetActive(statusOnOffCheck);
            cameraObj.SetActive(!statusOnOffCheck);

            if (statusOnOffCheck == true)
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
            setSaveStatus();
            statUpCheck = false;
        }

        statText[6].text = $"���� ����Ʈ : {statPoint}";
    }

    /// <summary>
    /// �������ͽ��� �����ϴ� �Լ�
    /// </summary>
    private void setSaveStatus()
    {
        statusData.damage = damage;
        statusData.attackSpeed = attackSpeed;
        statusData.speed = speed;
        statusData.hp = hp;
        statusData.armor = armor;
        statusData.critical = critical;
        statusData.criticalDamage = criticalDamage;
        statusData.stamina = stamina;
        statusData.statPoint = statPoint;

        string setSaveStat = JsonConvert.SerializeObject(statusData);
        PlayerPrefs.SetString("saveStatusData", setSaveStat);
    }

    /// <summary>
    /// ����� �������ͽ� �����͸� �޾ƿ��� �Լ�
    /// </summary>
    /// <param name="_statusData"></param>
    private void getSaveStatus(StatusData _statusData)
    {
        damage = _statusData.damage;
        attackSpeed = _statusData.attackSpeed;
        speed = _statusData.speed;
        hp = _statusData.hp;
        armor = _statusData.armor;
        critical = _statusData.critical;
        criticalDamage = _statusData.criticalDamage;
        stamina = _statusData.stamina;
        statPoint = _statusData.statPoint;
    }

    /// <summary>
    /// �������ͽ� �� ������ üũ�ϴ� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetStatusOnOff()
    {
        return statusOnOffCheck;
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
        return critical;
    }

    /// <summary>
    /// ġ��Ÿ���ݷ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCriticalDamage()
    {
        return criticalDamage;
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
