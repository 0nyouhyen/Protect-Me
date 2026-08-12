using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("게임 수치")]
    public float playerClickDamage = 15f;
    public int gold = 0;
    public int targetKillCount = 15; // 목표 몬스터 수 (시작 숫자)

    [HideInInspector]
    public int remainingCount;       // 남은 몬스터 수

    [Header("UI 연동")]
    public TMP_Text killCountText;   // 남은 몬스터 수 표시 텍스트 UI
    public TMP_Text goldText;
    public GameObject gameOverPanel;
    public GameObject stageClearPanel;

    [Header("사운드 에셋")]
    public AudioClip stageClearSound; // :loud_sound: 스테이지 클리어 효과음 에셋
    public AudioClip gameOverSound;
    [Range(0f, 1f)]
    public float soundVolume = 1.0f;  // 소리 크기 조절 (0~1)

    private bool isStaegeCleared = false;

    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 GameManager 바로 삭제
            return;
        }
    }

    void Start()
    {
        // :bulb: 게임 시작 시 남은 몬스터 수를 목표치(15)로 초기화
        remainingCount = targetKillCount;

        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (stageClearPanel != null) stageClearPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // :bulb: 몬스터 처치 시 호출 (카운트 차감)
    public void AddKillCount()
    {
        remainingCount -= 1; // 1씩 깎임
        gold += 10;

        // 0 이하로 내려가지 않게 방지
        if (remainingCount < 0) remainingCount = 0;

        UpdateUI();

        // :bulb: 남은 몬스터 수가 0 이하가 되면 스테이지 클리어
        if (remainingCount <= 0 && !isStaegeCleared)
        {
            StageClear();
        }
    }

    public void UpdateUI()
    {
        if (killCountText != null)
            killCountText.text = remainingCount.ToString(); // :bulb: 남은 숫자 표시

        if (goldText != null)
            goldText.text = gold.ToString();
    }

    private void StageClear()
    {
        isStaegeCleared = true;

        // :loud_sound: 스테이지 클리어 효과음 재생 (Time.timeScale = 0f 상태에서도 멈추지 않고 끝까지 재생)
        if (stageClearSound != null)
        {
            Vector3 spawnPos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(stageClearSound, spawnPos, soundVolume);
        }

        if (stageClearPanel != null)
        {
            stageClearPanel.SetActive(true); // Stage Clear 패널 켜기
        }

        Time.timeScale = 0f; // 게임 일시정지
    }

    public void GameOver()
    {
        // :loud_sound: 게임 오버 효과음 재생 (Time.timeScale = 0f 상태에서도 정상 재생)
        if (gameOverSound != null)
        {
            Vector3 spawnPos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(gameOverSound, spawnPos, soundVolume);
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}