using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    public Transform tr;
    public ObjectFactory enemyFactory;
    public ObjectFactory towerFactory;
    public ObjectFactory bulletFactory;

    public static FactoryManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void CreateEnemy(GameObject obj)
    {
        enemyFactory.SpawnEnemy(obj, tr);
    }

    public TowerManager CreateTower(GameObject obj, Vector3 pos)
    {
        return towerFactory.SpawnTower(obj, tr, pos);
    }

    public Bullet CreateBullet(GameObject obj, Vector3 pos, Vector3 rotation)
    {
        return bulletFactory.SpawnBullet(obj, tr, pos,rotation);
    }
}
