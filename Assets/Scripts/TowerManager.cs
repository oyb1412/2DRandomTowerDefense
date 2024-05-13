using System.Collections;
using UnityEngine;

/// <summary>
/// 모든 타워 관리
/// </summary>
public class TowerManager : MonoBehaviour
{
    #region Variable
    public enum Tower {
        BowTower, SwordTower, AxeTower, FireTower, SlowTower, StunTower,
        MasterBowTower, MasterSwordTower, MasterAxeTower, MasterFireTower, MasterSlowTower, MasterStunTower, SuperTower
    }
    public Tower towerType;

    [Header("--Info--")]
    //타워 발사체 속도
    private float towerVelocity = 10f;
    //타워 최대 레벨
    public int MaxTowerLevel;
    //타워 데미지
    public float TowerDamage;
    //타워 사거리
    public float TowerRange;
    //타워 관통력
    public int TowerPierce;
    //타워 공격 속도
    public float TowerAttackSpeed;
    [Header("--Prefab--")]
    //각 레벨에 맞는 오라 프리펩
    [SerializeField] private GameObject[] levelEffect;
    //현재 레벨 오라 프리펩
    private GameObject currentLevelEffect;
    //발사체 프리펩
    [SerializeField] private GameObject bulletPrefabs;
    //레벨업 타워 지목 화살표 프리펩
    [SerializeField] private GameObject levelUpArrowPrefabs;
    //마스터타워 프리펩
    [SerializeField] private GameObject masterTowerPrefabs;
    //레벨업 이펙트 프리펩
    [SerializeField] private GameObject levelUpEffectPrefabs;
    //타워 생성 이펙트 프리펩
    [SerializeField] private GameObject CreateEffectPrefabs;
    //타워 판매 이펙트 프리펩
    [SerializeField] private GameObject DeleteEffectPrefabs;
    //애너미 레이어
    [SerializeField] private LayerMask layer;
    //레벨업 화살표 사용 중 유부
    private bool useArrow;
    //공격 타이머
    private float SetBulletTimer;
    //레벨 표기 별 프리펩
    [SerializeField] private GameObject levelStarPrefabs;
    //공격 대상 타겟 트랜스폼
    private Transform target;
    //발사체 방향
    private Vector2 bulletDir;
    //각 레벨 별 프리펩
    private Transform[] levelStar;
    //공격 범위 내 모든 적
    private RaycastHit2D[] targets;

    //타워 관통력
    public int TowerProjectile { get; set; }

    //각 스테이트 업그레이드 여부
    public static bool StateUpgrade1 { get; set; }
    public static bool StateUpgrade2 { get; set; }
    public static bool StateUpgrade3 { get; set; }
    public static bool StateUpgrade4 { get; set; }
    public static bool StateUpgrade5 { get; set; }
    public static bool StateUpgrade6 { get; set; }

    //현재 타워 레벨
    public int CurrentTowerLevel { get; set; }
    public Animator Animator { get; private set; }
    //레벨업 가능 여부
    public bool canLevelUp { get; set; }
    //공격 가능 여부
    public bool IsLive { get; set; }

    #endregion

    #region InitMethod
    private void Awake()
    {
        IsLive = true;
        Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameObject star = Instantiate(levelStarPrefabs, transform);
        if (towerType == Tower.MasterStunTower || towerType == Tower.MasterAxeTower || towerType == Tower.MasterFireTower ||
            towerType == Tower.MasterBowTower || towerType == Tower.MasterSlowTower || towerType == Tower.MasterSwordTower)
        {
            star.transform.position = new Vector3(transform.position.x , transform.position.y - 0.4f, 1f);

        }
        else
        {
            star.transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, 1f);
            levelStar = new Transform[5];
            levelStar = star.GetComponentsInChildren<Transform>();
            for (int i = 2; i < levelStar.Length; i++)
            {
                levelStar[i].gameObject.SetActive(false);
            }
        }
        star.SetActive(true);
    }
    #endregion

    #region UpdateMethod
    private void Update() {
        if (!Manager.Instance.IsLive || !IsLive)
            return;

        if (target) {
            FireAnimation();
        }
    }
    private void FixedUpdate()
    {
        if (!Manager.Instance.IsLive || !IsLive)
            return;

        targets = Physics2D.CircleCastAll(transform.position, TowerRange, Vector2.zero, 0f, layer);
        if(targets != null)
        {
            if(targets.Length == 1)
            {
                target = targets[0].transform;
            }
            else if(targets.Length > 1)
            {
                target = MouseManager.instance.GetEarliestTarget(targets);
            }
        }
    }
    #endregion

    #region FireMethod
    /// <summary>
    /// 공격 애니메이션
    /// </summary>
    void FireAnimation()
    {
        SetBulletTimer += Time.deltaTime;

        if (SetBulletTimer > TowerAttackSpeed)
        {
            Animator.SetTrigger("Attack");
            SetBulletTimer = 0;
        }
    }

    /// <summary>
    /// 발사체 발사 메소드 호출(공격 애니메이션 콜백으로 호출)
    /// </summary>
    public void FireBullet()
    {
        if (target)
        {
            switch(towerType)
            {
                case Tower.BowTower:
                    SetBullet((int)Tower.BowTower);
                    break;
                case Tower.SwordTower:
                    SetBullet((int)Tower.SwordTower);
                    break;
                case Tower.AxeTower:
                    SetBullet((int)Tower.AxeTower);
                    break;
                case Tower.FireTower:
                    SetBullet((int)Tower.FireTower);
                    break;
                case Tower.SlowTower:
                    SetBullet((int)Tower.SlowTower);
                    break;
                case Tower.StunTower:
                    SetBullet((int)Tower.StunTower);
                    break;
                case Tower.SuperTower:
                    SetBullet((int)Tower.SuperTower);
                    break;
                case Tower.MasterBowTower:
                    SetBullet((int)Tower.MasterBowTower);
                    break;
                case Tower.MasterSwordTower:
                    SetBullet((int)Tower.MasterSwordTower);
                    break;
                case Tower.MasterAxeTower:
                    SetBullet((int)Tower.MasterAxeTower);
                    break;
                case Tower.MasterSlowTower:
                    SetBullet((int)Tower.MasterSlowTower);
                    break;
                case Tower.MasterStunTower:
                    SetBullet((int)Tower.MasterStunTower);
                    break;
            }
            SetBulletTimer = 0;
        }
    }

    /// <summary>
    /// 발사체 발사
    /// </summary>
    /// <param name="type">발사할 타워의 타입</param>
    private void SetBullet(int type)
    {
        if(towerType == Tower.StunTower || towerType == Tower.MasterStunTower || towerType == Tower.SlowTower || towerType == Tower.MasterSlowTower
            || towerType == Tower.FireTower || towerType == Tower.MasterFireTower) 
        {

                bulletDir = target.transform.position - transform.position;
                var bullet = FactoryManager.Instance.CreateBullet(bulletPrefabs, transform.position, bulletDir);
                bullet.Init(TowerDamage, towerVelocity, bulletDir, CurrentTowerLevel, type, TowerPierce,transform.position.x,transform.position.y);
        }
        if(TowerProjectile == 1)
        {
            if(towerType == Tower.BowTower || towerType == Tower.MasterBowTower || towerType == Tower.SwordTower || towerType == Tower.MasterSwordTower 
                || towerType == Tower.AxeTower || towerType == Tower.MasterAxeTower || towerType == Tower.SuperTower)
            StartCoroutine(Co_FireCorutine());
        }
        else
        {
            if (towerType == Tower.BowTower || towerType == Tower.MasterBowTower || towerType == Tower.SwordTower || towerType == Tower.MasterSwordTower
            || towerType == Tower.AxeTower || towerType == Tower.MasterAxeTower || towerType == Tower.SuperTower)
            {
                bulletDir = target.transform.position - transform.position;
                var bullet = FactoryManager.Instance.CreateBullet(bulletPrefabs, transform.position, bulletDir);
                bullet.Init(TowerDamage, towerVelocity, bulletDir, CurrentTowerLevel, type, TowerPierce);
            }
        }

    }

    /// <summary>
    /// 투사체 연사를 위한 코루틴
    /// </summary>
    private IEnumerator Co_FireCorutine()
    {
        bulletDir = target.transform.position - transform.position;
        var bullet = FactoryManager.Instance.CreateBullet(bulletPrefabs, transform.position, bulletDir);
        bullet.Init(TowerDamage, towerVelocity, bulletDir, CurrentTowerLevel, 0, TowerPierce);
        yield return new WaitForSeconds(0.1f);
        var bullet2 = FactoryManager.Instance.CreateBullet(bulletPrefabs, transform.position, bulletDir);
        bullet2.Init(TowerDamage, towerVelocity, bulletDir, CurrentTowerLevel, 0, TowerPierce);
    }
    #endregion

    #region TowerMethod
    /// <summary>
    /// 레벨업 가능 화살표 생성
    /// </summary>
    public void CreateLevelUPArrow()
    {
        if (!useArrow)
        {
            useArrow = true;
            Transform pos = Instantiate(levelUpArrowPrefabs, transform).transform;
            pos.position = new Vector3(transform.position.x, transform.position.y + 1f,
                transform.position.z);
        }
    }

    /// <summary>
    /// 레벨업 가능 화살표 제거
    /// </summary>
    public void DeleteLevelUPArrow()
    {
        var obj = GetComponentInChildren<ArrowMovement>();
        if (obj)
        {
            obj.gameObject.SetActive(false);
            useArrow = false;
        }
    }

    /// <summary>
    /// 타워 레벨업
    /// </summary>
    public void TowerLevelUp()
    {
        DeleteLevelUPArrow();
        CreateLevelUpEffect();

        CurrentTowerLevel++;
        NewCreateLevelEffect();

        levelStar[CurrentTowerLevel+1].gameObject.SetActive(true);
        transform.localScale *= 1.05f;
        TowerDamage *= 1.5f;
        TowerRange += 0.5f;
        canLevelUp = false;
        if (CurrentTowerLevel == MaxTowerLevel)
           CreateMasterTower();
    }

    /// <summary>
    /// 타워 최대레벨 합성 시 마스터타워 생성
    /// </summary>
    void CreateMasterTower()
    {
        Transform trans = Instantiate(masterTowerPrefabs, null).transform;
        trans.position = transform.position;
        trans.GetComponent<TowerManager>().CreateLevelEffect();
        gameObject.SetActive(false);
    }
    #endregion

    #region EffectMethod
    /// <summary>
    /// 레벨업 이펙트 출력
    /// </summary>
    void CreateLevelUpEffect()
    {
        Transform trans = Instantiate(levelUpEffectPrefabs, null).transform;
        trans.position = new Vector2(transform.position.x, transform.position.y+1.1f);
    }

    /// <summary>
    /// 레벨에 맞는 이펙트 출력
    /// </summary>
    void NewCreateLevelEffect() {
        Destroy(currentLevelEffect.gameObject);
        currentLevelEffect = Instantiate(levelEffect[CurrentTowerLevel], transform);
    }

    /// <summary>
    /// 타워 생성시 레벨에 맞는 이펙트 출력
    /// </summary>
    public void CreateLevelEffect() {
        currentLevelEffect = Instantiate(levelEffect[0], transform);
    }

    /// <summary>
    /// 타워 생성 이펙트 출력
    /// </summary>
    public void CreateEffect()
    {
        Transform trans = Instantiate(CreateEffectPrefabs, transform).transform;
        trans.position = new Vector2(transform.position.x, transform.position.y + 0.8f);
    }

    /// <summary>
    /// 타워 판매 이펙트 출력
    /// </summary>
    public void DeleteEffect()
    {
        Transform trans = Instantiate(DeleteEffectPrefabs, null).transform;
        trans.position = new Vector2(transform.position.x, transform.position.y + 0.8f);
    }

    #endregion

    private void OnDisable() {
        Destroy(gameObject);
    }
}
