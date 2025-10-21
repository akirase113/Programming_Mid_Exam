using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1; // 적의 체력 (1로 설정하면 한 방에 죽음)

    // 외부(PlayerAttack 스크립트)에서 호출될 함수
    public void TakeDamage()
    {
        health--; // 체력 1 감소

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("적이 처치되었습니다!");
        
        // (선택) 여기에 사망 애니메이션 재생 코드를 넣습니다.
        // animator.SetTrigger("Die");

        // 콜라이더를 비활성화해서 다시 클릭되지 않게 함
        GetComponent<Collider2D>().enabled = false;
        
        // (선택) 사망 애니메이션 시간을 기다린 후 파괴
        // Destroy(gameObject, 1.0f); // 1초 뒤 파괴
        
        // 즉시 파괴
        Destroy(gameObject);
    }
}