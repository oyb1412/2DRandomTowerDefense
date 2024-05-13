using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 이동 및 게임 종료 
/// </summary>
public class SceneControl : MonoBehaviour
{
    /// <summary>
    /// 씬 이동(콜백으로 호출)
    /// </summary>
    /// <param name="index">이동할 씬</param>
    public void GoScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// 게임 종료(콜백으로 호출)
    /// </summary>
    public void GameExit()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
