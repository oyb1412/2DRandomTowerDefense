using UnityEngine;

/// <summary>
/// 투사체 관리 클래스
/// </summary>
public class Bullet : MonoBehaviour
{
    #region Variable
    [Header("--Info--")]
    //투사체 타입
    private int bulletType;
    //투사체 레벨
    private int bulletLevel;
    //투사체 데미지
    private float bulletDamage;
    //투사체 관통력
    private int bulletPierce;
    //공격 대상 수 제한
    private bool colTrigger;

    //발사 방향
    private Rigidbody2D rb;
    //공격 대상
    private EnemyMovement target;
    //공격 이펙트 프리펩
    [SerializeField] private GameObject effectPrefabs;
    #endregion

    #region InitMethod
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = transform.localScale * (1 + (0.1f * bulletLevel));

    }
    /// <summary>
    /// 발사체 생성시 초기화
    /// </summary>
    /// <param name="damage">데미지</param>
    /// <param name="velocity">발사체 발사 속도</param>
    /// <param name="dir">발사체 발사 방향</param>
    /// <param name="level">발사체 레벨</param>
    /// <param name="type">발사체 타입</param>
    /// <param name="pierce">발사체 관통력</param>
    /// <param name="posx">이펙트 위치 x</param>
    /// <param name="posy">이펙트 위치 y</param>
    public void Init(float damage,float velocity, Vector2 dir,int level, int type,int pierce, float posx = 0f, float posy = 0f)
    {
        bulletDamage = damage;
        bulletType = type;
        bulletLevel = level;
        bulletPierce = pierce;
        rb.velocity = dir * velocity;

        if (bulletType == (int)TowerManager.Tower.StunTower || bulletType == (int)TowerManager.Tower.MasterStunTower)
        {
            var obj = Instantiate(effectPrefabs, transform.parent);
            var effect = obj.transform.GetComponent<DamageEffect>();
            effect.transform.position = new Vector3(posx, posy);
            effect.EffectType = type;
            effect.EffectDamage = damage;
            effect.EffectLevel = level;
            gameObject.SetActive(false);
        }
    }
    #endregion

    /// <summary>
    /// 발사체 충돌
    /// </summary>
    /// <param name="collision">충돌 대상</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || !collision)
            return;

        target = collision.GetComponent<EnemyMovement>();
        if (!colTrigger)
        {
            switch (bulletType)
            {
                case (int)TowerManager.Tower.BowTower:
                case (int)TowerManager.Tower.MasterBowTower:
                case (int)TowerManager.Tower.AxeTower:
                case (int)TowerManager.Tower.SwordTower:
                case (int)TowerManager.Tower.MasterAxeTower:
                case (int)TowerManager.Tower.MasterSwordTower:
                case (int)TowerManager.Tower.SuperTower:
                    if (bulletPierce >= 0)
                    {
                        target.SetHp(-bulletDamage);
                        SetEffect();
                        bulletPierce--;
                    }
                    if (bulletPierce < 0)
                        gameObject.SetActive(false);

                    AudioManager.instance.PlayerSfx(AudioManager.Sfx.Attack);

                    break;

                case (int)TowerManager.Tower.FireTower:
                case (int)TowerManager.Tower.MasterFireTower:
                case (int)TowerManager.Tower.SlowTower:
                case (int)TowerManager.Tower.MasterSlowTower:

                    SetDamageEffect(bulletType, bulletLevel, bulletDamage);
                    gameObject.SetActive(false);
                    colTrigger = true;
                    AudioManager.instance.PlayerSfx(AudioManager.Sfx.Magic);

                    break;
            }
        }

    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 일반 타워이펙트 생성 및 위치 조정
    /// </summary>
    private void SetEffect()
    {
       var obj = Instantiate(effectPrefabs, null);
       obj.transform.position = target.transform.position; 
    }

    /// <summary>
    /// 마법 타워 이펙트 생성 및 위치 조정
    /// </summary>
    /// <param name="type"></param>
    /// <param name="level"></param>
    /// <param name="damage"></param>
    private void SetDamageEffect(int type, int level, float damage)
    {
        var obj = Instantiate(effectPrefabs, target.transform);
        obj.transform.position = target.transform.position;
        var effect = obj.transform.GetComponent<DamageEffect>();
        effect.EffectType = type;
        effect.EffectDamage = damage;
        effect.EffectLevel = level;
    }
}
