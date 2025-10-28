using UnityEngine;

public class MagicEffectMover : MonoBehaviour
{
    private Vector2 targetPosition; // 마법이 도달할 최종 위치 (적의 발밑)
    public float fallSpeed;        // 마법이 떨어지는 속도
    private bool isMoving = false;  // 현재 움직이는 중인지?

    // PlayerAttack.cs에서 이 함수를 호출하여 목표 위치와 속도를 설정합니다.
    public void SetTarget(Vector2 target, float speed)
    {
        targetPosition = target;
        fallSpeed = speed;
        isMoving = true; // 움직임 시작
    }

    void Update()
    {
        if (isMoving)
        {
            // 목표 위치로 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

            // 목표 위치에 도달했는지 확인
            if (Vector2.Distance(transform.position, targetPosition) < 0.05f) // 거의 도착했다면
            {
                isMoving = false; // 움직임 멈춤
                // (선택) 여기서 추가적인 이펙트(예: 땅에 꽂혔을 때 작은 폭발)를 발생시킬 수 있습니다.
                // 또는 AutoDestroy 스크립트에 이펙트 재생이 끝나고 파괴되도록 맡깁니다.
                
                // 만약 AutoDestroy 스크립트가 있다면, 이펙트 애니메이션이 다 재생되면 알아서 사라질 겁니다.
            }
        }
    }
}