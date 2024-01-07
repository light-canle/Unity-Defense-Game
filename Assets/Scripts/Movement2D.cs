using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;

    // moveSpeed 변수의 프로퍼티
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value); // 이동 속도가 음수가 되지 않도록 설정!
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
