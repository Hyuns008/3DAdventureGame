using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private CharacterController characterController; //�÷��̾ ������ �ִ� ĳ���� ��Ʈ�ѷ��� �޾ƿ� ����
    private Vector3 moveVec; //�÷��̾��� �Է°��� �޾ƿ� ����

    private Camera mainCam;

    private Animator anim; //�÷��̾��� �ִϸ��̼��� �޾ƿ� ����

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

    //���� ����� ���� ������
    private bool isAttack = false; //������ �ߴ��� ���θ� Ȯ���ϱ� ���� ����
    private float attackTimer; //���� �� �����̸� ���� ���ư��� Ÿ�̸�
    private float attackDelay; //������ �ð�
    private int attackCount; //�� ��° ������ �ߴ��� üũ�ϱ� ���� ����
    private bool attackCombo; //�޺� ������ ���� ����
    private float comboTimer; //�޺� ������ ���� �ð�
    private float changeStaminaAttack; //���ݸ���� �����ϱ� ���� ����

    [Header("�÷��̾��� ���� ����")]
    [SerializeField] private Transform playerHandTrs; //�÷��̾��� �� ��ġ
    [SerializeField] private Transform playerBackTrs; //�÷��̾��� �� ��ġ
    [SerializeField] private GameObject weapon; //�÷��̾��� ����
    private float weaponChangeDelay; //�÷��̾��� �հ� ���⸦ �����ϱ� ���� ������ �ð�
    private bool weaponChange = false; //�÷��̾ ���⸦ �����Ͽ����� üũ�ϱ� ���� ����

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

        curStamina = maxStamina;
    }

    private void Update()
    {
        playerTimers();
        playerLookAtScreen();
        playerMove();
        playerStamina();
        playerDiveRoll();
        playerWeaponChange();
        playerAttack();
        playerAnim();
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
        transform.rotation = Quaternion.Euler(0f, mainCam.transform.eulerAngles.y, 0f);
    }

    /// <summary>
    /// �÷��̾��� �⺻ �̵��� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (useDieveRoll == true)
        {
            characterController.Move(transform.rotation * new Vector3(0f, 0f, diveRollForce) * Time.deltaTime);
            return;
        }

        if (isAttack == true)
        {
            return;
        }

        moveVec = new Vector3(inputHorizontal(), 0f, inputVertical());

        if (inputVertical() < 0)
        {
            characterController.Move(transform.rotation * moveVec * Time.deltaTime * 0.5f);
        }
        else if (inputHorizontal() != 0)
        {
            characterController.Move(transform.rotation * moveVec * Time.deltaTime * 0.7f);
        }
        else
        {
            characterController.Move(transform.rotation * moveVec * Time.deltaTime);
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
            Debug.Log("������");
            anim.Play("Unarmed-DiveRoll-Forward1");
            curStamina -= 30;
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
            }
            else
            {
                idleChange = 0;
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
            isAttack = true;
            attackDelay = 0.3f;
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

            anim.Play("Attack Tree");
            attackDelay = 1f;
            isAttack = true;

            curStamina -= 30f;
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
}
