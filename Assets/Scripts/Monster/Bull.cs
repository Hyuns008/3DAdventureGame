using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : Monster
{
    [Header("���� �ݶ��̴�")]
    [SerializeField] private BoxCollider hitCollider;

    [Header("���� ����")]
    [SerializeField, Tooltip("���ݽ� ���� ���ݱ��� ������ �ð�")] private float attackDelay;
    [SerializeField] private float delayTimer; //������ Ÿ�̸�
    [SerializeField] private bool attackOn = false; //���� ���ɿ��θ� üũ�ϴ� ����
    [SerializeField] private float phase; //������ ������
    private int comboAttack; //���� ���� ����
    [Space]
    [SerializeField, Tooltip("�޼� ����")] private GameObject leftWeapon;
    [SerializeField, Tooltip("������ ����")] private GameObject rightWeapon;


    private Vector3 beforePos;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        bullAnimatoin();
        if (base.noHit == false)
        {
            playerHitCheck();
            monstertimer();
        }
    }

    private void hitTrigger(Collider _hitCollider)
    {
        if (_hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (base.moveStop != true)
            {
                base.moveStop = true;
            }

            if (attackOn == true)
            {
                MonsterAttackCheck leftAttackSc = leftWeapon.GetComponent<MonsterAttackCheck>();
                MonsterAttackCheck rightAttackSc = rightWeapon.GetComponent<MonsterAttackCheck>();

                if (phase == 0)
                {
                    base.anim.Play("1Phasecombo1");

                    delayTimer = attackDelay;

                    rightAttackSc.SetAttackDamage(base.damage);
                }
                else if (phase == 1)
                {
                    base.anim.Play("2Phasecombo1");

                    delayTimer = attackDelay + 1;

                    leftAttackSc.SetAttackDamage(base.damage + (base.damage * 0.3f));
                    rightAttackSc.SetAttackDamage(base.damage + (base.damage * 0.3f));
                }
                else
                {
                    base.anim.Play("3Phasecombo1");

                    delayTimer = attackDelay + 2;

                    leftAttackSc.SetAttackDamage(base.damage + (base.damage * 0.5f));
                    rightAttackSc.SetAttackDamage(base.damage + (base.damage * 0.5f));
                }

                attackOn = false;
            }
        }
    }

    /// <summary>
    /// �÷��̾ �����ϱ� ���� �ݶ��̴�
    /// </summary>
    private void playerHitCheck()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < chasePlayerRadius)
        {
            if (attackOn == true)
            {
                MonsterAttackCheck leftAttackSc = leftWeapon.GetComponent<MonsterAttackCheck>();
                MonsterAttackCheck rightAttackSc = rightWeapon.GetComponent<MonsterAttackCheck>();

                if (phase == 0)
                {
                    base.anim.Play("1Phasecombo1");

                    delayTimer = attackDelay;

                    rightAttackSc.SetAttackDamage(base.damage);
                }
                else if (phase == 1)
                {
                    base.anim.Play("2Phasecombo1");

                    delayTimer = attackDelay + 1;

                    leftAttackSc.SetAttackDamage(base.damage + (base.damage * 0.3f));
                    rightAttackSc.SetAttackDamage(base.damage + (base.damage * 0.3f));
                }
                else
                {
                    base.anim.Play("3Phasecombo1");

                    delayTimer = attackDelay + 2;

                    leftAttackSc.SetAttackDamage(base.damage + (base.damage * 0.5f));
                    rightAttackSc.SetAttackDamage(base.damage + (base.damage * 0.5f));
                }

                attackOn = false;
            }
        }

        //Collider[] hitCheck = Physics.OverlapBox(hitCollider.bounds.center, hitCollider.bounds.size * 0.5f, Quaternion.identity,
        //    LayerMask.GetMask("Player"));

        //int hitCheckCount = hitCheck.Length;

        //if (hitCheckCount > 0)
        //{
        //    for (int i = 0; i < hitCheckCount; i++)
        //    {
        //        hitTrigger(hitCheck[i]);
        //    }
        //}
        //else
        //{
        //    if (base.moveStop != false)
        //    {
        //        base.moveStop = false;
        //    }
        //}
    }

    /// <summary>
    /// ������ Ÿ�̸Ӱ� ���ִ� �Լ�
    /// </summary>
    private void monstertimer()
    {
        if (attackOn == false)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                attackOn = true;
            }
        }
    }

    /// <summary>
    /// bull���� �ִϸ��̼��� �־��ֱ� ���� �Լ�
    /// </summary>
    private void bullAnimatoin()
    {
        base.anim.SetFloat("Phase", phase);
    }

    /// <summary>
    /// �ִϸ��̼ǿ� ���� �̵��� �����Ű�� ���� �Լ�
    /// </summary>
    public void MoveOn()
    {
        base.moveStop = false;
    }

    /// <summary>
    /// ȸ���� ���߰� ���ִ� �Լ�
    /// </summary>
    public void RotateStopTrue()
    {
        base.rotateStop = true;
    }

    /// <summary>
    /// ȸ���� �ٽ� �����ϰ� ���ִ� �Լ�
    /// </summary>
    public void RotateStopFalse()
    {
        base.rotateStop = false;
    }

    /// <summary>
    /// �޼� ������ �ݶ��̴��� ����
    /// </summary>
    public void LeftAttackSetActiveTrue()
    {
        leftWeapon.SetActive(true);
    }

    /// <summary>
    /// �޼� ������ �ݶ��̴��� ����
    /// </summary>
    public void LeftAttackSetActiveFalse()
    {
        leftWeapon.SetActive(false);
    }

    /// <summary>
    /// ������ ������ �ݶ��̴��� ����
    /// </summary>
    public void RightAttackSetActiveTrue()
    {
        rightWeapon.SetActive(true);
    }

    /// <summary>
    /// ������ ������ �ݶ��̴��� ����
    /// </summary>
    public void RightAttackSetActiveFalse()
    {
        rightWeapon.SetActive(false);
    }
}
