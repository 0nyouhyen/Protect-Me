using System;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [Header("몬스터 능력치")]
    public float hp = 25f;
    public float damage = 10f;
    public float moveSpeed = 2.0f;

    [Header("사운드 에셋")]
    public AudioClip deathSound; // 🔊 몬스터 사망 효과음 에셋 드래그 앤 드롭
    [Range(0f, 1f)]
    public float soundVolume = 1.0f; // 소리 크기 조절 (0~1)

    private Transform targetPlayer;
    private Animator animator;
    private Vector3 originalScale;

    private bool isDead = false;

    void Start()
    {
        originalScale = transform.localScale;
        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetBool("Walk", true);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) targetPlayer = playerObj.transform;
    }

    void Update()
    {
        if (isDead) return;

        if (targetPlayer != null && targetPlayer.gameObject != null)
        {
            Vector3 dir = (targetPlayer.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            if (dir.x != 0)
            {
                Vector3 scale = originalScale;
                scale.x = Mathf.Abs(originalScale.x) * (dir.x > 0 ? -1 : 1);
                transform.localScale = scale;
            }
        }
    }

    private void OnMouseDown()
    {
        if (isDead) return;

        float damageAmount = (GameManager.Instance != null)
            ? GameManager.Instance.playerClickDamage
            : 15f;

        TakeDamage(damageAmount);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        hp -= damageAmount;

        if (hp <= 0)
        {
            isDead = true; // 💡 클릭으로 잡았을 때만 isDead = true
            Die();
        }
    }

    private void Die()
    {
        // 🔊 몬스터 사망 효과음 재생
        // PlayClipAtPoint는 오브젝트가 Destroy되어도 소리가 끝까지 정상 재생됩니다!
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, soundVolume);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Player"))
        {
            JellyHealth jelly = collision.GetComponent<JellyHealth>();
            if (jelly != null) jelly.TakeDamage(damage);

            // 💡 플레이어와 부딪혀서 자폭할 때는 소리를 내지 않고 삭제
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 💡 마우스 클릭으로 체력을 다 깎아서 죽은 몬스터만 카운트 차감
        if (isDead && GameManager.Instance != null)
        {
            GameManager.Instance.AddKillCount();
        }
    }
}