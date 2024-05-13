using UnityEngine;

/// <summary>
/// ��ų ������ �浹
/// </summary>
public class LazerAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        var targetEnemy = collision.GetComponent<EnemyMovement>();
        targetEnemy.IsLongStun = true;
        targetEnemy.SetHp(-10);
    }
}
