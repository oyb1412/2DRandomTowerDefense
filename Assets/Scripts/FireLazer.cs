using UnityEngine;

/// <summary>
/// ��ų ������ ����Ʈ 
/// </summary>
public class FireLazer : MonoBehaviour
{
    //�ڵ� ���� Ÿ�̸�
    private float timer;
    //����Ʈ ������
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
