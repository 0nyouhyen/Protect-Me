using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestStageManager : MonoBehaviour
{
    [Header("UI 연동")]
    public TMP_Text goldText; // 휴식 스테이지의 골드 표시 텍스트 UI

    void Start()
    {

        // GameManager가 존재하는지 확인 후, 싱글톤에 남아있는 gold 값을 가져와서 표시
        if (GameManager.Instance != null && goldText != null)
        {
            goldText.text = GameManager.Instance.gold.ToString();
            Debug.Log($"[RestStage] 현재 골드 연동 완료: {GameManager.Instance.gold}G");
        }
    }
}
