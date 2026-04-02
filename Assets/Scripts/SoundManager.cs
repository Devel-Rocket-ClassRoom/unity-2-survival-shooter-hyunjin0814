using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioMixer masterMixer;

    void Awake()
    {
        // 싱글톤 
        if (instance == null)
        {
            instance = this;
            // (씬 변경되도 오브젝트 파괴 X) 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 중복 생성 방지, 항상 객체 1개만 유지
            Destroy(gameObject);
        }
    }

    // 효과음(SFX)을 1회 재생
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}