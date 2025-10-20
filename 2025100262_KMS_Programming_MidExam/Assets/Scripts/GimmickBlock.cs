using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;         // �ڵ� ���� Ž�� �Ÿ�
    public bool isDelete = false;       // ���� �� �������� ����

    bool isFell = false;                // ���� �÷���
    float fadeTime = 0.5f;               // ���̵� �ƿ� �ð�

    private void Start()
    {
        // Rigidbody2D ���� �ùķ��̼� ����
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     // �÷��̾� ã��

        if (player != null)
        {
            // �÷��̾���� �Ÿ� ��� (������Ʈ�� �÷��̾��� ��ġ ������ ������ ���� ������)
            float d = Vector2.Distance(transform.position, player.transform.position);

            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)   // ���� ���� �ùķ��̼��� ���� ������ ���                
                {
                    rbody.bodyType = RigidbodyType2D.Dynamic;   // Rigidbody2D ���� �ùķ��̼� ����
                }
            }
        }

        if (isFell) // ���� �� ���� �÷��װ� ������ ���
        {
            fadeTime -= Time.deltaTime;     // �ð��� ������ ����
            Color col = GetComponent<SpriteRenderer>().color;   // ��������Ʈ �������� ���� ��������
            col.a = fadeTime;               // ���� ���� ���̵� Ÿ������ ����
            GetComponent<SpriteRenderer>().color = col; // ���� ������Ʈ
            if (fadeTime <= 0.0f)   // ���̵� Ÿ���� 0 ���ϰ� �Ǹ�
            {
                Destroy(gameObject);    // ������Ʈ ����
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            isFell = true;  // ���� �÷��� ����            
        }
    }
}
