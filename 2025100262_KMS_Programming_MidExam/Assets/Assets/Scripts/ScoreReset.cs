#if UNITY_EDITOR    // ����Ƽ �����Ϳ����� �����ϵǵ��� ����
using UnityEditor;
using UnityEngine;

public class ScoreReset
{
    [MenuItem("Tools/PlayerPrefs �ʱ�ȭ")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
#endif