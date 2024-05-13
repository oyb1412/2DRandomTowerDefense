using UnityEngine;
using UnityEngine.UI;
using static TowerManager;

/// <summary>
/// 타워 생성 매니저
/// </summary>
public class CreateManager : MonoBehaviour
{
    #region Variable
    //타워 생성 UI 프리펩
    [SerializeField]private GameObject createPanel;
    //타워 판매 UI 프리펩
    [SerializeField] private GameObject deletePanel;
    //타워 마우스 enter 커서 이미지
    [SerializeField] private Texture2D towerMouseCursor;
    //셀 마우스 enter 커서 이미지
    [SerializeField] private Texture2D CellMouseCursor;
    //타워 판매 텍스트
    [SerializeField] private Text deleteText;
    //타워 판매 리워드 텍스트
    [SerializeField] private Text goldText;
    //현재 선택한 셀
    private CellManager targetCell;
    //마우스 enter 셀
    private CellManager mouseOnCell;
    //각 타워 프리펩
    [SerializeField] private GameObject[] towerPrefabs;
    //선택 중인 타워
    private TowerManager targetTower;
    //선택 중인 타워(복수)
    [SerializeField] private TowerManager[] targetTowers;
    //연속 클릭 횟수
    private int click;
    //타워 이동시 가장 가까운 셀을 검출할 범위
    [SerializeField] private float scanRange;
    //셀 레이어
    [SerializeField] private LayerMask layer;
    //공격 범위내의 적 저장
    private RaycastHit2D[] targets;
    //공격 대상 저장
    private Transform target;
    //건설 중 여부
    private bool createTrigger;
    //건설 시 연속클릭 방지 타이머
    private float createTimer;
    //판매 중 여부
    private bool DeleteTrigger;
    //판매 시 연속클릭 방지 타이머
    private float DeleteTimer;
    //타워 이동 중 여부
    private bool towerMoveTrigger;
    //타워 이동 시 연속클릭 방지 타이머
    private float towerMoveTimer;

    #endregion
    private void Awake()
    {
        createPanel.SetActive(false);
        deletePanel.SetActive(false);
        targetTowers = new TowerManager[2];
    }

    #region UpdateMethod
    private void Update()
    {
        if (!Manager.Instance.IsLive || SkillManager.instance.SkillRangeObject.activeSelf)
            return;

        MoveTower();
        GetTowersNum();
        SelectCell();
        DeleteTower();
    }

    private void LateUpdate()
    {
        if (!Manager.Instance.IsLive ||SkillManager.instance.SkillRangeObject.activeSelf)
            return;

        SelectTower();
        MouseOnCell();
        MouseOnTower();

        if(!MouseManager.instance.MouseRayCast("Field") && !MouseManager.instance.MouseRayCast("Tower"))
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if (Manager.Instance.GetGold() >= 15)
            goldText.color = Color.white;
        else
            goldText.color = Color.red;
    }
    #endregion

    #region TowerMethod
    /// <summary>
    /// 타 셀로 타워 이동
    /// </summary>
    private void MoveTower()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && MouseManager.instance.MouseRayCast("Tower") && !towerMoveTrigger
            && towerMoveTimer == 0)
        {
            towerMoveTrigger = true;
            targetTower = MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>();
            var cell = Physics2D.CircleCast(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer);
            cell.transform.GetComponent<CellManager>().UseSell = false;
        }
        if (towerMoveTrigger)
        {
            towerMoveTimer++;
            targetTower.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 1f));
            targetTower.Animator.SetTrigger("Stun");
            targetTower.IsLive = false;
            //플레이어 공격 불가

            var targetCell = Physics2D.CircleCast(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer).
                transform.GetComponent<CellManager>();

            if (Input.GetKeyDown(KeyCode.Alpha4) && towerMoveTimer > 1.5f && !targetCell.UseSell)
            {
                //다시한번 키를 누르면 레이캐스트 서클로 가장 가까운 cell정보를 가져옴.
                var cell = Physics2D.CircleCast(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer);
                cell.transform.GetComponent<CellManager>().UseSell = true;
                targetTower.transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y, -2f);
                towerMoveTimer = 0;
                towerMoveTrigger = false;
                targetTower.IsLive = true;

            }
            //타워의 위치를 새로운 위치로 고정

        }
    }

    /// <summary>
    /// 타워 선택
    /// </summary>
    private void SelectTower()
    {
        if (Input.GetMouseButtonDown(0) && click == 0 && targetTowers.Length > 0 ||
            Input.GetKeyDown(KeyCode.Alpha3) && click == 0 && targetTowers.Length > 0)
        {
            if (MouseManager.instance.MouseRayCast("Tower") && targetTower.MaxTowerLevel > 0)
            {
                targetTowers[0] = MouseManager.instance.MouseRayCast("Tower").GetComponent<TowerManager>();
                if (targetTowers[0].canLevelUp && targetTowers[0].CurrentTowerLevel < targetTowers[0].MaxTowerLevel)
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

                    if (targetTowers[0].canLevelUp && targetTowers[1].canLevelUp && (targetTowers[0].CurrentTowerLevel == targetTowers[1].CurrentTowerLevel) &&
                        targetTowers[0].towerType == targetTowers[1].towerType && targetTowers[1].CurrentTowerLevel < targetTowers[1].MaxTowerLevel)
                    {
                        targetTowers[0].TowerLevelUp();
                        targetTowers[1].gameObject.SetActive(false);
                        targets = Physics2D.CircleCastAll(targetTowers[1].transform.position, scanRange, Vector2.zero, 0f, layer);
                        target = MouseManager.instance.GetNearTarget(targets);
                        var obj = target.GetComponent<CellManager>();
                        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Upgrade);

                        obj.UseSell = false;
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

    /// <summary>
    /// 타워 판매
    /// </summary>
    private void DeleteTower()
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
                    deleteText.text = "+" + (targetTower.CurrentTowerLevel+1) * 5 + "g";
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
            Manager.Instance.SetGold((targetTower.CurrentTowerLevel+1) * 5);
            targets = Physics2D.CircleCastAll(targetTower.transform.position, scanRange, Vector2.zero, 0f, layer);
            target = MouseManager.instance.GetNearTarget(targets);
            var obj = target.GetComponent<CellManager>();
            obj.UseSell = false;
            targetTower.DeleteEffect();
            targetTower.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 타워 마우스 Enter
    /// </summary>
    private void MouseOnTower()
    {
        if (MouseManager.instance.MouseRayCast("Tower"))
        {
            Cursor.SetCursor(towerMouseCursor, new Vector2(towerMouseCursor.width / 2, towerMouseCursor.height / 2), CursorMode.Auto);
        }
    }

    /// <summary>
    /// 레벨업 가능한 타워 검출
    /// </summary>
    private void GetTowersNum()
    {
        GameObject[] tower = GameObject.FindGameObjectsWithTag("Tower");
        TowerManager[] info = new TowerManager[tower.Length];

        for (int i = 0; i < tower.Length; i++)
        {
            info[i] = tower[i].GetComponent<TowerManager>();
        }

        for (int i = 0; i < tower.Length; i++)
        {
            for (int j = i + 1; j < tower.Length; j++)
            {
               if (info[i].CurrentTowerLevel == info[j].CurrentTowerLevel)
               {
                   info[i].canLevelUp = true;
                   info[j].canLevelUp = true;
               }                    
            }
        }
    }

    /// <summary>
    /// 랜덤 타워 생성
    /// </summary>
    /// <returns></returns>
    private int StateUpgradeRandom()
    {
        int ran = Random.Range(0, 105);

        if (TowerManager.StateUpgrade1)
        {
            if (ran < 44)
                ran = 0;
            else if (ran >= 44 && ran < 67)
                ran = 1;
            else if (ran >= 67 && ran < 90)
                ran = 2;
            else if (ran >= 90 && ran < 95)
                ran = 3;
            else if (ran >= 95 && ran < 100)
                ran = 4;
            else if (ran >= 100 && ran < 104)
                ran = 5;
            else
                ran = 6;
        }
        else if (TowerManager.StateUpgrade2)
        {
            if (ran < 23)
                ran = 0;
            else if (ran >= 23 && ran < 67)
                ran = 1;
            else if (ran >= 67 && ran < 90)
                ran = 2;
            else if (ran >= 90 && ran < 95)
                ran = 3;
            else if (ran >= 95 && ran < 100)
                ran = 4;
            else if (ran >= 100 && ran < 104)
                ran = 5;
            else
                ran = 6;
        }
        else if (TowerManager.StateUpgrade3)
        {
            if (ran < 23)
                ran = 0;
            else if (ran >= 23 && ran < 46)
                ran = 1;
            else if (ran >= 46 && ran < 90)
                ran = 2;
            else if (ran >= 90 && ran < 95)
                ran = 3;
            else if (ran >= 95 && ran < 100)
                ran = 4;
            else if (ran >= 100 && ran < 104)
                ran = 5;
            else
                ran = 6;

        }
        else if (TowerManager.StateUpgrade4)
        {
            if (ran < 27)
                ran = 0;
            else if (ran >= 27 && ran < 54)
                ran = 1;
            else if (ran >= 54 && ran < 81)
                ran = 2;
            else if (ran >= 81 && ran < 91)
                ran = 3;
            else if (ran >= 91 && ran < 97)
                ran = 4;
            else if (ran >= 97 && ran < 103)
                ran = 5;
            else
                ran = 6;
        }
        else
        {
            if (ran < 30)
                ran = 0;
            else if (ran >= 30 && ran < 60)
                ran = 1;
            else if (ran >= 60 && ran < 90)
                ran = 2;
            else if (ran >= 90 && ran < 95)
                ran = 3;
            else if (ran >= 95 && ran < 100)
                ran = 4;
            else if (ran >= 100 && ran < 104)
                ran = 5;
            else
                ran = 6;
        }
        if (Manager.Instance.getSuperTower)
        {
            ran = 6;
            Manager.Instance.getSuperTower = false;
        }

        return ran;
    }

    /// <summary>
    /// 타워 생성
    /// </summary>
    private void CreateTower() {
        if (Manager.Instance.GetGold() >= 15) {
            createPanel.SetActive(false);
            createPanel.transform.position = new Vector3(20f, 20f, 0f);
            targetCell.UseSell = true;
            TowerManager trans = FactoryManager.Instance.CreateTower(towerPrefabs[StateUpgradeRandom()], new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, -2f));
            targetTower = trans;
            targetTower.CurrentTowerLevel = 0;
            targetTower.CreateLevelEffect();
            targetTower.CreateEffect();
            AudioManager.instance.PlayerSfx(AudioManager.Sfx.Create);

            if (TowerManager.StateUpgrade1) {
                if (targetTower.towerType == Tower.BowTower || targetTower.towerType == Tower.MasterBowTower
                    || targetTower.towerType == Tower.SuperTower) {
                    targetTower.TowerDamage *= 1.5f;
                    targetTower.TowerRange *= 1.5f;
                    targetTower.TowerAttackSpeed -= 0.1f;
                }
            }
            if (TowerManager.StateUpgrade2) {
                if (targetTower.towerType == Tower.SwordTower || targetTower.towerType == Tower.MasterSwordTower) {
                    targetTower.TowerDamage *= 1.5f;
                    targetTower.TowerRange *= 1.5f;
                    targetTower.TowerAttackSpeed -= 0.1f;
                }
            }
            if (TowerManager.StateUpgrade3) {
                if (targetTower.towerType == Tower.AxeTower || targetTower.towerType == Tower.MasterAxeTower) {
                    targetTower.TowerDamage *= 1.5f;
                    targetTower.TowerRange *= 1.5f;
                    targetTower.TowerAttackSpeed -= 0.1f;
                }
            }
            if (TowerManager.StateUpgrade4) {
                if (targetTower.towerType == Tower.FireTower || targetTower.towerType == Tower.MasterFireTower
                    || targetTower.towerType == Tower.SlowTower || targetTower.towerType == Tower.MasterSlowTower
                    || targetTower.towerType == Tower.StunTower || targetTower.towerType == Tower.MasterStunTower
                    || targetTower.towerType == Tower.SuperTower) {
                    targetTower.TowerDamage *= 1.5f;
                    targetTower.TowerRange *= 1.5f;
                    targetTower.TowerAttackSpeed -= 0.1f;
                }
            }
            if (TowerManager.StateUpgrade5) {
                targetTower.TowerPierce += 1;
            }
            if (TowerManager.StateUpgrade6) {
                targetTower.TowerProjectile = 1;
            }
            Manager.Instance.SetGold(-15);
        }
    }
    #endregion

    #region CellMethod

    /// <summary>
    /// 셀 선택
    /// </summary>
    private void SelectCell() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Alpha1)) {
            if (MouseManager.instance.MouseRayCast("Field")) {
                targetCell = MouseManager.instance.MouseRayCast("Field").GetComponent<CellManager>();
                if (!targetCell.UseSell) {
                    createTrigger = true;
                    createPanel.SetActive(true);
                    createPanel.transform.position = targetCell.transform.position;
                }
            }
        }
        if (createTrigger)
            createTimer += Time.deltaTime;

        if (createTrigger && Input.GetKeyDown(KeyCode.Alpha1) && createTimer > 0.2f) {
            createTimer = 0;
            createTrigger = false;
            CreateTower();
        }
        if (createPanel && Input.GetMouseButtonDown(1)) {
            createPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 셀 마우스 Enter
    /// </summary>
    private void MouseOnCell() {
        if (MouseManager.instance.MouseRayCast("Field")) {
            mouseOnCell = MouseManager.instance.MouseRayCast("Field").GetComponent<CellManager>();
            if (!mouseOnCell.UseSell) {
                mouseOnCell.Spriter.color = Color.yellow;
                Cursor.SetCursor(CellMouseCursor, new Vector2(CellMouseCursor.width / 2, CellMouseCursor.height / 2), CursorMode.Auto);
            }
        }
    }
    #endregion
}
