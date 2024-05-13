using UnityEngine;
using DG.Tweening;

/// <summary>
/// �̵� ��� �� ���� ���� ���� UI
/// </summary>
public class ArrowMove : MonoBehaviour
{
    public enum arrow { StartArrow, EndArrow, NextRoundButton};
    public arrow arrowType;

    //�� ��� �̵� �� ��ư ��ȣ�ۿ�
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
