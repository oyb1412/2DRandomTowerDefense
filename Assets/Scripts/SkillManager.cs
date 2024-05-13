using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų ����
/// </summary>
public class SkillManager : MonoBehaviour
{
    #region Variable
    public static SkillManager instance;
    //��ų ��� ��ư
    [SerializeField] private Button skillButton;
    //��ų ��Ÿ�� ǥ�� �̹���
    [SerializeField] private Image skillCoolTimeImage;
    //��ų ���� ǥ�� �̹���
    [SerializeField] private GameObject skillRangeObjectPrefab;
    //������ ��ų �̹��� ������Ʈ
    public GameObject SkillRangeObject { get; private set; }
    //��ų ����Ʈ ������Ʈ
    [SerializeField] private GameObject skillPrefab;
    //��ų ��Ÿ��
    [SerializeField] private int skillCoolTime;
    //���� ��ų ��Ÿ��
    private float currentSkillCoolTime;
    //��ų ��밡�� ����
    private bool useSkill;

    #endregion
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentSkillCoolTime = skillCoolTime;
        SkillRangeObject = Instantiate(skillRangeObjectPrefab, transform);
        SkillRangeObject.SetActive(false);
    }

    void Update()
    {
        if(SkillRangeObject.gameObject.activeSelf)
        {
            SkillRangeObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1f));
            Cursor.visible = false;
            //��ų ���
            if(Input.GetMouseButtonDown(0))
            {
                //���� ����Ʈ ������ ����
                AudioManager.instance.PlayerSfx(AudioManager.Sfx.Skill);

                Transform skill = Instantiate(skillPrefab, null).transform;
                skill.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y+200f, 1f));
                Cursor.visible = true;
                skillCoolTimeImage.transform.localScale = Vector2.one;
                currentSkillCoolTime = 0;
                useSkill = true;
                skillButton.interactable = false;
                SkillRangeObject.gameObject.SetActive(false);
            }
            //��ų ���
            if (Input.GetMouseButtonDown(1))
            {
                SkillRangeObject.gameObject.SetActive(false);
                skillButton.interactable = true;
                Cursor.visible = true;
            }
        }

        if (currentSkillCoolTime < skillCoolTime && useSkill)
        {
            currentSkillCoolTime += Time.deltaTime;
            skillCoolTimeImage.transform.localScale -= new Vector3(0f, Time.deltaTime / skillCoolTime, 0f);
            if (skillCoolTimeImage.transform.localScale.y < 0 || currentSkillCoolTime >= skillCoolTime)
            {
                skillCoolTimeImage.transform.localScale = Vector2.zero;
                skillButton.interactable = true;
                currentSkillCoolTime = skillCoolTime;
                useSkill = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
            SkillClick();
    }

    /// <summary>
    /// ��ų ��ư ����
    /// </summary>
    public void SkillClick()
    {
        if(currentSkillCoolTime >= skillCoolTime && !useSkill)
        {
            SkillRangeObject.SetActive(true);

            skillButton.interactable = false;
        }
    }
}
