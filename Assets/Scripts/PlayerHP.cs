using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;  // ��üȭ���� ���� ������ �̹���
    [SerializeField]
    private float maxHP = 20;   // �ִ� ü��
    private float currentHP;    // ���� ü��

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;      // ���� ü���� �ִ� ü�°� ���� ����
    }

    public void TakeDamage(float damage)
    {
        // ���� ü���� damage��ŭ ���ҽ�Ŵ
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0�� �� ��� - ���ӿ���
        if (currentHP <= 0)
        {

        }
    }
    
    private IEnumerator HitAlphaAnimation()
    {
        // ��üȭ�� ũ��� ��ġ�� imageScreen ������ color ������ ����
        Color color = imageScreen.color;
        // imageScreen�� ������ 40%�� ����
        color.a = 0.4f;
        imageScreen.color = color;

        // ������ 0%�� �ɶ����� ����
        while ( color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
