using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Vector3 moveVec;
    protected Animator anim;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("������ �̵��ӵ�")] protected float moveSpeed;
    [SerializeField, Tooltip("������ �̵��� ���� ���Ͱ�")] protected Vector3 moveXYZ;
    [SerializeField, Tooltip("������ ���ݷ�")] protected float damage;
    [SerializeField, Tooltip("������ ü��")] protected float hp;
    [SerializeField, Tooltip("������ ����")] protected float armor;
    [SerializeField, Tooltip("�÷��̾� Ȯ�ο���")] protected Collider checkColl;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        monsterMove();
        monsterAnim();
    }

    /// <summary>
    /// �ڽĿ��� ������ ���͸� �������� ����ϴ� �Լ�
    /// </summary>
    protected virtual void monsterMove()
    {
        moveVec = new Vector3 (moveXYZ.x, moveXYZ.y, moveXYZ.z) * moveSpeed;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// �ڽĿ��� ������ ������ �ִϸ� ����ϴ� �Լ�
    /// </summary>
    protected virtual void monsterAnim()
    {
        anim.SetFloat("isWalk", moveVec.z);
    }
}
