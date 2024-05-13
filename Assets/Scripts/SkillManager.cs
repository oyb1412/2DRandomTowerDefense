using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스킬 관리
/// </summary>
public class SkillManager : MonoBehaviour
{
    #region Variable
    public static SkillManager instance;
    //스킬 사용 버튼
    [SerializeField] private Button skillButton;
    //스킬 쿨타임 표기 이미지
    [SerializeField] private Image skillCoolTimeImage;
    //스킬 범위 표기 이미지
    [SerializeField] private GameObject skillRangeObjectPrefab;
    //생성된 스킬 이미지 오브젝트
    public GameObject SkillRangeObject { get; private set; }
    //스킬 이펙트 오브젝트
    [SerializeField] private GameObject skillPrefab;
    //스킬 쿨타임
    [SerializeField] private int skillCoolTime;
    //현재 스킬 쿨타임
    private float currentSkillCoolTime;
    //스킬 사용가능 여부
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
            //스킬 사용
            if(Input.GetMouseButtonDown(0))
            {
                //번개 이펙트 프리펩 생성
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
            //스킬 취소
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
    /// 스킬 버튼 선택
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
