using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Vector3 moveVec;
    protected Animator anim;

    private GameManager gameManager;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("������ �̵��ӵ�")] protected float moveSpeed;
    [SerializeField, Tooltip("������ �̵��� ����")] protected bool moveStop;
    [Space]
    [SerializeField, Tooltip("������ ���� ȸ�� �ð� �ּ�, �ִ�")] protected Vector2 rotateTime;
    [SerializeField, Tooltip("������ ���� ȸ�� Y �� �ּ�, �ִ�")] protected Vector2 rotateY;
    protected float rotateTimer; //Ÿ�̸�
    protected float randomRotateY; //���� Y�� ȸ���� �޾� �� ����
    protected float randomRotateTime; //���� Ÿ���� �޾� �� ����
    [SerializeField, Tooltip("�ε巴�� ȸ���ϱ� ���� �ִ�ӵ�")] protected float smoothMaxSpeed = 10f;
    protected float curVelocity;
    [Space]
    [SerializeField, Tooltip("������ ���ݷ�")] protected float damage;
    [SerializeField, Tooltip("������ ü��")] protected float hp;
    [SerializeField, Tooltip("������ ����")] protected float armor;
    [SerializeField, Tooltip("�÷��̾� Ȯ�ο���")] protected BoxCollider checkColl;
    [Space]
    [SerializeField, Tooltip("���� �������� ǥ���� ������")] private GameObject hitDamagePrefab;
    [SerializeField, Tooltip("�Ӹ����� �����Ǵ� ����")] private float heightValue;
    [Space]
    [SerializeField, Tooltip("�÷��̾�� ������ ����ġ")] private float setExp;

    private bool noHit; //�״� �ִϸ��̼��� ����� �� ��Ʈ������ ���� ���� ����

    private InputController player;

    protected virtual void OnTrigger(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collider.gameObject.GetComponent<InputController>();
        }
    }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        randomRotateTime = Random.Range(rotateTime.x, rotateTime.y);
        randomRotateY = Random.Range(rotateY.x, rotateY.y);
    }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
    }

    protected virtual void Update()
    {
        if (noHit == true)
        {
            return;
        }

        playerCheck();
        monsterTimers();
        monsterMove();
        monsterAnim();
        monsterDead();
    }

    /// <summary>
    /// ������ �ݶ��̴� �ȿ� �÷��̾ �ִ��� üũ���ִ� �Լ�
    /// </summary>
    protected virtual void playerCheck()
    {
        Collider[] playerColl = Physics.OverlapBox(checkColl.bounds.center, checkColl.bounds.size * 0.5f, Quaternion.identity,
            LayerMask.GetMask("Player"));

        int count = playerColl.Length;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                OnTrigger(playerColl[0]);
            }
        }
        else
        {
            player = null;
        }
    }

    /// <summary>
    /// ���Ϳ��� ����Ǵ� Ÿ�̸ӵ��� ����
    /// </summary>
    protected virtual void monsterTimers()
    {
        if (rotateTimer >= 100f)
        {
            rotateTimer = 0f;
        }

        rotateTimer += Time.deltaTime;

        if (rotateTimer >= randomRotateTime)
        {
            randomRotateTime = Random.Range(rotateTime.x, rotateTime.y);
            randomRotateY = Random.Range(rotateY.x, rotateY.y);
            rotateTimer = 0f;
        }
    }

    /// <summary>
    /// �ڽĿ��� ������ ���͸� �������� ����ϴ� �Լ�
    /// </summary>
    protected virtual void monsterMove()
    {
        if (player == null)
        {
            float smoothDamp = Mathf.SmoothDampAngle(transform.eulerAngles.y, randomRotateY, ref curVelocity, 0.3f, smoothMaxSpeed);
            
            transform.rotation = Quaternion.Euler(0.0f, smoothDamp, 0.0f);

            moveVec = transform.forward * moveSpeed;
        }
        else
        {
            //float smoothDamp = Mathf.SmoothDampAngle(transform.eulerAngles.y, player.transform.position.x + player.transform.position.y, 
            //    ref curVelocity, 0.3f, smoothMaxSpeed);
            //
            //transform.rotation = Quaternion.Euler(0.0f, smoothDamp, 0.0f);

            Vector3 vec = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(vec);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            moveVec = transform.forward * moveSpeed;
        }

        moveVec.y = rigid.velocity.y;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// �ڽĿ��� ������ ���Ͱ� �ǰ� 0�� �Ǿ��� �� ����Ǵ� �Լ�
    /// </summary>
    protected virtual void monsterDead()
    {
        if (hp <= 0.0f)
        {
            gameManager.SetExp(setExp);
            anim.Play("Die");
            noHit = true;
            Destroy(gameObject, 2f);
        }
    }

    /// <summary>
    /// �ڽĿ��� ������ ������ �ִϸ� ����ϴ� �Լ�
    /// </summary>
    protected virtual void monsterAnim()
    {
        anim.SetFloat("isWalk", moveVec.z);
    }

    /// <summary>
    /// ���Ͱ� �¾Ҵ��� üũ���ִ� �Լ�
    /// </summary>
    public virtual void monsterHit(float _hitDamge, Color _damageText)
    {
        if (noHit == true)
        {
            return;
        }

        float donwDamage = _hitDamge - armor;

        GameObject hitDamageObj = Instantiate(hitDamagePrefab,
            new Vector3(transform.localPosition.x, heightValue, transform.localPosition.z), Quaternion.identity, transform);
        TMP_Text hitText = hitDamageObj.transform.GetChild(0).GetComponent<TMP_Text>();

        string damgeTest = donwDamage.ToString("F0");

        if (donwDamage <= -10f)
        {
            hitText.color = Color.yellow;
            hitText.text = "Miss";
            return;
        }
        else if (donwDamage <= 0f && donwDamage > -10f)
        {
            hitText.color = Color.white;
            hitText.text = $"{1}";
            hp -= 1;
        }
        else if (donwDamage > 0f)
        {
            hitText.color = _damageText;
            hitText.text = $"{damgeTest}";
            hp -= donwDamage;
        }

        anim.Play("Hit");
    }
}
