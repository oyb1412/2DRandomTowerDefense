## **📃핵심 기술**

### ・타일맵과의 상호작용

🤔**WHY?**

타일맵 위에 직접 타워를 배치해야 하기 때문에, 타일맵과의 각종 상호작용 로직의 필요성을 느꼈기 때문

🤔**HOW?**

 관련 코드

- MouseManager
    
    ```csharp
    using UnityEngine;
    using UnityEngine.Tilemaps;
    
    public class MouseManager : MonoBehaviour
    {
    	public Collider2D MouseRayCast(string tag)
    	{
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,0f);
        if (hit.collider != null && hit.collider.CompareTag(tag))
        {
            return hit.collider;
        }
        else
            return null;
    	}
    }
    ```
    

🤓**Result!**

선택한 타일맵 객체의 정보를 받아올 수 있게 되어 타워 생성, 제거, 이동 등의 로직의 구현이 보다 원할해짐

### ・적 유닛의 자동적인 이동

🤔**WHY?**

NavmeshAgent를 이용한 이동이 아닌, 더욱더 최적화가 가능한 이동로직의 필요성을 느꼈기 때문

🤔**HOW?**

 관련 코드

- EnemyMovement
    
    ```csharp
    using System.Collections;
    using UnityEngine;
    
    public class EnemyMovement : MonoBehaviour
    {
    
        public Transform[] wayPoints;
        public int currentWayPoint;
        public float speed;
        public Vector2 nextDir;
       
    
        void Movement()
        {
            if (!isLive || !Manager.Instance.isLive)
                return;
    
            if(currentWayPoint < wayPoints.Length)
            {
                if(Vector2.Distance(transform.position, wayPoints[currentWayPoint].position) > 0.05f )
                {
                    nextDir = wayPoints[currentWayPoint].position - transform.position;
                    nextDir = nextDir.normalized;
    
                    transform.Translate(nextDir * speed * Time.deltaTime);
    
                }
                else
                {
                    currentWayPoint++;
                }
            }
    
            if(Vector2.Distance(transform.position, wayPoints[10].position) < 0.05f)
            {
                Manager.Instance.SetHeart(-1);
                gameObject.SetActive(false);
            }
        }
       
    }
    ```
    

🤓**Result!**

NevmeshAgent를 사용하지 않고 적의 이동경로를 지정해 그 경로를 따라 이동하도는 로직을 구현해, 최적화 성공

### ・타워 랜덤생성, 판매, 합성, 위치변경

🤔**WHY?**

랜덤 타워디펜스의 핵심적인 로직인 타워 랜덤생성과 합성, 타워의 위치변경의 구현

🤔**HOW?**

 관련 코드

- CreateManager(생성)
    
    ```csharp
    using UnityEngine;
    using static TowerManager;
    using UnityEngine.UI;
    public class CreateManager : MonoBehaviour
    {
      public void CreateTower()
      {
          if (Manager.Instance.GetGold() >= 15)
          {
              createPanel.SetActive(false);
              createPanel.transform.position = new Vector3(20f, 20f, 0f);
              targetCell.useSell = true;
              TowerManager trans = FactoryManager.Instance.CreateTower(towerPrefabs[StateUpgradeRandom()], new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, -2f));
              targetTower = trans;
              targetTower.currentTowerLevel = 0;
              targetTower.CreateLevelEffect();
              targetTower.CreateEffect();
              AudioManager.instance.PlayerSfx(AudioManager.Sfx.Create);
    
              if (TowerManager.stateUpgrade1)
              {
                  if(targetTower.towerType == Tower.BowTower || targetTower.towerType == Tower.MasterBowTower
                      || targetTower.towerType == Tower.SuperTower)
                  {
                      targetTower.towerDamager *= 1.5f;
                      targetTower.towerRange *= 1.5f;
                      targetTower.towerAttackSpeed -= 0.1f;
                  }
              }
              if (TowerManager.stateUpgrade2)
              {
                  if (targetTower.towerType == Tower.SwordTower || targetTower.towerType == Tower.MasterSwordTower)
                  {
                      targetTower.towerDamager *= 1.5f;
                      targetTower.towerRange *= 1.5f;
                      targetTower.towerAttackSpeed -= 0.1f;
                  }
              }
              if (TowerManager.stateUpgrade3)
              {
                  if (targetTower.towerType == Tower.AxeTower || targetTower.towerType == Tower.MasterAxeTower)
                  {
                      targetTower.towerDamager *= 1.5f;
                      targetTower.towerRange *= 1.5f;
                      targetTower.towerAttackSpeed -= 0.1f;
                  }
              }
              if (TowerManager.stateUpgrade4)
              {
                  if (targetTower.towerType == Tower.FireTower || targetTower.towerType == Tower.MasterFireTower
                      || targetTower.towerType == Tower.SlowTower || targetTower.towerType == Tower.MasterSlowTower
                      || targetTower.towerType == Tower.StunTower || targetTower.towerType == Tower.MasterStunTower
                      || targetTower.towerType == Tower.SuperTower)
                  {
                      targetTower.towerDamager *= 1.5f;
                      targetTower.towerRange *= 1.5f;
                      targetTower.towerAttackSpeed -= 0.1f;
                  }
              }
              if (TowerManager.stateUpgrade5)
              {
                  targetTower.towerPierce += 1;
              }
              if (TowerManager.stateUpgrade6)
              {
                  targetTower.towerProjectile = 1;
              }
                  Manager.Instance.SetGold(-15);
          }
      }
    }
    ```
    
- CreateManager(판매)
    
    ```csharp
    using UnityEngine;
    using static TowerManager;
    using UnityEngine.UI;
    public class CreateManager : MonoBehaviour
    {
     void DeleteTower()
     {
         if (Input.GetKeyDown(KeyCode.Alpha2))
         {
             if (MouseManager.instance.MouseRayCast("Tower"))
             {
                 targetTower = MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>();
                 if (targetTower)
                 {
                     DeleteTrigger = true;
                     deletePanel.SetActive(true);
                     deleteText.text = "+" + (targetTower.currentTowerLevel+1) * 5 + "g";
                     deletePanel.transform.position = new Vector2(targetTower.transform.position.x, targetTower.transform.position.y + 1f);
                 }
             }
         }
    
         if (DeleteTrigger)
         {
             DeleteTimer += Time.deltaTime;
             if(Input.GetMouseButtonDown(1))
             {
                 DeleteTimer = 0;
                 DeleteTrigger = false;
                 deletePanel.SetActive(false);
             }
         }
         if (DeleteTrigger && Input.GetKeyDown(KeyCode.Alpha2) && DeleteTimer > 0.2f)
         {
             DeleteTimer = 0;
             DeleteTrigger = false;
             deletePanel.SetActive(false);
             Manager.Instance.SetGold((targetTower.currentTowerLevel+1) * 5);
             targets = Physics2D.CircleCastAll(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer);
             target = MouseManager.instance.GetNearTarget(targets);
             var obj = target.GetComponent<CellManager>();
             obj.useSell = false;
             targetTower.DeleteEffect();
             targetTower.gameObject.SetActive(false);
         }
     }
    }
    ```
    
- CreateManager(합성)
    
    ```csharp
    using UnityEngine;
    using static TowerManager;
    using UnityEngine.UI;
    public class CreateManager : MonoBehaviour
    {
        void SelectTower()
        {
            if (Input.GetMouseButtonDown(0) && click == 0 && targetTowers.Length > 0 ||
                Input.GetKeyDown(KeyCode.Alpha3) && click == 0 && targetTowers.Length > 0)
            {
                if (MouseManager.instance.MouseRayCast("Tower") && targetTower.maxTowerLevel > 0)
                {
                    targetTowers[0] = MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>();
                    if (targetTowers[0].canLevelUp && targetTowers[0].currentTowerLevel < targetTowers[0].maxTowerLevel)
                    {
                        targetTowers[0].CreateLevelUPArrow();
                        click++;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && click == 1 ||
                Input.GetKeyDown(KeyCode.Alpha3) && click == 1)
            {
                if (MouseManager.instance.MouseRayCast("Tower"))
                {
                    if (targetTowers[0] != MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>())
                    {
                        targetTowers[1] = MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>();
    
                        if (targetTowers[0].canLevelUp && targetTowers[1].canLevelUp && (targetTowers[0].currentTowerLevel == targetTowers[1].currentTowerLevel) &&
                            targetTowers[0].towerType == targetTowers[1].towerType && targetTowers[1].currentTowerLevel < targetTowers[1].maxTowerLevel)
                        {
                            targetTowers[0].TowerLevelUp();
                            targetTowers[1].gameObject.SetActive(false);
                            targets = Physics2D.CircleCastAll(targetTowers[1].transform.position, scanRange, Vector2.zero, 0f, layer);
                            target = MouseManager.instance.GetNearTarget(targets);
                            var obj = target.GetComponent<CellManager>();
                            AudioManager.instance.PlayerSfx(AudioManager.Sfx.Upgrade);
    
                            obj.useSell = false;
                            click = 0;
                        }
                    }
                }
            }
            if(Input.GetMouseButtonDown(1) && click == 1)
            {
                targetTowers[0].DeleteLevelUPArrow();
                click = 0;
            }
        }
    }
    ```
    
- CreateManager(위치이동)
    
    ```csharp
    using UnityEngine;
    using static TowerManager;
    using UnityEngine.UI;
    public class CreateManager : MonoBehaviour
    {
     void MoveTower()
     {
         if (Input.GetKeyDown(KeyCode.Alpha4) && MouseManager.instance.MouseRayCast("Tower") && !towerMoveTrigger
             && towerMoveTimer == 0)
         {
             towerMoveTrigger = true;
             targetTower = MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>();
             var cell = Physics2D.CircleCast(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer);
             cell.transform.GetComponent<CellManager>().useSell = false;
         }
         if (towerMoveTrigger)
         {
             towerMoveTimer++;
             targetTower.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                 Input.mousePosition.y, 1f));
             targetTower.animator.SetTrigger("Stun");
             targetTower.isLive = false;
             //플레이어 공격 불가
    
             var targetCell = Physics2D.CircleCast(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer).
                 transform.GetComponent<CellManager>();
    
             if (Input.GetKeyDown(KeyCode.Alpha4) && towerMoveTimer > 1.5f && !targetCell.useSell)
             {
                 //다시한번 키를 누르면 레이캐스트 서클로 가장 가까운 cell정보를 가져옴.
                 var cell = Physics2D.CircleCast(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer);
                 cell.transform.GetComponent<CellManager>().useSell = true;
                 targetTower.transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y, -2f);
                 towerMoveTimer = 0;
                 towerMoveTrigger = false;
                 targetTower.isLive = true;
    
             }
             //타워의 위치를 새로운 위치로 고정
         }
     }
    }
    ```
    

🤓**Result!**

6종의 타워중 랜덤한 타워를 확률에 맞게 생성, 선택한 타워를 판매, 동일 종류,레벨의 타워를 합성, 타워의 위치 재변경 등 타워디펜스에 필요한 로직을 구현해, 게임성 상승
