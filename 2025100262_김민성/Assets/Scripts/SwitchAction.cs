using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock;      // 스위치와 연결된 이동 블록
    public Sprite imageOn;          // 스위치가 켜졌을 때의 이미지
    public Sprite imageOff;         // 스위치가 꺼졌을 때의 이미지
    public bool on = false;         // 스위치 상태 (true: 켜짐, false: 꺼짐)

    private void Start()
    {
        if (on)
        {
            GetComponent<SpriteRenderer>().sprite = imageOn; // 초기 상태가 켜짐이면 켜진 이미지로 설정
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff; // 초기 상태가 꺼짐이면 꺼진 이미지로 설정
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (on)
            {
                on = false;
                GetComponent<SpriteRenderer>().sprite = imageOff; // 스위치 꺼짐 이미지로 변경
                MovingBlock block = targetMoveBlock.GetComponent<MovingBlock>();
                block.Stop();
            }
            else
            {
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn; // 스위치 켜짐 이미지로 변경
                MovingBlock block = targetMoveBlock.GetComponent<MovingBlock>();
                block.Move();
            }
        }
    }
}
