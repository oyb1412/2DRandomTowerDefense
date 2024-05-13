using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ���콺 ��ȣ�ۿ�
/// </summary>
public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// ���콺 ��ġ�� ������Ʈ�� ���콺�� �浹 ����
    /// </summary>
    /// <param name="tag">������ ������Ʈ�� tag</param>
    /// <returns></returns>
    public Collider2D MouseRayCast(string tag)
    {
        int mask = -1 << LayerMask.NameToLayer("Text");
        mask = ~mask;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,0f, mask);
        if (hit.collider != null && hit.collider.CompareTag(tag))
        {
            return hit.collider;
        }
        else
            return null;
    }

    /// <summary>
    /// ���� ����� �� ����
    /// </summary>
    /// <param name="targets">������ ����</param>
    public Transform GetNearTarget(RaycastHit2D[] targets)
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;

            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;

                result = target.transform;
            }
        }
        return result;
    }

    /// <summary>
    /// ���� ������ ���� ����
    /// </summary>
    /// <param name="targets">���� ��Ÿ��� ���� ��</param>
    public Transform GetEarliestTarget(RaycastHit2D[] targets)
    {
        EnemyMovement[] enemys = new EnemyMovement[targets.Length];
        GenericManager<EnemyMovement> generic = new GenericManager<EnemyMovement>();

        for (int i = 0; i < targets.Length; i++)
        {
            for (int q = 0; q < targets.Length - 1; q++)
            {
                enemys[q] = targets[q].transform.GetComponent<EnemyMovement>();
                enemys[q + 1] = targets[q + 1].transform.GetComponent<EnemyMovement>();
                if (enemys[q].CurrentWayPoint < enemys[q + 1].CurrentWayPoint)
                {
                    generic.Swap(ref enemys[q], ref enemys[q + 1]);
                }
            }
        }
        return enemys[0].transform;
    }
}
