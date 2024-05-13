using UnityEngine;

/// <summary>
/// ��� ������ �̵� �� ����
/// </summary>
public class GoldMovement : MonoBehaviour
{
    //�ڵ� ���� Ÿ�̸�
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
