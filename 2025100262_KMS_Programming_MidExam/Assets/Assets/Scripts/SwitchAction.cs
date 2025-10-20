using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock;      // ����ġ�� ����� �̵� ���
    public Sprite imageOn;          // ����ġ�� ������ ���� �̹���
    public Sprite imageOff;         // ����ġ�� ������ ���� �̹���
    public bool on = false;         // ����ġ ���� (true: ����, false: ����)

    private void Start()
    {
        if (on)
        {
            GetComponent<SpriteRenderer>().sprite = imageOn; // �ʱ� ���°� �����̸� ���� �̹����� ����
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff; // �ʱ� ���°� �����̸� ���� �̹����� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (on)
            {
                on = false;
                GetComponent<SpriteRenderer>().sprite = imageOff; // ����ġ ���� �̹����� ����
                MovingBlock block = targetMoveBlock.GetComponent<MovingBlock>();
                block.Stop();
            }
            else
            {
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn; // ����ġ ���� �̹����� ����
                MovingBlock block = targetMoveBlock.GetComponent<MovingBlock>();
                block.Move();
            }
        }
    }
}
