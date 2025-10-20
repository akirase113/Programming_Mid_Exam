using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        GameObject obj = new GameObject("ButtonSound");         // 빈 게임 오브젝트 생성
        AudioSource src = obj.AddComponent<AudioSource>();      // AudioSource 컴포넌트 추가
        src.clip = Resources.Load<AudioClip>("Sounds/SE_button");        // 오디오 클립 로드
        src.Play();

        DontDestroyOnLoad(obj);             // 씬이 바뀌어도 오브젝트 파괴하지 않음
        Destroy(obj, src.clip.length);      // 사운드 재생이 끝나면 오브젝트 파괴
    }
}
