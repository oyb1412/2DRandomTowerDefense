using UnityEngine;

/// <summary>
/// Ÿ�ϸ� �� �� ����
/// </summary>
public class CellManager : MonoBehaviour
{
    //���� �� ��� ����
    public bool UseSell { get; set; }
    //���� �� ��������Ʈ ������
    public SpriteRenderer Spriter { get; set; }
    private void Awake()
    {
        Spriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Spriter.color = Color.white;
    }
}
