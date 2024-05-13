using System.Collections;
using UnityEngine;

/// <summary>
/// 애너미 관리 
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    #region Variable
    public enum Enemy {NormalEnemy,BossEnemy }
    public Enemy enemyType;
    //애너미 이동 경로
    [SerializeField]private Transform[] wayPoints;
    //사망시 출력할 골드 프리펩
    [SerializeField]private GameObject gold;
    //현재 웨이포인트
    public int CurrentWayPoint { get;private set; }
    //이동 속도
    [SerializeField] private float speed;
    //이동 방향
    private Vector2 nextDir;
    //현재 체력
    public float CurrentHp { get; private set; }
    //최대 체력
    [SerializeField] private float maxHp;
    public Animator animator { get; private set; }
    //애너미 메인 콜라이더
    private Collider2D col;
    //생존 여부
    private bool isLive = true;
    //슬로우 여부
    public bool IsSlow { get; set; }
    //슬로우 타이머
    private float slowTimer;
    //스턴 여부
    public bool IsStun { get; set; }
    //스턴 타이머
    private float stunTimer;
    //스킬로 인한 긴 스턴 타이머
    private float longStunTimer;
    //스킬로 인한 긴 스턴
    public bool IsLongStun { get; set; }
    //슬로우 적용 속도
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
    /// 다음 웨이포인트로 이동
    /// </summary>
    private void Movement()
    {
        if (!isLive || !Manager.Instance.IsLive)
            return;

        //현재 웨이포인트가 마지막 웨이포인트가 아니고
        if(CurrentWayPoint < wayPoints.Length)
        {
            //플레이어가 다음 웨이포인트 위치에 도착하지 못하면
            if(Vector2.Distance(transform.position, wayPoints[CurrentWayPoint].position) > 0.05f )
            {
                //이동 경로 계산
                nextDir = wayPoints[CurrentWayPoint].position - transform.position;
                nextDir = nextDir.normalized;

                //다음 웨이포인트 위치로 이동
                transform.Translate(nextDir * speed * Time.deltaTime);

            }
            else // 다음 웨이포인트 위치에 도달하면
            {
                //다음 웨이포인트를 현재 웨이포인트로 바꾸고, 그 다음 웨이포인트를 목표로 설정
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
    /// 체력 조정
    /// </summary>
    /// <param name="hp">조정할 체력</param>
    public void SetHp(float hp)
    {
        CurrentHp += hp;

        if(CurrentHp <= 0)
        {
            StartCoroutine(Co_DeadAnimation());
        }
    }

    /// <summary>
    /// 사망 애니메이션 코루틴
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
