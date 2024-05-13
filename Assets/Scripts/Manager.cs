using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ���� �Ŵ���
/// </summary>
public class Manager : MonoBehaviour
{
    #region Variable
    public static Manager Instance;

    //�� ��ų ���׷��̵� UI �ǳ�
    [SerializeField] private GameObject[] upgradeStateInfo;
    //��ų ���׷��̵� �з��� UI �ǳ�
    public GameObject stateUpgradeObj;
    //���ӿ��� UI
    [SerializeField] private GameObject gameOverUI;
    //���ӽ¸� UI
    [SerializeField] private GameObject gameClearUI;
    //���� ��� ǥ�� �ؽ�Ʈ
    [SerializeField] private Text goldText;
    //���� ���
    [SerializeField] private int currentGold;
    //���� �����
    [SerializeField] private int currentHearts;
    //���� ����� ǥ�� UI
    [SerializeField] private GameObject[] hearts;
    //���� ������ ���� ���� ǥ�� �ؽ�Ʈ
    [SerializeField] private GameObject gameOverText;
    //���� ���� ����
    public bool IsLive { get; set; } = true;
    //���� ���� ����
    private bool isOver;
    //���׷��̵� ���� �� ����
    public bool IsUpgradeState { get; set; }
    //����Ÿ�� �Ǽ� ���� ����
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
    /// ���� ���׷��̵� UI ���
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
    /// ������Ʈ1 ���׷��̵�(�ݹ����� ȣ��)
    /// </summary>
    public void StateUpgrade1()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade1 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// ������Ʈ2 ���׷��̵�(�ݹ����� ȣ��)
    /// </summary>
    public void StateUpgrade2()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade2 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// ������Ʈ3 ���׷��̵�(�ݹ����� ȣ��)
    /// </summary>
    public void StateUpgrade3()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade3 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// ������Ʈ4 ���׷��̵�(�ݹ����� ȣ��)
    /// </summary>
    public void StateUpgrade4()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade4 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// ������Ʈ5 ���׷��̵�(�ݹ����� ȣ��)
    /// </summary>
    public void StateUpgrade5()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
        TowerManager.StateUpgrade5 = true;
        stateUpgradeObj.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => stateUpgradeObj.transform.DOScaleX(0.0f, 0.3f));
        IsLive = true;
    }

    /// <summary>
    /// ������Ʈ6 ���׷��̵�(�ݹ����� ȣ��)
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
    /// ��� ����
    /// </summary>
    /// <param name="gold">������ ���</param>
    public void SetGold(int gold)
    {
        currentGold += gold;
    }

    /// <summary>
    /// ��� ��ȯ
    /// </summary>
    /// <returns>���� ���</returns>
    public int GetGold()
    {
        return currentGold;
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    /// <param name="heart">������ �����</param>
    public void SetHeart(int heart)
    {
        currentHearts += heart;
    }

    /// <summary>
    /// ���� �����(�ݹ����� ȣ��)
    /// </summary>
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ���� Pause ���� �� ����(�ݹ����� ȣ��)
    /// </summary>
    public void StopButtonEnter()
    {
        if(IsLive)
            IsLive = false;
        else
            IsLive = true;
    }
}
