using UnityEngine;

/// <summary>
/// 타일맵 각 셀 관리
/// </summary>
public class CellManager : MonoBehaviour
{
    //현재 셀 사용 여부
    public bool UseSell { get; set; }
    //현재 셀 스프라이트 렌더러
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
