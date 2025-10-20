using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody = null;   // Rigidbody2D ����
    private float axisH = 0.0f;         // ���� �Է� ��

    private void Start()
    {
        // ��ũ��Ʈ�� �ִ� ���� ������Ʈ�� Rigidbody2D ������Ʈ�� ������ rbody ������ �Ҵ�
        rbody = GetComponent<Rigidbody2D>();
    }

    private float speed = 3.0f;          // �̵� �ӵ�

    private void Update()
    {
        axisH = Input.GetAxis("Horizontal");    // ���� �Է� ���� axisH ������ ����

        Debug.Log("axisH: " + axisH);  // ���� �Է� �� ���
        // ���� ����
        if (axisH > 0.0f)   // ������ �̵�
        {
            Debug.Log("������ �̵�");
            transform.localScale = new Vector2(1.0f, 1.0f);
            //GetComponent<SpriteRenderer>().flipX = false; // �¿� ���� ����
        }
        else if (axisH < 0.0f)  // ���� �̵�
        {
            Debug.Log("���� �̵�");
            transform.localScale = new Vector2(-1.0f, 1.0f);        // �¿� ����
            //GetComponent<SpriteRenderer>().flipX = true;  // �¿� ���� ����
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody2D�� �ӵ��� �����Ͽ� �÷��̾� �̵�
        //rbody.velocity = new Vector2(axisH * 3.0f, rbody.velocity.y);
        rbody.linearVelocity = new Vector2(axisH * 3.0f, rbody.linearVelocity.y);
    }

}