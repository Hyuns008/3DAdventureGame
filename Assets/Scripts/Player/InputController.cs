using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public class PlayerData
    {
        public int idleChange;
        public float moveSpeed;
        public float maxStamina;
        public float curStamina;
        public float playerMaxExp;
        public int playerCurExp;
        public float levelPoint;
        public int statusPoint;
        public int skillPoint;
        public int weaponLevel;
        public float playerDamage;
        public float playerMaxHp;
        public float playerCurHp;
        public float playerArmor;
        public int weaponNumber;
    }

    private PlayerData playerData = new PlayerData();

    private SaveManager saveManager;
    private GameManager gameManager;

    private CharacterController characterController; //�÷��̾ ������ �ִ� ĳ���� ��Ʈ�ѷ��� �޾ƿ� ����
    private Vector3 moveVec; //�÷��̾��� �Է°��� �޾ƿ� ����

    private Camera mainCam;

    private Animator anim; //�÷��̾��� �ִϸ��̼��� �޾ƿ� ����

    [Header("���� ���� �ҷ�����")]
    [SerializeField] private bool dataLoad = false;

    [Header("�÷��̾� �ִϸ��̼� ����")]
    [SerializeField, Range(0, 1)] private int idleChange;

    [Header("�÷��̾� �̵��ӵ�")]
    [SerializeField] private float moveSpeed;

    [Header("�÷��̾� ���׹̳�")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float curStamina;

    [Header("�÷��̾� ������")]
    [SerializeField, Tooltip("�÷��̾��� ������ ��")] private float diveRollForce;
    [SerializeField, Tooltip("������ ��Ÿ��")] private float diveRollCoolTime;
    private float diveRollTimer; //������ ��Ÿ���� ������ Ÿ�̸� ����
    private bool useDieveRoll = false; //�����⸦ ����ߴ��� üũ�ϱ� ���� ����
    private Vector3 diveVec; //������ �� ȭ���� ȸ���ص� ���� �ٶ�ô� �������� �����⸦ �ϱ� ���� ���� ���� ����
    private bool diveNoHit = false; //������ �� ��� ������ ���� ����

    [Header("�÷��̾� ���� ����")]
    [SerializeField, Tooltip("�÷��̾� ����")] private int playerLevel = 1;
    [SerializeField, Tooltip("�÷��̾� �ִ� ����ġ")] private float playerMaxExp;
    [SerializeField, Tooltip("�÷��̾� ���� ����ġ")] private float playerCurExp;
    private int statusPoint; //�ɷ�ġ�� �ø� �� �ִ� ����Ʈ
    private int skillPoint; //��ų ������ �ø� �� �ִ� ����Ʈ

    //���� ����� ���� ������
    private bool isAttack = false; //������ �ߴ��� ���θ� Ȯ���ϱ� ���� ����
    private float attackTimer; //���� �� �����̸� ���� ���ư��� Ÿ�̸�
    private float attackDelay; //������ �ð�
    private int attackCount; //�� ��° ������ �ߴ��� üũ�ϱ� ���� ����
    private bool attackCombo; //�޺� ������ ���� ����
    private float comboTimer; //�޺� ������ ���� �ð�
    private float changeStaminaAttack; //���ݸ���� �����ϱ� ���� ����
    [Header("�÷��̾� ���� ����")]
    [SerializeField] private Collider hitArea;
    [SerializeField] private float playerDamage = 1;
    [SerializeField, Range(0.0f, 100.0f)] private float playerCritical;
    [SerializeField, Range(0.5f, 4.0f)] private float playerCriticalDamage = 0.5f;
    private float playerAttackDamage; //��� ����Ǿ �� ������ ����
    private bool playerCriticalAttack = false; //�ÿ��̾ ���� �� ũ��Ƽ���� �ߵ��Ǿ�����
    private bool monsterAttack = false; //���͸� �����ϱ� ���� ����

    [Header("�÷��̾� ü�� ���� x = max, y = cur")]
    [SerializeField] private Vector2 playerMaxCurHp;

    [Header("�÷��̾� ���� ����")]
    [SerializeField] private float playerArmor;

    [Header("�÷��̾��� ���� ����")]
    [SerializeField] private Transform playerHandTrs; //�÷��̾��� �� ��ġ
    [SerializeField] private Transform playerBackTrs; //�÷��̾��� �� ��ġ
    [SerializeField] private GameObject weapon; //�÷��̾��� ����
    [SerializeField] private int weaponNumber; //�����ȣ�� �޾ƿ� ���� �� �ҷ����⸦ �ϱ� ���� ����
    private float weaponChangeDelay; //�÷��̾��� �հ� ���⸦ �����ϱ� ���� ������ �ð�
    private bool weaponChange = false; //�÷��̾ ���⸦ �����Ͽ����� üũ�ϱ� ���� ����
    private int weaponLevel; //���� ������ �޾� �� ����

    [Header("�������� �ݱ� ���� �ݶ��̴�")]
    [SerializeField] private BoxCollider pickUpArea;

    [Header("�÷��̾� ���¹�")]
    [SerializeField] private GameObject playerBar;
    private Image playerHpBar; //�÷��̾��� ü�� �̹���
    private Image playerStaminaBar; //�÷��̾��� ���׹̳� �̹���

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        diveRollTimer = diveRollCoolTime;
    }

    private void Start()
    {
        mainCam = Camera.main;

        saveManager = SaveManager.Instance;

        gameManager = GameManager.Instance;

        playerHpBar = playerBar.transform.Find("LayOut/Hp").GetComponent<Image>();

        playerStaminaBar = playerBar.transform.Find("LayOut/Stamina").GetComponent<Image>();

        curStamina = maxStamina;

        playerMaxCurHp.y = playerMaxCurHp.x;
    }

    private void OnTrigger(Collider collision)
    {
        if (collision.gameObject.tag == "Item" && Input.GetKeyDown(KeyCode.E))
        {
            Weapon weaponSc = collision.gameObject.GetComponent<Weapon>();
            weaponNumber = weaponSc.WeaponNumber();
            if (weaponSc.WeaponLevel() <= playerLevel)
            {
                weapon = collision.gameObject;
                playerDamage = (playerDamage + weaponSc.WeaponDamage());
                weapon.transform.SetParent(playerBackTrs.transform);
                weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
                weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster") && monsterAttack == true)
        {
            Monster monsterSc = collision.GetComponent<Monster>();

            if (playerCriticalAttack == true)
            {
                monsterSc.monsterHit(playerAttackDamage + (playerAttackDamage * playerCriticalDamage));
                playerCriticalAttack = false;
            }
            else
            {
                monsterSc.monsterHit(playerAttackDamage);
                playerCriticalAttack = false;
            }
        }
    }

    private void Update()
    {
        playerDataLoad();

        if (gameManager.GetGamePause() == true)
        {
            gameManager.SetGamePause(true);
            return;
        }
        else
        {
            gameManager.SetGamePause(false);
        }

        playerColliderCheck();
        playerTimers();
        playerLookAtScreen();
        playerMove();
        playerStamina();
        playerDiveRoll();
        playerWeaponChange();
        playerAttack();
        playerExpCheck();
        playerLevelUp();
        playerBarCheck();
        playerAnim();
    }

    /// <summary>
    /// �÷��̾��� �����͸� �ҷ����� ���� �Լ�
    /// </summary>
    private void playerDataLoad()
    {

    }

    //#if UNITY_EDITOR//��ó��

    //    [SerializeField] float radius = 1.0f;
    //    [SerializeField] Color lineColor = Color.red;
    //    [SerializeField] bool showLine = false;

    //    private void OnDrawGizmos()
    //    {
    //        if (showLine == true)
    //        {
    //            Handles.color = lineColor;
    //            Handles.DrawWireDisc(transform.position, transform.up, radius);
    //            Handles.color = lineColor;
    //            Handles.DrawWireCube(pickUpArea.bounds.center, pickUpArea.bounds.size);
    //        }
    //    }
    //#endif

    /// <summary>
    /// �������� �ݱ� ���� �Լ�
    /// </summary>
    private void playerColliderCheck()
    {
        Collider[] pickUpColl = Physics.OverlapBox(pickUpArea.bounds.center, pickUpArea.bounds.size * 0.5f, Quaternion.identity,
            LayerMask.GetMask("Weapon"));

        if (idleChange == 0)
        {
            Collider[] attackColl = Physics.OverlapBox(hitArea.bounds.center, hitArea.bounds.size * 0.3f, Quaternion.identity,
                LayerMask.GetMask("Monster"));

            if (attackColl != null)
            {
                int count = attackColl.Length;
                for (int i = 0; i < count; i++)
                {
                    OnTrigger(attackColl[i]);
                }

                monsterAttack = false;
            }
        }
        else
        {
            Collider[] attackColl = Physics.OverlapBox(hitArea.bounds.center, hitArea.bounds.size * 0.5f, Quaternion.identity,
                LayerMask.GetMask("Monster"));

            if (attackColl != null)
            {
                int count = attackColl.Length;
                for (int i = 0; i < count; i++)
                {
                    OnTrigger(attackColl[i]);
                }

                monsterAttack = false;
            }
        }

        if (pickUpColl != null)
        {
            int count = pickUpColl.Length;
            for (int i = 0; i < count; i++)
            {
                OnTrigger(pickUpColl[0]);
            }
        }
    }

    /// <summary>
    /// �÷��̾�� ����Ǵ� Ÿ�̸Ӹ� ����ϴ� �Լ�
    /// </summary>
    private void playerTimers()
    {
        if (useDieveRoll == true)
        {
            diveRollTimer -= Time.deltaTime;
            if (diveRollTimer <= 0f)
            {
                diveRollTimer = diveRollCoolTime;
                diveNoHit = false;
                useDieveRoll = false;
            }
        }

        if (isAttack == true)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                if (attackCount < 1)
                {
                    attackCombo = true;
                }
                attackTimer = 0f;
                isAttack = false;
            }
        }

        if (attackCombo == true)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= 0.3f)
            {
                comboTimer = 0f;
                attackCount = 0;
                attackCombo = false;
            }
        }

        if (weaponChange == true)
        {
            weaponChangeDelay += Time.deltaTime;
            if (weaponChangeDelay >= 1f)
            {
                weaponChangeDelay = 0f;
                weaponChange = false;
            }
        }
    }

    /// <summary>
    /// �÷��̾ ���콺�� ������ ȭ���� �� �� �ְ� ����ϴ� �Լ�
    /// </summary>
    private void playerLookAtScreen()
    {
        if (useDieveRoll == true)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0f, mainCam.transform.eulerAngles.y, 0f);
    }

    /// <summary>
    /// �÷��̾��� �⺻ �̵��� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (useDieveRoll == true)
        {
            if (idleChange == 0)
            {
                characterController.Move(Quaternion.Euler(diveVec) * new Vector3(0f, 0f, diveRollForce * 1.5f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(Quaternion.Euler(diveVec) * new Vector3(0f, 0f, diveRollForce * 1.2f) * Time.deltaTime);
            }
            return;
        }

        if (isAttack == true || (weaponChange == true && weaponChangeDelay <= 0.5f))
        {
            return;
        }

        moveVec = new Vector3(inputHorizontal(), 0f, inputVertical());

        if (inputVertical() < 0)
        {
            if (idleChange == 0)
            {
                characterController.Move(transform.rotation * moveVec * Time.deltaTime);
            }
            else
            {
                characterController.Move(transform.rotation * (moveVec * 0.5f) * Time.deltaTime);
            }
        }
        else if (inputHorizontal() != 0)
        {
            if (idleChange == 0)
            {
                characterController.Move(transform.rotation * (moveVec * 1.2f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(transform.rotation * (moveVec * 0.7f) * Time.deltaTime);
            }
        }
        else
        {
            if (idleChange == 0)
            {
                characterController.Move(transform.rotation * (moveVec * 1.5f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(transform.rotation * (moveVec * 1f) * Time.deltaTime);
            }
        }
    }

    private float inputHorizontal()
    {
        return Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

    private float inputVertical()
    {
        return Input.GetAxisRaw("Vertical") * moveSpeed;
    }

    /// <summary>
    /// �÷��̾��� ���׹̳ʸ� �����ϱ� ���� �Լ�
    /// </summary>
    private void playerStamina()
    {
        if (curStamina < maxStamina)
        {
            curStamina += 10f * Time.deltaTime;
        }
        else if (curStamina > maxStamina)
        {
            curStamina = maxStamina;
        }

        if (curStamina < 0)
        {
            curStamina = 0f;
        }
    }

    /// <summary>
    /// ������(ȸ��)�� ����ϴ� �Լ�
    /// </summary>
    private void playerDiveRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false && curStamina > 30f)
        {
            anim.Play("Unarmed-DiveRoll-Forward1");
            diveVec = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            curStamina -= 30;
            diveNoHit = true;
            useDieveRoll = true;
        }
    }

    /// <summary>
    /// �÷��̾ ����� ���� �����ϱ� ���� �Լ�
    /// </summary>
    private void playerWeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Q) && weapon != null && weaponChange == false)
        {
            if (idleChange == 0)
            {
                idleChange = 1;
                weapon.transform.position = playerHandTrs.transform.position;
                weapon.transform.rotation = playerHandTrs.transform.rotation;
                weapon.transform.SetParent(playerHandTrs.transform);
                anim.Play("ChangeWeapon");
            }
            else
            {
                idleChange = 0;
                weapon.transform.position = playerBackTrs.transform.position;
                weapon.transform.rotation = playerBackTrs.transform.rotation;
                weapon.transform.SetParent(playerBackTrs.transform);
                anim.Play("ChangeHand");
            }

            weaponChange = true;
        }
    }

    /// <summary>
    /// �÷��̾��� ������ ����ϴ� �Լ�
    /// </summary>
    private void playerAttack()
    {
        if (useDieveRoll == true)
        {
            isAttack = false;
            attackDelay = 0f;
            attackCount = 0;
            attackCombo = false;
            attackTimer = 0f;
            comboTimer = 0f;
            return;
        }

        if (isAttack == false && attackCombo == false && attackCount > 0)
        {
            attackCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isAttack == false)
        {
            if (idleChange == 0)
            {
                if (attackCombo == true)
                {
                    attackCount = 1;
                    comboTimer = 0f;
                    attackCombo = false;
                }

                if (attackCount == 0)
                {
                    playerAttackDamage = playerDamage;
                }
                else
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.5f);
                }

                float critical = Random.Range(0.0f, 100.0f);

                if (critical <= playerCritical)
                {
                    playerCriticalAttack = true;
                }
                else
                {
                    playerCriticalAttack = false;
                }

                anim.Play("Attack Tree");
                attackDelay = 1f;
                isAttack = true;
            }
            else
            {
                changeStaminaAttack = 0;

                if (attackCombo == true)
                {
                    attackCount = 1;
                    comboTimer = 0f;
                    attackCombo = false;
                }

                if (attackCount == 0)
                {
                    playerAttackDamage = playerDamage;
                }
                else
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.5f);
                }

                float critical = Random.Range(0.0f, 100.0f);

                if (critical <= playerCritical)
                {
                    playerCriticalAttack = true;
                }
                else
                {
                    playerCriticalAttack = false;
                }

                anim.Play("Attack Tree");
                attackDelay = 1f;
                isAttack = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && isAttack == false
            && idleChange == 1 && curStamina > 30f)
        {
            changeStaminaAttack = 1;

            if (attackCombo == true)
            {
                attackCount = 1;
                comboTimer = 0f;
                attackCombo = false;
            }

            if (attackCount == 0)
            {
                playerAttackDamage = playerDamage + (playerDamage * 0.7f);
            }
            else
            {
                playerAttackDamage = playerDamage + (playerDamage * 0.9f);
            }

            float critical = Random.Range(0.0f, 100.0f);

            if (critical <= playerCritical)
            {
                playerCriticalAttack = true;
            }
            else
            {
                playerCriticalAttack = false;
            }

            anim.Play("Attack Tree");
            attackDelay = 1f;
            isAttack = true;

            curStamina -= 30f;
        }
    }

    /// <summary>
    /// ����ġ�� �����ϴ��� �� �ϴ��� üũ�ϰ� �޾ƿ��� ���� �Լ�
    /// </summary>
    private void playerExpCheck()
    {
        if (gameManager.GetExp() > 0)
        {
            playerCurExp += gameManager.GetExp();
            gameManager.SetExp(-gameManager.GetExp());
        }
    }

    /// <summary>
    /// �÷��̾ ���� ����ġ �̻� �׿��� �� �������ϱ� ���� �Լ�
    /// </summary>
    private void playerLevelUp()
    {
        if (playerMaxExp <= playerCurExp)
        {
            playerCurExp -= playerMaxExp;
            playerLevel++;
            statusPoint += 3;
            skillPoint += 3;
            playerMaxExp *= 1.3f;
        }
    }

    /// <summary>
    /// �÷��̾ ü��  �Ǵ� ���׹̳ʰ� ����� �� üũ���ֱ� ���� �Լ�
    /// </summary>
    private void playerBarCheck()
    {
        if (curStamina != 100)
        {
            playerStaminaBar.fillAmount = curStamina / 100;
        }
    }

    /// <summary>
    /// �÷��̾��� �⺻���� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void playerAnim()
    {
        anim.SetFloat("VeticalMove", inputVertical());
        anim.SetFloat("HorizontalMove", inputHorizontal());
        anim.SetFloat("IdleChange", idleChange);
        anim.SetFloat("ChangeAttack", idleChange);
        anim.SetFloat("AttackCount", attackCount);
        anim.SetFloat("StaminaAttack", changeStaminaAttack);
    }

    public void AttackHit()
    {
        monsterAttack = true;
    }
}
