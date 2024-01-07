using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;

    // moveSpeed ������ ������Ƽ
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value); // �̵� �ӵ��� ������ ���� �ʵ��� ����!
        get => moveSpeed;
    }

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
