using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; // 포탄이 삭제되기까지의 시간

    private void Start()
    {
        Destroy(gameObject, deleteTime);        // deleteTime 후에 포탄 오브젝트 삭제
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);        // 다른 오브젝트와 충돌 시 포탄 오브젝트 삭제
    }
}
