using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 능력치")]
    public float hp = 25f;
    public float damage = 10f;
    public float moveSpeed = 1.5f;

    [Header("통통 튀는 연출 설정")]
    public float bounceSpeed = 8f;
    public float bounceHeight = 0.2f;

    private Transform targetPlayer; // 젤리의 위치
    private Vector3 moveDirection;  // 젤리를 향한 이동 방향
    private SpriteRenderer spriteRenderer;
    private float startY;
    private float bounceTimer;

    void Start()
    {
        startY = transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 플레이어 태그 가진 젤리한테 움직임
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            targetPlayer = playerObj.transform;

            // 젤리 방면 방향 벡터 계산
            moveDirection = (targetPlayer.position - transform.position).normalized;

            // 젤리가 왼쪽에 있으면 스프라이트 뒤집기
            if (spriteRenderer != null && moveDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    void Update()
    {
        // 젤리를 향해 이동 z축 이동 방지를 위해 vector2로 방지
        if (targetPlayer != null)
        {
            // 실시간으로 젤리 위치를 추적하고 싶다면 아래 주석을 해제하세요. (ai 도움)
            // moveDirection = (targetPlayer.position - transform.position).normalized;

            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
        }

        // 통통 튀는 연출
        bounceTimer += Time.deltaTime * bounceSpeed;
        float newY = startY + Mathf.Abs(Mathf.Sin(bounceTimer)) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnMouseDown()
    {
        TakeDamage(GameManager.Instance != null ? GameManager.Instance.playerClickDamage : 15f);
    }

    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0) Die();
    }

    private bool isDead = false; // 중복 실행 방지용 변수

    public void Die()
    {
        // 죽은 상태에서 중복 실행 방지
        if (isDead) return;
        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKillCount(); // 킬+1, 골드+10 함께 처리
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JellyHealth jelly = collision.GetComponent<JellyHealth>();
            if (jelly != null)
            {
                jelly.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}