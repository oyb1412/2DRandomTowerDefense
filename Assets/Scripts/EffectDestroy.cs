using UnityEngine;

/// <summary>
/// 각종 이펙트 자동 삭제 클래스
/// </summary>
public class EffectDestroy : MonoBehaviour
{
    //이펙트 자동 삭제 타이머
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
            Destroy(gameObject);
    }
}
