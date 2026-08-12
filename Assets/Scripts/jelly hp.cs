using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyHealth : MonoBehaviour
{
    [Header("젤리 체력 설정")]
    public float maxHp = 100f;
    public float currentHp;
    public Slider hpSlider;

    [Header("게임 오버 연동")]
    public GameObject gameOverCanvas;

    [Header("사운드 에셋")]
    public AudioClip hitSound;       // 피격 효과음
    [Range(0f, 1f)]
    public float soundVolume = 1.0f; // 소리 크기 조절 (0~1)

    private bool isDead = false;

    void Start()
    {
        currentHp = maxHp;
        UpdateHPUI();

        // 게임 시작 시 게임 오버 캔버스 안 뜨게
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // 이미 죽은 상태면 대미지 무시

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0f);

        // 피격 효과음 재생
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, soundVolume);
        }

        // 대미지 잘 들어가는지 확인용
        Debug.Log($"[JellyHealth] 데미지 {damage} 받음! 남은 HP: {currentHp}/{maxHp}");

        UpdateHPUI();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[JellyHealth] 젤리가 사망했습니다. 게임 오버!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void UpdateHPUI()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp / maxHp;
        }
        else
        {
            Debug.LogWarning("[JellyHealth] Inspector에 hpSlider가 연결되지 않았습니다!");
        }
    }
}