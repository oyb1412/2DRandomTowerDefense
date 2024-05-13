using System.Collections;
using UnityEngine;

/// <summary>
/// �ֳʹ� ���� 
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    #region Variable
    public enum Enemy {NormalEnemy,BossEnemy }
    public Enemy enemyType;
    //�ֳʹ� �̵� ���
    [SerializeField]private Transform[] wayPoints;
    //����� ����� ��� ������
    [SerializeField]private GameObject gold;
    //���� ��������Ʈ
    public int CurrentWayPoint { get;private set; }
    //�̵� �ӵ�
    [SerializeField] private float speed;
    //�̵� ����
    private Vector2 nextDir;
    //���� ü��
    public float CurrentHp { get; private set; }
    //�ִ� ü��
    [SerializeField] private float maxHp;
    public Animator animator { get; private set; }
    //�ֳʹ� ���� �ݶ��̴�
    private Collider2D col;
    //���� ����
    private bool isLive = true;
    //���ο� ����
    public bool IsSlow { get; set; }
    //���ο� Ÿ�̸�
    private float slowTimer;
    //���� ����
    public bool IsStun { get; set; }
    //���� Ÿ�̸�
    private float stunTimer;
    //��ų�� ���� �� ���� Ÿ�̸�
    private float longStunTimer;
    //��ų�� ���� �� ����
    public bool IsLongStun { get; set; }
    //���ο� ���� �ӵ�
    private float slowSpeed;

    #endregion

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        CurrentHp = maxHp;
        CurrentHp += ((SpawnManager.instance.CurrentLevel - 1) * 2);
        slowSpeed = speed * 0.5f;
    }

    #region UpdateMethod
    void Update()
    {
        if (!isLive || !Manager.Instance.IsLive)
            return;

        Movement();
        animator.SetTrigger("Move");

        if (IsSlow)
        {
            slowTimer += Time.deltaTime;
            speed = slowSpeed;
            if(slowTimer > 1f)
            {
                speed = slowSpeed * 2f;
                slowTimer = 0;
                IsSlow = false;
            }
        }

        if(IsStun)
        {

            stunTimer += Time.deltaTime;
            speed = 0;
            if (stunTimer > 0.6f)
            {
                speed = slowSpeed * 2f;
                stunTimer = 0;
                IsStun = false;
            }
        }

        if (IsLongStun)
        {

            longStunTimer += Time.deltaTime;
            speed = 0;
            animator.SetTrigger("Stun");
            if (longStunTimer > 2.5f)
            {
                speed = slowSpeed * 2f;
                longStunTimer = 0;
                IsLongStun = false;
            }
        }
    }
    private void LateUpdate() {
        if (!isLive || !Manager.Instance.IsLive)
            return;
        if (enemyType == Enemy.BossEnemy) {
            if (nextDir.x > 0)
                transform.localScale = new Vector3(-1.5f, 1.5f, 1f);
            else if (nextDir.x < 0)
                transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        } else {
            if (nextDir.x > 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (nextDir.x < 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    #endregion

    /// <summary>
    /// ���� ��������Ʈ�� �̵�
    /// </summary>
    private void Movement()
    {
        if (!isLive || !Manager.Instance.IsLive)
            return;

        //���� ��������Ʈ�� ������ ��������Ʈ�� �ƴϰ�
        if(CurrentWayPoint < wayPoints.Length)
        {
            //�÷��̾ ���� ��������Ʈ ��ġ�� �������� ���ϸ�
            if(Vector2.Distance(transform.position, wayPoints[CurrentWayPoint].position) > 0.05f )
            {
                //�̵� ��� ���
                nextDir = wayPoints[CurrentWayPoint].position - transform.position;
                nextDir = nextDir.normalized;

                //���� ��������Ʈ ��ġ�� �̵�
                transform.Translate(nextDir * speed * Time.deltaTime);

            }
            else // ���� ��������Ʈ ��ġ�� �����ϸ�
            {
                //���� ��������Ʈ�� ���� ��������Ʈ�� �ٲٰ�, �� ���� ��������Ʈ�� ��ǥ�� ����
                CurrentWayPoint++;
            }
        }

        if(Vector2.Distance(transform.position, wayPoints[10].position) < 0.05f)
        {
            Manager.Instance.SetHeart(-1);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="hp">������ ü��</param>
    public void SetHp(float hp)
    {
        CurrentHp += hp;

        if(CurrentHp <= 0)
        {
            StartCoroutine(Co_DeadAnimation());
        }
    }

    /// <summary>
    /// ��� �ִϸ��̼� �ڷ�ƾ
    /// </summary>
    private IEnumerator Co_DeadAnimation()
    {
        this.gameObject.layer = 0;
        col.enabled = false;
        isLive = false;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(0.3f);
        var obj = Instantiate(gold, null).transform;
        obj.position = transform.position;
        if(enemyType == Enemy.BossEnemy)
        {
            Manager.Instance.getSuperTower = true;
            Manager.Instance.SetGold(30);
        }
        else
            Manager.Instance.SetGold(3);

        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
