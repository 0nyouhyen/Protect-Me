using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagercontroller : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonSound;

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1.0f;

        if (audioSource != null && buttonSound != null)
        {
            StartCoroutine(PlaySoundAndChangeScene(sceneName));
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 ButtonSound가 연결되지 않았습니다.");
            SceneManager.LoadScene(sceneName);
        }
    }

    // 휴식 스테이지 이동
    public void GoToRestStage()
    {
        LoadSceneByName("RestStage");
    }

    // 타이틀 씬 이동
    public void GoToTitle()
    {
        LoadSceneByName("Title"); 
    }

    IEnumerator PlaySoundAndChangeScene(string sceneName)
    {
        audioSource.PlayOneShot(buttonSound);

        // 효과음 길이만큼 대기 (Time.timeScale = 0 인 상태에서도 정상 대기)
        yield return new WaitForSecondsRealtime(buttonSound.length);

        // 씬 넘어가기 전 멈춰있던 게임 시간 정상화
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(sceneName);
    }
}