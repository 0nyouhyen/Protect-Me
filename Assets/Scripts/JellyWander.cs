using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyWander : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 1.5f;
    public float wanderRadius = 3.0f;

    // 가두고 싶은 영역 범위
    [Header("이동 제한 구역 (벽 내부 좌표)")]
    public float minX = -4.0f;
    public float maxX = 4.0f;
    public float minY = -2.0f;
    public float maxY = 1.0f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
                if (animator != null) animator.Play("Idle");
            }
        }
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (animator != null) animator.Play("Idle");

            float idleTime = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(idleTime);

            // 랜덤 목표 지점 생성
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            Vector2 rawTarget = startPosition + randomCircle;

            // 벽 밖으로 나가지 못하게 막기
            targetPosition = new Vector2(
                Mathf.Clamp(rawTarget.x, minX, maxX),
                Mathf.Clamp(rawTarget.y, minY, maxY)
            );

            // 좌우 반전
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = (targetPosition.x < transform.position.x);
            }

            // Walk idle 재생 및 이동
            if (animator != null) animator.Play("Walk");

            isMoving = true;

            while (isMoving)
            {
                yield return null;
            }
        }
    }
}
