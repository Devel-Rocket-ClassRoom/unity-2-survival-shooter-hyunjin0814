using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 어디서든 접근 가능하게

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioMixer masterMixer;

    void Awake()
    {
        // 씬이 바뀌어도 파괴되지 않게 설정 (선택 사항)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 소리 재생 함수들을 여기에 모음
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}