using UnityEngine;

/// <summary>
/// ���� ���� BGM 
/// </summary>
public class StartManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayerBgm(true);
    }
}
