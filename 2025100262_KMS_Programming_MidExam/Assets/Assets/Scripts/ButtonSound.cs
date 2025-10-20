using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        GameObject obj = new GameObject("ButtonSound");         // �� ���� ������Ʈ ����
        AudioSource src = obj.AddComponent<AudioSource>();      // AudioSource ������Ʈ �߰�
        src.clip = Resources.Load<AudioClip>("Sounds/SE_button");        // ����� Ŭ�� �ε�
        src.Play();

        DontDestroyOnLoad(obj);             // ���� �ٲ� ������Ʈ �ı����� ����
        Destroy(obj, src.clip.length);      // ���� ����� ������ ������Ʈ �ı�
    }
}
