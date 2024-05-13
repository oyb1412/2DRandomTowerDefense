using UnityEngine;

/// <summary>
/// 오브젝트 생성 팩토리 시스템
/// </summary>
public abstract class Factory<T> : MonoBehaviour
{
    public EnemyMovement SpawnEnemy(T type, Transform parent)
    {
        EnemyMovement unit = CreateEnemy(type);
        unit.transform.SetParent(parent);
        return unit;
    }

    public TowerManager SpawnTower(T type, Transform parent, Vector3 pos)
    {
        TowerManager unit = CreateTower(type);
        unit.transform.SetParent(parent);
        unit.transform.position = pos;
        return unit;
    }

    public Bullet SpawnBullet(T type, Transform parent, Vector3 pos,Vector3 rotation)
    {
        Bullet unit = CreateBullet(type);
        unit.transform.SetParent(parent);
        unit.transform.position = pos;
        unit.transform.rotation = Quaternion.FromToRotation(Vector3.up, rotation);
        return unit;
    }

    public abstract EnemyMovement CreateEnemy(T type);
    public abstract TowerManager CreateTower(T type);
    public abstract Bullet CreateBullet(T type);
}
