using UnityEngine;
using DG.Tweening;

/// <summary>
/// 이동 경로 및 다음 게임 시작 UI
/// </summary>
public class ArrowMove : MonoBehaviour
{
    public enum arrow { StartArrow, EndArrow, NextRoundButton};
    public arrow arrowType;

    //각 경로 이동 및 버튼 상호작용
    void Update()
    {
        if (!Manager.Instance.IsLive)
            return;

        if (arrowType == arrow.StartArrow)
        {
            if (transform.position.y > 3.5f)
            {
                transform.Translate(0f, -1f * Time.deltaTime, 0f);
            }
            else if (transform.position.y <= 3.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            }
        }
        else if(arrowType == arrow.EndArrow)
        {
            if (transform.position.y < -1.5f)
            {
                transform.Translate(0f, 1f * Time.deltaTime, 0f);
            }
            else if (transform.position.y >= -1.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 1f, 0f);
            }
        }
        else if(arrowType == arrow.NextRoundButton)
        {
            RectTransform rect = GetComponent<RectTransform>();
            if(transform.position.y >= 9.9f)
                rect.DOMoveY(9.5f, 0.5f);
            else if(transform.position.y <= 9.51f)
                rect.DOMoveY(10f, 0.5f);
        }
    }
}
