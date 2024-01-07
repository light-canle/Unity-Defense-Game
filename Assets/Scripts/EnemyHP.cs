using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;        // �ִ� ü��
    private float currentHP;    // ���� ü��
    private bool isDie = false; // ��� ���� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;      // ���� ü�°� �ִ� ü���� ���� ����
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // �̹� ���� ü���� 0 ���Ϸ� ������ ��� ���°� �Ǿ��� �� �� ���ظ� ������
        // enemy.OnDie()�� ���� �� ����� �� �����Ƿ� �̸� ���´�.
        if (isDie == true) return;

        // ���� ü���� damage��ŭ ����
        currentHP -= damage;
        // ���� �ڷ�ƾ�� ���߰� �ٽ� ����
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0 ������ ��� ��� ���·� �ٲ�
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie(EnemyDestoryType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ���� color ������ ����
        Color color = spriteRenderer.color;
        // ���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;
        // 0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);
        // ���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }

}
