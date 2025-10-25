using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("이펙트 프리팹 설정")]
    // 1. Inspector에서 MagicEffect 프리팹을 연결
    public GameObject magicEffectPrefab;

    [Header("이펙트 움직임 설정")]
    // 2. 마법이 생성될 높이
    [Tooltip("마법 이펙트가 적의 발밑보다 얼마나 높은 곳에서 생성될지")]
    public float magicSpawnHeightOffset = 3f;

    // 3. 마법이 떨어지는 속도
    [Tooltip("마법 이펙트가 발밑으로 내려오는 속도")]
    public float magicFallSpeed = 10f;

    // ---------------------------------------------------------------
    // Update() 함수와 AttackCheck() 함수는 
    // EnemyClickable.cs 방식에서는 필요 없으므로 삭제되었습니다.
    // ---------------------------------------------------------------


    /// <summary>
    /// EnemyClickable.cs 스크립트에 의해 'public'으로 호출되는 함수
    /// </summary>
    /// <param name="enemyCollider">클릭된 적의 콜라이더</param>
    public void PerformAttack(Collider2D enemyCollider)
    {
        // 1. 적의 Transform 정보 가져오기
        Transform enemyTransform = enemyCollider.transform;

        // 2. "적의 발밑" 위치 계산
        float footOffsetY = enemyCollider.bounds.size.y / 2f;
        Vector2 targetFootPosition = new Vector2(enemyTransform.position.x, enemyTransform.position.y - footOffsetY);

        // 3. "적의 발밑 상공" (마법 생성 위치) 계산
        Vector2 startSpawnPosition = new Vector2(targetFootPosition.x, targetFootPosition.y + magicSpawnHeightOffset);

        // 4. 마법 이펙트 프리팹 생성
        if (magicEffectPrefab != null)
        {
            // startSpawnPosition 위치에 magicEffectPrefab을 생성
            GameObject instantiatedMagic = Instantiate(magicEffectPrefab, startSpawnPosition, Quaternion.identity);

            // 생성된 이펙트에 붙어있는 MagicEffectMover 스크립트를 찾음
            MagicEffectMover mover = instantiatedMagic.GetComponent<MagicEffectMover>();
            if (mover != null)
            {
                // 이펙트가 떨어질 목표 위치와 속도를 설정해 줌
                mover.SetTarget(targetFootPosition, magicFallSpeed);
            }
            else
            {
                Debug.LogError("MagicEffectMover 스크립트가 MagicEffectPrefab에 없습니다!");
            }
        }
        else
        {
            Debug.LogError("Magic Effect Prefab이 할당되지 않았습니다!");
        }

        // 5. 적에게 데미지 주기 (Enemy.cs 스크립트 찾기)
        Enemy enemyScript = enemyCollider.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // Enemy.cs의 TakeDamage() 함수 호출
            enemyScript.TakeDamage();
        }
    }
}