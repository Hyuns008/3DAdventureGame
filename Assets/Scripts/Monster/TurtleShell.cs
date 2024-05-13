using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Monster
{
    [Header("���� �ݶ��̴�")]
    [SerializeField] private BoxCollider hitCollider;
    private bool playerAttack = false;

    [Header("���� ����")]
    [SerializeField, Tooltip("���ݽ� ���� ���ݱ��� ������ �ð�")] private float attackDelay;
    private float delayTimer; //������ Ÿ�̸�
    private bool attackOn = false; //���� ���ɿ��θ� üũ�ϴ� ����

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
            if (attackOn == true)
            {
                base.anim.Play("Attack02");

                if (playerAttack == true)
                {
                    InputController playerSc = _hitCollider.GetComponent<InputController>();
                    playerSc.PlayerHitCheck(base.damage);
                    delayTimer = attackDelay;
                    attackOn = false;
                    playerAttack = false;
                }
            }
        }
    }

    /// <summary>
    /// �÷��̾ �����ϱ� ���� �ݶ��̴�
    /// </summary>
    private void playerHitCheck()
    {
        Collider[] hitCheck = Physics.OverlapBox(hitCollider.bounds.center, hitCollider.bounds.size * 0.5f, Quaternion.identity, 
            LayerMask.GetMask("Player"));

        int hitCheckCount = hitCheck.Length;

        if (hitCheckCount > 0)
        {
            for (int i = 0; i < hitCheckCount; i++)
            {
                hitTrigger(hitCheck[i]);
            }
        }
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
    /// �ִϸ��̼ǿ� ���� �÷��̾ �����ϱ� ���� �Լ�
    /// </summary>
    public void AttackHit()
    {
        playerAttack = true;
    }
}
