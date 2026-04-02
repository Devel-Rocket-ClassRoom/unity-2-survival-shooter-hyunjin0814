using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI scoreText;

    public AudioMixer masterMixer;      // 오디오 믹서
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;
    public GameObject settingsUI;       // 세팅 UI

    public Image damageFlashImage;                          // 깜빡일 이미지
    public float flashSpeed = 5f;                           // 깜빡이는 속도
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f);  // 데미지 입을 때 나올 색상

    public PlayerInput playerInput;
    private int score;

    public GameObject gameOverPanel; 
    public float restartDelay = 3f;  

    private void Awake()
    {
        score = 0;
    }

    void Start()
    {
        bgmSlider.value = 1f;
        sfxSlider.value = 1f;

        // 리스너 연결
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        muteToggle.onValueChanged.AddListener(SetMute);

        muteToggle.isOn = false;
        settingsUI.SetActive(false);

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // 1. ESC 키 입력 감지 (GetKeyDown 추천)
        if (playerInput.Escape)
        {
            OpenSettings(); // 꺼져 있다면 켜기
        }

        if (damageFlashImage != null && damageFlashImage.color.a > 0)   // damageFlashImage.color.a: 투명도(alpha)값
        {
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);  // 부드럽게 색을 변경
        }
    }

    public void SetHealthSlide(float amount)
    {
        healthSlider.value = amount;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"SCORE: {score}";
    }

    public void OpenSettings()
    {
        settingsUI.SetActive(true);
        Time.timeScale = 0f;        // 게임 시간 정지
    }

    public void SetBGMVolume(float volume)
    {
        if (volume <= 0.0001f)
        {
            // 믹서에 -80dB(완전 무음)를 강제로 입력
            masterMixer.SetFloat("BGM_Vol", -80f);
        }
        else
        {
            // 0보다 클 때만 소리 조절
            masterMixer.SetFloat("BGM_Vol", Mathf.Log10(volume) * 20);
        }
    }

    // 효과음 조절
    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.0001f)
        {
            // 믹서에 -80dB(완전 무음)를 강제로 입력
            masterMixer.SetFloat("SFX_Vol", -80f);
        }
        else
        {
            // 0보다 클 때만 소리 조절
            masterMixer.SetFloat("SFX_Vol", Mathf.Log10(volume) * 20);
        }
    }

    public void SetMute(bool isMuted)
    {
        // 전체 소리를 끄려면 Master 볼륨을 최하로 낮춤
        float volume = isMuted ? -80f : Mathf.Log10(bgmSlider.value) * 20;
        masterMixer.SetFloat("Master", volume);
    }

    public void PlayDamageFlash()
    {
        if (damageFlashImage != null)
        {
            damageFlashImage.color = flashColor;
        }
    }

    public void CloseSettings() { settingsUI.SetActive(false); Time.timeScale = 1f; }
    public void QuitGame() { Application.Quit(); }

    public void OnPlayerDead()
    {
        gameOverPanel.SetActive(true);

        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        yield return new WaitForSecondsRealtime(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;
    }
}