using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Vector3 moveVec;
    protected Animator anim;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("������ �̵��ӵ�")] protected float moveSpeed;
    [SerializeField, Tooltip("������ �̵��� ����")] protected bool moveStop;
    [SerializeField, Tooltip("������ �̵��� ���� ���Ͱ�")] protected Vector3 moveXYZ;
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
    [SerializeField, Tooltip("�÷��̾� Ȯ�ο���")] protected Collider checkColl;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        randomRotateTime = Random.Range(rotateTime.x, rotateTime.y);
        randomRotateY = Random.Range(rotateY.x, rotateY.y);
    }

    protected virtual void Update()
    {
        monsterTimers();
        monsterMove();
        monsterAnim();
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
        float v = Mathf.SmoothDampAngle(transform.eulerAngles.y, randomRotateY, ref curVelocity, 0.3f, smoothMaxSpeed);
        transform.rotation = Quaternion.Euler(0, v, 0);
        moveVec = transform.rotation * new Vector3 (moveXYZ.x, moveXYZ.y, moveXYZ.z) * moveSpeed;
        rigid.velocity = moveVec;
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
    public virtual void monsterHit(float _hitDamge)
    {
        hp -= _hitDamge;
    }
}
