using UnityEngine;

/// <summary>
/// 스킬 레이저 충돌
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
