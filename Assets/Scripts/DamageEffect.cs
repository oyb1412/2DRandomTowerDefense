using UnityEngine;

/// <summary>
/// ���� ���� �浹 ȿ��
/// </summary>
public class DamageEffect : MonoBehaviour
{
    //�ڵ� ���� Ÿ�̸�
    private float damageTimer;
    //����Ʈ Ÿ��
    public int EffectType { get; set; }
    //����Ʈ ������
    public float EffectDamage { get; set; }
    //����Ʈ ����
    public int EffectLevel { get; set; }

    private void Awake()
    {
        transform.localScale = transform.localScale * (1 + (0.1f * EffectLevel));
    }
    private void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer > 0.5f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || !collision)
            return;

        var target = collision.GetComponent<EnemyMovement>();

            switch (EffectType)
            {
                case (int)TowerManager.Tower.FireTower:
                    target.SetHp(-EffectDamage);
                    break;

                case (int)TowerManager.Tower.SlowTower:
                    target.SetHp(-EffectDamage);
                    target.IsSlow = true;
                    break;

            case (int)TowerManager.Tower.StunTower:
                target.SetHp(-EffectDamage);
                target.animator.SetTrigger("Stun");
                target.IsStun = true;
                break;
            }
 
    }
}
