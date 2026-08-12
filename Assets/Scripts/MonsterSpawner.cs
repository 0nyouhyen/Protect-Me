using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰할 몬스터 프리팹들")]
    public GameObject[] monsterPrefabs;

    [Header("스폰 위치 좌표 (X축 - 좌/우)")]
    public float leftSpawnX = -10.0f; 
    public float rightSpawnX = 10.0f;

    [Header("스폰 위치 좌표 범위 (Y축 - 위/아래)")]
    public float minY = -3.0f;       
    public float maxY = 1.0f;         

    [Header("스폰 딜레이 (최소/최대)")]
    public float minSpawnDelay = 2.0f;
    public float maxSpawnDelay = 5.0f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefabs.Length == 0) return;

        // 등록된 몬스터 중 랜덤 선택
        int randomIndex = Random.Range(0, monsterPrefabs.Length);
        GameObject selectedPrefab = monsterPrefabs[randomIndex];

        // 왼쪽(0) 또는 오른쪽(1) 랜덤 결정
        bool spawnOnLeft = Random.Range(0, 2) == 0;

        // X좌표 및 이동 방향 설정
        float spawnX = spawnOnLeft ? leftSpawnX : rightSpawnX;
        int moveDir = spawnOnLeft ? 1 : -1;

        // y 좌표 랜덤 선택
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        // 생성
        GameObject newMonster = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
    }
}