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
    private Transform hitTransform = null;      // 마우스 픽킹으로 선택한 오브젝트 임시 저장

    private void Awake()
    {
        // MainCamera 태그를 가지고 있는 오브젝트를 탐색한 뒤 Camera 컴포넌트의 정보를 전달한다.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스가 UI에 머물러 있을 때는 아래 코드가 실행되지 않도록 함
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선을 생성한다
            // ray.origin은 광선의 시작위치(여기서는 카메라 위치), ray.direction은 광선의 진행방향을 뜻한다.
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // (아래의 코드를 통해 2D 모니터에서 3D 월드의 오브젝트를 마우스로 선택할 수 있게 한다.)
            // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if ( Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform; // 선택한 오브젝트 임시 저장

                // 광선에 부딪힌 오브젝트의 태그가 'Tile'인 경우
                if (hit.transform.CompareTag("Tile"))
                {
                    // 타워를 생성하는 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
                // 타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 On
                else if ( hit.transform.CompareTag("Tower") )
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if ( Input.GetMouseButtonUp(0))
        {
            // 마우스를 눌렀을 때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
            // (예를 들어 빈공간을 선택한 경우)
            if ( hitTransform == null || hitTransform.CompareTag("Tower") == false )
            {
                // 타워 정보 패널을 비활성화 한다
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}
