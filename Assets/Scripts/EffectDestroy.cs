using UnityEngine;

/// <summary>
/// ���� ����Ʈ �ڵ� ���� Ŭ����
/// </summary>
public class EffectDestroy : MonoBehaviour
{
    //����Ʈ �ڵ� ���� Ÿ�̸�
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
            Destroy(gameObject);
    }
}
