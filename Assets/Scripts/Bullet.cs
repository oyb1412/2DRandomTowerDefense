using UnityEngine;

/// <summary>
/// ����ü ���� Ŭ����
/// </summary>
public class Bullet : MonoBehaviour
{
    #region Variable
    [Header("--Info--")]
    //����ü Ÿ��
    private int bulletType;
    //����ü ����
    private int bulletLevel;
    //����ü ������
    private float bulletDamage;
    //����ü �����
    private int bulletPierce;
    //���� ��� �� ����
    private bool colTrigger;

    //�߻� ����
    private Rigidbody2D rb;
    //���� ���
    private EnemyMovement target;
    //���� ����Ʈ ������
    [SerializeField] private GameObject effectPrefabs;
    #endregion

    #region InitMethod
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = transform.localScale * (1 + (0.1f * bulletLevel));

    }
    /// <summary>
    /// �߻�ü ������ �ʱ�ȭ
    /// </summary>
    /// <param name="damage">������</param>
    /// <param name="velocity">�߻�ü �߻� �ӵ�</param>
    /// <param name="dir">�߻�ü �߻� ����</param>
    /// <param name="level">�߻�ü ����</param>
    /// <param name="type">�߻�ü Ÿ��</param>
    /// <param name="pierce">�߻�ü �����</param>
    /// <param name="posx">����Ʈ ��ġ x</param>
    /// <param name="posy">����Ʈ ��ġ y</param>
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
    /// �߻�ü �浹
    /// </summary>
    /// <param name="collision">�浹 ���</param>
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
    /// �Ϲ� Ÿ������Ʈ ���� �� ��ġ ����
    /// </summary>
    private void SetEffect()
    {
       var obj = Instantiate(effectPrefabs, null);
       obj.transform.position = target.transform.position; 
    }

    /// <summary>
    /// ���� Ÿ�� ����Ʈ ���� �� ��ġ ����
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
