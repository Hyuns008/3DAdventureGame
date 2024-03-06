using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private CharacterController characterController; //�÷��̾ ������ �ִ� ĳ���� ��Ʈ�ѷ��� �޾ƿ� ����
    private Vector3 moveVec; //�÷��̾��� �Է°��� �޾ƿ� ����

    private Camera mainCam;

    private Animator anim; //�÷��̾��� �ִϸ��̼��� �޾ƿ� ����

    [Header("�÷��̾� ����")]
    [SerializeField, Range(0, 1), Tooltip("Idle �ִϸ��̼� ����")] private int idleChange;
    [Space]
    [SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")] private float moveSpeed;
    [Space]
    [SerializeField, Tooltip("�÷��̾��� ������ ��")] private float diveRollForce;
    [SerializeField, Tooltip("������ ��Ÿ��")] private float diveRollCoolTime;
    private Transform diveRollTrs; //������ �� ȸ���� ������ �޾ƿ� ����
    private float diveRollTimer; //������ ��Ÿ���� ������ Ÿ�̸� ����
    private float deveRollValue;
    private float dive;
    private bool useDieveRoll = false; //�����⸦ ����ߴ��� üũ�ϱ� ���� ����

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        diveRollTrs = GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        playerTimers();
        playerLookAtScreen();
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
    /// �÷��̾ ���콺�� ������ ȭ���� �� �� �ְ� ����ϴ� �Լ�
    /// </summary>
    private void playerLookAtScreen()
    {
        transform.rotation = Quaternion.Euler(0f, mainCam.transform.rotation.y, 0f);
    }

    /// <summary>
    /// �÷��̾��� �⺻ �̵��� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (useDieveRoll == true)
        {
            diveRollTrs.rotation = Quaternion.Euler(new Vector3(0f, deveRollValue, 0f));
            characterController.Move(diveRollTrs.rotation * new Vector3(0f, 0f, diveRollForce) * Time.deltaTime);
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
        deveRollValue = Mathf.SmoothDampAngle(transform.localRotation.y, -90f, ref dive, 3f, 4f);
        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false)
        {
            if (inputHorizontal() < 0f)
            {
                deveRollValue = Mathf.SmoothDampAngle(transform.localRotation.y, -90f, ref dive, 0f, 4f, Time.deltaTime);
            }
            else if (inputHorizontal() > 0f)
            {
                deveRollValue = Mathf.SmoothDampAngle(transform.localRotation.y, 90f, ref dive, 0f, 4f, Time.deltaTime);
            }
            else if (inputVertical() < 0f)
            {
                deveRollValue = Mathf.SmoothDampAngle(transform.localRotation.y, 180f, ref dive, 0f, 4f, Time.deltaTime);
            }
            else if (inputVertical() > 0f)
            {
                deveRollValue = Mathf.SmoothDampAngle(transform.localRotation.y, 0f, ref dive, 0f, 4f, Time.deltaTime);
            }

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
