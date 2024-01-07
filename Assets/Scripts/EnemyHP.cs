using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;        // 최대 체력
    private float currentHP;    // 현재 체력
    private bool isDie = false; // 사망 상태 여부
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;      // 현재 체력과 최대 체력을 같게 설정
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // 이미 적의 체력이 0 이하로 내려가 사망 상태가 되었을 때 또 피해를 입으면
        // enemy.OnDie()가 여러 번 실행될 수 있으므로 이를 막는다.
        if (isDie == true) return;

        // 현재 체력을 damage만큼 감소
        currentHP -= damage;
        // 기존 코루틴을 멈추고 다시 실행
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 체력이 0 이하인 경우 사망 상태로 바꿈
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie(EnemyDestoryType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 적의 현재 색을 color 변수에 저장
        Color color = spriteRenderer.color;
        // 적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;
        // 0.05초 동안 대기
        yield return new WaitForSeconds(0.05f);
        // 적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }

}
