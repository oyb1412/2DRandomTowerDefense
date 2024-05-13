using UnityEngine;

public class ObjectFactory : Factory<GameObject>
{
    public override Bullet CreateBullet(GameObject type)
    {
        Bullet bullet;
        bullet = Instantiate(type).GetComponent<Bullet>();
        return bullet;
    }

    public override EnemyMovement CreateEnemy(GameObject type)
    {
        EnemyMovement enemy;
        enemy = Instantiate(type).GetComponent<EnemyMovement>();
        return enemy;
    }

    public override TowerManager CreateTower(GameObject type)
    {
        TowerManager tower;
        tower = Instantiate(type).GetComponent<TowerManager>();
        return tower;
    }
}
