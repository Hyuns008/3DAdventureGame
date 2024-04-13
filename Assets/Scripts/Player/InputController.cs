using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameManager gameManager;
    private InformationManager InformationManager;
    private PlayerStateManager playerStateManager;

    private CharacterController characterController; //�÷��̾ ������ �ִ� ĳ���� ��Ʈ�ѷ��� �޾ƿ� ����
    private Vector3 moveVec; //�÷��̾��� �Է°��� �޾ƿ� ����

    private Camera mainCam;

    private Animator anim; //�÷��̾��� �ִϸ��̼��� �޾ƿ� ����

    [Header("�÷��̾� �ִϸ��̼� ����")]
    [SerializeField, Range(0, 1)] private int idleChange;

    [Header("�÷��̾� �߷�")]
    [SerializeField] private float gravity;

    [Header("�÷��̾� �̵��ӵ�")]
    [SerializeField] private float moveSpeed;

    [Header("�÷��̾� ���׹̳�")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float curStamina;

    [Header("�÷��̾� ������")]
    [SerializeField, Tooltip("������ ��Ÿ��")] private float diveRollCoolTime;
    private float diveRollTimer; //������ ��Ÿ���� ������ Ÿ�̸� ����
    private bool useDieveRoll = false; //�����⸦ ����ߴ��� üũ�ϱ� ���� ����
    private Vector3 diveVec; //������ �� ȭ���� ȸ���ص� ���� �ٶ�ô� �������� �����⸦ �ϱ� ���� ���� ���� ����
    private bool diveNoHit = false; //������ �� ��� ������ ���� ����

    //���� ����� ���� ������
    private bool isAttack = false; //������ �ߴ��� ���θ� Ȯ���ϱ� ���� ����
    private float attackTimer; //���� �� �����̸� ���� ���ư��� Ÿ�̸�
    private float attackDelay; //������ �ð�
    private int attackCount; //�� ��° ������ �ߴ��� üũ�ϱ� ���� ����
    private bool attackCombo; //�޺� ������ ���� ����
    private float comboTimer; //�޺� ������ ���� �ð�
    private float changeStaminaAttack; //���ݸ���� �����ϱ� ���� ����
    [Header("�÷��̾� ���� ����")]
    [SerializeField] private List<Collider> hitArea;
    [SerializeField] private float playerDamage;
    [SerializeField] private float playerAttackSpeed;
    [SerializeField, Range(0.0f, 100.0f)] private float playerCritical;
    [SerializeField, Range(0.0f, 10.0f)] private float playerCriticalDamage;
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
    private float weaponDamage;
    private float weaponAttackSpeed;

    [Header("�������� �ݱ� ���� �ݶ��̴�")]
    [SerializeField] private BoxCollider pickUpArea;

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

        gameManager = GameManager.Instance;

        InformationManager = InformationManager.Instance;

        playerStateManager = PlayerStateManager.Instance;

        curStamina = maxStamina;

        playerMaxCurHp.y = playerMaxCurHp.x;

        playerStatusCheck();
    }

    private void Update()
    {
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
        playerGravity();
        playerMove();
        playerStamina();
        playerDiveRoll();
        playerWeaponChange();
        playerAttack();
        playerBarCheck();

        if (InformationManager.GetBoolTest() == true)
        {
            playerStatusCheck();
        }

        playerAnim();
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

    private void OnTrigger(Collider collision)
    {
        if (InformationManager.GetStatusOnOff() == true)
        {
            return;
        }

        if (collision.gameObject.tag == "Item" && Input.GetKeyDown(KeyCode.E))
        {
            if (weapon == null)
            {
                Weapon weaponSc = collision.gameObject.GetComponent<Weapon>();
                Rigidbody weaponRigid = collision.gameObject.GetComponent<Rigidbody>();
                BoxCollider weaponColl = collision.gameObject.GetComponent<BoxCollider>();
                weaponNumber = weaponSc.WeaponNumber();
                if (weaponSc.WeaponLevel() <= InformationManager.GetLevel())
                {
                    weaponRigid.isKinematic = true;
                    weaponColl.isTrigger = true;
                    weapon = collision.gameObject;
                    weaponDamage = weaponSc.WeaponDamage();   
                    weaponAttackSpeed = weaponSc.WeaponAttackSpeed();
                    playerDamage = InformationManager.GetPlayerStatDamage() + weaponDamage;
                    playerAttackSpeed = InformationManager.GetPlayerStatAttackSpeedAnim()
                        + (InformationManager.GetPlayerStatAttackSpeedAnim() * weaponAttackSpeed);
                    weapon.transform.SetParent(playerBackTrs.transform);
                    weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
                    weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster") && monsterAttack == true)
        {
            Monster monsterSc = collision.GetComponent<Monster>();

            if (playerCriticalAttack == true)
            {
                monsterSc.monsterHit(playerAttackDamage + (playerAttackDamage * playerCriticalDamage),
                    Color.red);
                playerCriticalAttack = false;
            }
            else
            {
                monsterSc.monsterHit(playerAttackDamage, Color.white);
                playerCriticalAttack = false;
            }
        }
    }

    /// <summary>
    /// �������� �ݱ� ���� �Լ�
    /// </summary>
    private void playerColliderCheck()
    {
        Collider[] pickUpColl = Physics.OverlapBox(pickUpArea.bounds.center, pickUpArea.bounds.size * 0.5f, Quaternion.identity,
            LayerMask.GetMask("Weapon"));

        if (idleChange == 0)
        {
            Collider[] attackColl = Physics.OverlapBox(hitArea[0].bounds.center, hitArea[0].bounds.size * 0.5f, Quaternion.identity,
                LayerMask.GetMask("Monster"));

            int attackCount = attackColl.Length;

            if (attackCount > 0)
            {
                for (int i = 0; i < attackCount; i++)
                {
                    OnTrigger(attackColl[i]);
                }

                monsterAttack = false;
            }
        }
        else
        {
            Collider[] attackColl = Physics.OverlapBox(hitArea[1].bounds.center, hitArea[1].bounds.size * 0.5f, Quaternion.identity,
                LayerMask.GetMask("Monster"));

            int attackCount = attackColl.Length;

            if (attackCount > 0)
            {
                for (int i = 0; i < attackCount; i++)
                {
                    OnTrigger(attackColl[i]);
                }

                monsterAttack = false;
            }
        }

        int pickUpCount = pickUpColl.Length;

        if (pickUpCount > 0)
        {
            for (int i = 0; i < pickUpCount; i++)
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

                monsterAttack = false;

                attackTimer = 0f;
                isAttack = false;
            }
        }

        if (attackCombo == true)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= 0.3f - (0.3f * (playerAttackSpeed - 1)))
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
        if (useDieveRoll == true || isAttack == true || InformationManager.GetStatusOnOff() == true)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0f, mainCam.transform.eulerAngles.y, 0f);
    }

    /// <summary>
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
    /// </summary>
    private void playerGravity()
    {
        if (characterController.isGrounded == false)
        {
            characterController.Move(new Vector3(0f, -gravity, 0f) * Time.deltaTime);
        }
        else
        {
            characterController.Move(new Vector3(0f, 0f, 0f));
        }
    }

    /// <summary>
    /// �÷��̾��� �⺻ �̵��� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (InformationManager.GetStatusOnOff() == true)
        {
            return;
        }

        if (useDieveRoll == true)
        {
            if (idleChange == 0)
            {
                characterController.Move(Quaternion.Euler(diveVec) * new Vector3(0f, 0f, (moveSpeed + 1) * 1.5f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(Quaternion.Euler(diveVec) * new Vector3(0f, 0f, (moveSpeed + 1) * 1.2f) * Time.deltaTime);
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
        if (InformationManager.GetStatusOnOff() == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false && curStamina > 20f)
        {
            anim.Play("Unarmed-DiveRoll-Forward1");
            diveVec = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            curStamina -= 20;
            diveNoHit = true;
            useDieveRoll = true;
        }
    }

    /// <summary>
    /// �÷��̾ ����� ���� �����ϱ� ���� �Լ�
    /// </summary>
    private void playerWeaponChange()
    {
        if (InformationManager.GetStatusOnOff() == true)
        {
            return;
        }

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
        if (InformationManager.GetStatusOnOff() == true)
        {
            return;
        }

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
                attackDelay = 1f - (1f * (playerAttackSpeed - 1));
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
                    playerAttackDamage = playerDamage + (playerDamage * 0.3f);
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
                attackDelay = 1f - (1f * (playerAttackSpeed - 1));
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
            attackDelay = 1f - (1f * (playerAttackSpeed - 1));
            isAttack = true;

            curStamina -= 30f;
        }
    }

    /// <summary>
    /// �÷��̾ ü��  �Ǵ� ���׹̳ʰ� ����� �� üũ���ֱ� ���� �Լ�
    /// </summary>
    private void playerBarCheck()
    {
        if (curStamina != maxStamina)
        {
            playerStateManager.SetPlayerStaminaBar(curStamina, maxStamina);
        }

        //playerHpValue.text = $"{playerMaxCurHp.y.ToString("F0")} / {playerMaxCurHp.x.ToString("F0")}";

        //playerStaminaValue.text = $"{curStamina.ToString("F0")} / {maxStamina.ToString("F0")}";
    }

    /// <summary>
    /// �������ͽ� �Ŵ����� �ҷ��� �÷��̾��� �����͸� �־� �ִ� �Լ�
    /// </summary>
    private void playerStatusCheck()
    {
        playerDamage = InformationManager.GetPlayerStatDamage() + weaponDamage;
        playerAttackSpeed = InformationManager.GetPlayerStatAttackSpeedAnim();
        moveSpeed = InformationManager.GetPlayerStatSpeed();
        playerMaxCurHp = new Vector2(InformationManager.GetPlayerStatHp(), playerMaxCurHp.y);
        playerArmor = InformationManager.GetPlayerStatArmor();
        playerCritical = InformationManager.GetPlayerStatCritical();
        playerCriticalDamage = InformationManager.GetPlayerStatCriticalDamage();
        maxStamina = InformationManager.GetPlayerStatStamina();
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
        anim.SetFloat("AttackSpeed", playerAttackSpeed);
    }

    public void AttackHit()
    {
        monsterAttack = true;
    }
}
