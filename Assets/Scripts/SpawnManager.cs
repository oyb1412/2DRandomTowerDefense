using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 애너미 스폰 관리
/// </summary>
public class SpawnManager : MonoBehaviour
{
    #region Variable
    public static SpawnManager instance;
    //애너미 프리펩
    [SerializeField] private GameObject[] enemyPrefabs;
    //보스 애너미 프리펩
    [SerializeField] private GameObject[] bossPrefabs;
    //승마 애너미 프리펩
    [SerializeField] private GameObject[] horseEnemyPrefabs;
    //다음 라운드시작 버튼 이미지
    [SerializeField] private GameObject nextRoundButtonImage;
    //애너미 스폰 타이머
    private float spawnTime;
    //승마 애너미 스폰 타이머
    private float horseSpawnTime;
    //애너미 스폰 카운트
    private int spawnCount;
    //승마 애너미 스폰 카운트
    private int horseSpawnCount;
    //보스 애너미 스폰 카운트
    private int bossSpawnCount;
    //현재 라운드 경과 시간
    private float roundTime;
    //현재 라운드 최대 시간
    private float maxRountTime = 40;
    //다음 라운드 텍스트
    [SerializeField] private Text nextRoundText;
    //현재 라운드 텍스트
    [SerializeField] private Text currentRoundText;
    //현재 게임 레벨
    public int CurrentLevel { get; private set; } = 1;
    //게임 승리 여부
    public bool IsClear { get; private set; }
    #endregion

    private void Awake()
    {
        instance = this;
        roundTime = maxRountTime;
        nextRoundButtonImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!Manager.Instance.IsLive || IsClear)
            return;

        nextRoundText.text = "Next Round : " +Mathf.FloorToInt(roundTime);
        currentRoundText.text = "Level :" + CurrentLevel;
        if(CurrentLevel == 24 && roundTime < 35)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
                IsClear = true;
        }
        if(roundTime >= 40)
        {
            if (CurrentLevel == 1)
                Manager.Instance.IsUpgradeState = true;

        }
        if(roundTime <= 20)
        {
            nextRoundButtonImage.gameObject.SetActive(true);
        }    

        if (roundTime > 0)
            roundTime -= Time.deltaTime;
        else
        {
            if(nextRoundButtonImage.activeSelf)
                nextRoundButtonImage.gameObject.SetActive(false);

            roundTime = 40;
            spawnCount = 0;
            horseSpawnCount = 0;
            bossSpawnCount = 0;
            CurrentLevel++;
        }

        if (spawnCount < 20)
            SetSpawn(CurrentLevel / 2);

        if (horseSpawnCount < 10 && CurrentLevel % 3 == 0)
            SetHorseSpawn();
    }

    /// <summary>
    /// 다음 라운드 시작(콜백으로 호출)
    /// </summary>
    public void NextRoundClick()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);

        var time = roundTime;
        roundTime = 40;
        spawnCount = 0;
        horseSpawnCount = 0;
        bossSpawnCount = 0;
        CurrentLevel++;
        Manager.Instance.SetGold((int)time);
        nextRoundButtonImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 현재 레벨에 맞는 애너미 스폰
    /// </summary>
    /// <param name="level">레벨</param>
    void SetSpawn(int level)
    {
        spawnTime += Time.deltaTime;

        if (spawnTime > 0.5f)
        {
            spawnTime = 0;
            FactoryManager.Instance.CreateEnemy(enemyPrefabs[level]);
            spawnCount++;
        }
        if (CurrentLevel % 8 == 0 && bossSpawnCount == 0)
        {
            FactoryManager.Instance.CreateEnemy(bossPrefabs[(CurrentLevel / 8) - 1]);
            bossSpawnCount++;
        }
    }

    /// <summary>
    /// 현재 레벨에 맞는 승마 애너미 스폰
    /// </summary>
    void SetHorseSpawn()
    {
        horseSpawnTime += Time.deltaTime;
        if (horseSpawnTime > 1f)
        { 
            horseSpawnTime = 0;
            FactoryManager.Instance.CreateEnemy(horseEnemyPrefabs[(CurrentLevel / 3) - 1]);
            horseSpawnCount++;
        }
    }
}
