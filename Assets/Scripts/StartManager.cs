using UnityEngine;

/// <summary>
/// 게임 시작 BGM 
/// </summary>
public class StartManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayerBgm(true);
    }
}
