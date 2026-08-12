using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 💡 파일명이 SceneCon.cs이므로 클래스명도 SceneCon으로 일치시켜 줍니다!
public class SceneChange : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonSound;

    public void GoToMainGame()
    {

        if (audioSource != null && buttonSound != null)
        {
            StartCoroutine(PlaySoundAndChangeScene());
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator PlaySoundAndChangeScene()
    {
        audioSource.PlayOneShot(buttonSound);

        // 💡 정지 상태 대비 WaitForSecondsRealtime 사용
        yield return new WaitForSecondsRealtime(0.3f);

        SceneManager.LoadScene("Main");
    }
}