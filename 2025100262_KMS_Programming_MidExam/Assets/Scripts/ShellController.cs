using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; // ��ź�� �����Ǳ������ �ð�

    private void Start()
    {
        Destroy(gameObject, deleteTime);        // deleteTime �Ŀ� ��ź ������Ʈ ����
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);        // �ٸ� ������Ʈ�� �浹 �� ��ź ������Ʈ ����
    }
}
