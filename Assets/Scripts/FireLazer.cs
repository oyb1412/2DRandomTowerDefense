using UnityEngine;

/// <summary>
/// 스킬 레이저 이펙트 
/// </summary>
public class FireLazer : MonoBehaviour
{
    //자동 삭제 타이머
    private float timer;
    //이펙트 프리펩
    [SerializeField]private GameObject LazerPrefab;
    void Start()
    {
        var col = Instantiate(LazerPrefab, transform).transform;
        col.position = new Vector3(transform.position.x, transform.position.y-1.9f, 1f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
            Destroy(gameObject);
    }
}
