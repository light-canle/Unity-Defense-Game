using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;      // ���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����

    private void Awake()
    {
        // MainCamera �±׸� ������ �ִ� ������Ʈ�� Ž���� �� Camera ������Ʈ�� ������ �����Ѵ�.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // ���콺�� UI�� �ӹ��� ���� ���� �Ʒ� �ڵ尡 ������� �ʵ��� ��
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }
        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            // ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ������ �����Ѵ�
            // ray.origin�� ������ ������ġ(���⼭�� ī�޶� ��ġ), ray.direction�� ������ ��������� ���Ѵ�.
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // (�Ʒ��� �ڵ带 ���� 2D ����Ϳ��� 3D ������ ������Ʈ�� ���콺�� ������ �� �ְ� �Ѵ�.)
            // ������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
            if ( Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform; // ������ ������Ʈ �ӽ� ����

                // ������ �ε��� ������Ʈ�� �±װ� 'Tile'�� ���
                if (hit.transform.CompareTag("Tile"))
                {
                    // Ÿ���� �����ϴ� SpawnTower() ȣ��
                    towerSpawner.SpawnTower(hit.transform);
                }
                // Ÿ���� �����ϸ� �ش� Ÿ�� ������ ����ϴ� Ÿ�� ����â On
                else if ( hit.transform.CompareTag("Tower") )
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if ( Input.GetMouseButtonUp(0))
        {
            // ���콺�� ������ �� ������ ������Ʈ�� ���ų� ������ ������Ʈ�� Ÿ���� �ƴϸ�
            // (���� ��� ������� ������ ���)
            if ( hitTransform == null || hitTransform.CompareTag("Tower") == false )
            {
                // Ÿ�� ���� �г��� ��Ȱ��ȭ �Ѵ�
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}