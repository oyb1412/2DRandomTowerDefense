using System.Collections;
using UnityEngine;

/// <summary>
/// ��� Ÿ�� ����
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
    //Ÿ�� �߻�ü �ӵ�
    private float towerVelocity = 10f;
    //Ÿ�� �ִ� ����
    public int MaxTowerLevel;
    //Ÿ�� ������
    public float TowerDamage;
    //Ÿ�� ��Ÿ�
    public float TowerRange;
    //Ÿ�� �����
    public int TowerPierce;
    //Ÿ�� ���� �ӵ�
    public float TowerAttackSpeed;
    [Header("--Prefab--")]
    //�� ������ �´� ���� ������
    [SerializeField] private GameObject[] levelEffect;
    //���� ���� ���� ������
    private GameObject currentLevelEffect;
    //�߻�ü ������
    [SerializeField] private GameObject bulletPrefabs;
    //������ Ÿ�� ���� ȭ��ǥ ������
    [SerializeField] private GameObject levelUpArrowPrefabs;
    //������Ÿ�� ������
    [SerializeField] private GameObject masterTowerPrefabs;
    //������ ����Ʈ ������
    [SerializeField] private GameObject levelUpEffectPrefabs;
    //Ÿ�� ���� ����Ʈ ������
    [SerializeField] private GameObject CreateEffectPrefabs;
    //Ÿ�� �Ǹ� ����Ʈ ������
    [SerializeField] private GameObject DeleteEffectPrefabs;
    //�ֳʹ� ���̾�
    [SerializeField] private LayerMask layer;
    //������ ȭ��ǥ ��� �� ����
    private bool useArrow;
    //���� Ÿ�̸�
    private float SetBulletTimer;
    //���� ǥ�� �� ������
    [SerializeField] private GameObject levelStarPrefabs;
    //���� ��� Ÿ�� Ʈ������
    private Transform target;
    //�߻�ü ����
    private Vector2 bulletDir;
    //�� ���� �� ������
    private Transform[] levelStar;
    //���� ���� �� ��� ��
    private RaycastHit2D[] targets;

    //Ÿ�� �����
    public int TowerProjectile { get; set; }

    //�� ������Ʈ ���׷��̵� ����
    public static bool StateUpgrade1 { get; set; }
    public static bool StateUpgrade2 { get; set; }
    public static bool StateUpgrade3 { get; set; }
    public static bool StateUpgrade4 { get; set; }
    public static bool StateUpgrade5 { get; set; }
    public static bool StateUpgrade6 { get; set; }

    //���� Ÿ�� ����
    public int CurrentTowerLevel { get; set; }
    public Animator Animator { get; private set; }
    //������ ���� ����
    public bool canLevelUp { get; set; }
    //���� ���� ����
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
    /// ���� �ִϸ��̼�
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
    /// �߻�ü �߻� �޼ҵ� ȣ��(���� �ִϸ��̼� �ݹ����� ȣ��)
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
    /// �߻�ü �߻�
    /// </summary>
    /// <param name="type">�߻��� Ÿ���� Ÿ��</param>
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
    /// ����ü ���縦 ���� �ڷ�ƾ
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
    /// ������ ���� ȭ��ǥ ����
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
    /// ������ ���� ȭ��ǥ ����
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
    /// Ÿ�� ������
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
    /// Ÿ�� �ִ뷹�� �ռ� �� ������Ÿ�� ����
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
    /// ������ ����Ʈ ���
    /// </summary>
    void CreateLevelUpEffect()
    {
        Transform trans = Instantiate(levelUpEffectPrefabs, null).transform;
        trans.position = new Vector2(transform.position.x, transform.position.y+1.1f);
    }

    /// <summary>
    /// ������ �´� ����Ʈ ���
    /// </summary>
    void NewCreateLevelEffect() {
        Destroy(currentLevelEffect.gameObject);
        currentLevelEffect = Instantiate(levelEffect[CurrentTowerLevel], transform);
    }

    /// <summary>
    /// Ÿ�� ������ ������ �´� ����Ʈ ���
    /// </summary>
    public void CreateLevelEffect() {
        currentLevelEffect = Instantiate(levelEffect[0], transform);
    }

    /// <summary>
    /// Ÿ�� ���� ����Ʈ ���
    /// </summary>
    public void CreateEffect()
    {
        Transform trans = Instantiate(CreateEffectPrefabs, transform).transform;
        trans.position = new Vector2(transform.position.x, transform.position.y + 0.8f);
    }

    /// <summary>
    /// Ÿ�� �Ǹ� ����Ʈ ���
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
