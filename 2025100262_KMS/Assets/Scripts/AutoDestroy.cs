using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // 이펙트 애니메이션의 총 길이 (초 단위)
    public float animationDuration = 1f;

    void Start()
    {
        // animationDuration초 뒤에 이 오브젝트(자기 자신)를 파괴
        Destroy(gameObject, animationDuration);
    }
}