using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �� �̵� �� ���� ���� 
/// </summary>
public class SceneControl : MonoBehaviour
{
    /// <summary>
    /// �� �̵�(�ݹ����� ȣ��)
    /// </summary>
    /// <param name="index">�̵��� ��</param>
    public void GoScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// ���� ����(�ݹ����� ȣ��)
    /// </summary>
    public void GameExit()
    {
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
