using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 게임 매니저
/// </summary>
public class Manager : MonoBehaviour
{
    #region Variable
    public static Manager Instance;

    //각 스킬 업그레이드 UI 판넬
    [SerializeField] private GameObject[] upgradeStateInfo;
    //스킬 업그레이드 패런츠 UI 판넬
    public GameObject stateUpgradeObj;
    //게임오버 UI
    [SerializeField] private GameObject gameOverUI;
    //게임승리 UI
    [SerializeField] private GameObject gameClearUI;
    //보유 골드 표기 텍스트
    [SerializeField] private Text goldText;
    //보유 골드
    [SerializeField] private int currentGold;
    //보유 생명력
    [SerializeField] private int currentHearts;
    //보유 생명력 표기 UI
    [SerializeField] private GameObject[] hearts;
    //게임 오버시 현재 라운드 표기 텍스트
    [SerializeField] private GameObject gameOverText;
    //게임 진행 여부
    public bool IsLive { get; set; } = true;
    //게임 오버 여부
    private bool isOver;
    //업그레이드 선택 중 여부
    public bool IsUpgradeState { get; set; }
    //슈퍼타워 건설 가능 여부
    public bool getSuperTower { get; set; }
    #endregion

    private void Awake()
    {
        Instance = this;
        gameOverUI.SetActive(false);
        gameClearUI.SetActive(false);
    }

    #region UpdateMethod
    private void Update()
    {
        if (!IsLive)
            return;

        goldText.text = "x" + currentGold;
        if(currentHearts == 2)
            hearts[2].SetActive(false);
        else if(currentHearts == 1)
            hearts[1].SetActive(false);
        else if(currentHearts == 0 && !isOver)
        {
            AudioManager.instance.PlayerBgm(false);
            AudioManager.instance.PlayerSfx(AudioManager.Sfx.Lose);

            hearts[0].SetActive(false);
            gameOverUI.SetActive(true);
            isOver = true;
            IsLive = false;
            gameOverText.GetComponent<Text>().text = "MaxRound :" + SpawnManager.instance.CurrentLevel;
            gameOverUI.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        }

        if(SpawnManager.instance.IsClear)
        {
            AudioManager.instance.PlayerBgm(false);
            gameClearUI.SetActive(true);
            IsLive = false;
            gameClearUI.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        }


        if (IsUpgradeState)
        {
            PickUpStateUpgradeObject();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }


    }
    private void LateUpdate()
    {
        if (stateUpgradeObj.transform.localScale.x < 0.1f)
        {
            stateUpgradeObj.gameObject.SetActive(false);
        }
    }
    #endregion

    #region UpgradeMethod
    /// <summary>
    /// 랜덤 업그레이드 UI 출력
    /// </summary>
    private void PickUpStateUpgradeObject()
    {
        stateUpgradeObj.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        stateUpgradeObj.gameObject.SetActive(true);
        for(int i = 0;i<upgradeStateInfo.Length;i++)
        {
            if(upgradeStateInfo[i] != null)
            upgradeStateInfo[i].gameObject.SetActive(false);
        }
        int[] num = new int[3];
        while (true)
        {
            num[0] = Random.Range(0, 6);
            num[1]= Random.Range(0, 6);
            num[2] = Random.Range(0, 6);

            if (num[0] != num[1] && num[0] != num[2] && num[1] != num[2])
            {
                upgradeStateInfo[num[0]].gameObject.SetActive(true);
                upgradeStateInfo[num[1]].gameObject.SetActive(true);
                upgradeStateInfo[num[2]].gameObject.SetActive(true);

                break;
            }
               
         }
         stateUpgradeObj.transform.DOScaleX(1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleY(1f, 0.3f));
         IsUpgradeState = false;
         IsLive = false;
    }

    /// <summary>
    /// 스테이트1 업그레이드(콜백으로 호출)
    /// </summary>
    public void StateUpgrade1()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade1 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// 스테이트2 업그레이드(콜백으로 호출)
    /// </summary>
    public void StateUpgrade2()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade2 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// 스테이트3 업그레이드(콜백으로 호출)
    /// </summary>
    public void StateUpgrade3()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade3 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// 스테이트4 업그레이드(콜백으로 호출)
    /// </summary>
    public void StateUpgrade4()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade4 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// 스테이트5 업그레이드(콜백으로 호출)
    /// </summary>
    public void StateUpgrade5()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade5 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// 스테이트6 업그레이드(콜백으로 호출)
    /// </summary>
    public void StateUpgrade6()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade6 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }
    #endregion

    /// <summary>
    /// 골드 변경
    /// </summary>
    /// <param name="gold">변경할 골드</param>
    public void SetGold(int gold)
    {
        currentGold += gold;
    }

    /// <summary>
    /// 골드 반환
    /// </summary>
    /// <returns>현재 골드</returns>
    public int GetGold()
    {
        return currentGold;
    }

    /// <summary>
    /// 생명력 변경
    /// </summary>
    /// <param name="heart">변경할 생명력</param>
    public void SetHeart(int heart)
    {
        currentHearts += heart;
    }

    /// <summary>
    /// 게임 재시작(콜백으로 호출)
    /// </summary>
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 게임 Pause 설정 및 해제(콜백으로 호출)
    /// </summary>
    public void StopButtonEnter()
    {
        if(IsLive)
            IsLive = false;
        else
            IsLive = true;
    }
}
