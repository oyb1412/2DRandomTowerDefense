using UnityEngine;

/// <summary>
/// 골드 프리펩 이동 및 삭제
/// </summary>
public class GoldMovement : MonoBehaviour
{
    //자동 삭제 타이머
    private float timer;

    void Update()
    {
        transform.Translate(new Vector3(0f, 1.5f, 0f) * Time.deltaTime);
        timer += Time.deltaTime;
        if(timer > 0.5f)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
