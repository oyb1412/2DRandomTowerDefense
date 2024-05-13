using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ֳʹ� ���� ����
/// </summary>
public class SpawnManager : MonoBehaviour
{
    #region Variable
    public static SpawnManager instance;
    //�ֳʹ� ������
    [SerializeField] private GameObject[] enemyPrefabs;
    //���� �ֳʹ� ������
    [SerializeField] private GameObject[] bossPrefabs;
    //�¸� �ֳʹ� ������
    [SerializeField] private GameObject[] horseEnemyPrefabs;
    //���� ������� ��ư �̹���
    [SerializeField] private GameObject nextRoundButtonImage;
    //�ֳʹ� ���� Ÿ�̸�
    private float spawnTime;
    //�¸� �ֳʹ� ���� Ÿ�̸�
    private float horseSpawnTime;
    //�ֳʹ� ���� ī��Ʈ
    private int spawnCount;
    //�¸� �ֳʹ� ���� ī��Ʈ
    private int horseSpawnCount;
    //���� �ֳʹ� ���� ī��Ʈ
    private int bossSpawnCount;
    //���� ���� ��� �ð�
    private float roundTime;
    //���� ���� �ִ� �ð�
    private float maxRountTime = 40;
    //���� ���� �ؽ�Ʈ
    [SerializeField] private Text nextRoundText;
    //���� ���� �ؽ�Ʈ
    [SerializeField] private Text currentRoundText;
    //���� ���� ����
    public int CurrentLevel { get; private set; } = 1;
    //���� �¸� ����
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
    /// ���� ���� ����(�ݹ����� ȣ��)
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
    /// ���� ������ �´� �ֳʹ� ����
    /// </summary>
    /// <param name="level">����</param>
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
    /// ���� ������ �´� �¸� �ֳʹ� ����
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
