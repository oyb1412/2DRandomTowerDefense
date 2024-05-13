using UnityEngine;

/// <summary>
/// 오디오 매니저
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Variable
    public enum Sfx {
        Attack, Create, Upgrade, Die, Lose, Select, Magic, Skill
    }

    [Header("--Instance--")]
    public static AudioManager instance;

    [Header("--Bgm--")]
    [SerializeField]private AudioClip bgmClip;
    [SerializeField] private float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("--Sfx--")]
    private int sfxChannels = 10;
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private float sfxVolume;
    private AudioSource[] sfxPlayers;


    #endregion

    #region InitMethod
    private void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        InitBgm();
        InitSfx();
    }

    void InitBgm()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
    }

    void InitSfx()
    {
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].bypassListenerEffects = true;
        }
    }
    #endregion


    public void PlayerBgm(bool islive)
    {
        bgmPlayer.clip = bgmClip;
        if (islive)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void PlayerSfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            if (sfxPlayers[i].isPlaying)
                continue;

            sfxPlayers[i].clip = sfxClips[(int)sfx];
            sfxPlayers[i].Play();
            break;
        }
    }
}
