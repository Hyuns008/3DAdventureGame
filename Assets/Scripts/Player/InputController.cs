using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private CharacterController characterController; //�÷��̾ ������ �ִ� ĳ���� ��Ʈ�ѷ��� �޾ƿ� ����
    private Vector3 moveVec; //�÷��̾��� �Է°��� �޾ƿ� ����

    private Animator anim; //�÷��̾��� �ִϸ��̼��� �޾ƿ� ����

    [Header("�÷��̾� ����")]
    [SerializeField, Range(0, 1), Tooltip("Idle �ִϸ��̼� ����")] private int idleChange;
    [Space]
    [SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")] private float moveSpeed;
    [Space]
    [SerializeField, Tooltip("������ ��Ÿ��")] private float diveRollCoolTime;
    private float diveRollTimer; //������ ��Ÿ���� ������ Ÿ�̸� ����
    private bool useDieveRoll = false; //�����⸦ ����ߴ��� üũ�ϱ� ���� ����

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        playerTimers();
        playerMove();
        playerDiveRoll();
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
    }

    /// <summary>
    /// �÷��̾��� �⺻ �̵��� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (useDieveRoll == true)
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
    /// ������(ȸ��)�� ����ϴ� �Լ�
    /// </summary>
    private void playerDiveRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false)
        {
            anim.Play("Unarmed-DiveRoll-Forward1");
            useDieveRoll = true;
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
    }
}
