using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;          // 타워 정보 (공격력, 공격 속도 등)
    [SerializeField]    
    private EnemySpawner enemySpawner;              // 현재 맵에 존재하는 적 리스트 정보를 얻기 위한 변수
    [SerializeField]
    private PlayerGold playerGold;                  // 플레이어가 가지고 있는 돈을 담고 있는 변수
    [SerializeField]
    private SystemTextViewer systemTextViewer;      // 돈 부족, 건설 불가와 같은 시스템 메시지 출력
    private bool isOnTowerButton = false;           // 타워 건설 버튼을 눌렀는지 체크
    private GameObject followTowerClone = null;     // 임시 타워 사용 완료 시 삭제를 위해 저장하는 변수
    private int towerType;                          // 타워 속성(종류)

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        // 버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if ( isOnTowerButton == true )
        {
            return;
        }
        
        // 타워 건설 가능 여부 확인
        // 1. 타워를 건설할 만큼 돈이 없으면 타워 건설을 하지 않음
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            // 골드가 부족해서 타워 건설이 불가능하다고 출력함
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        // 타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;
        // 마우스를 따라다니는 임시 타워 생성
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        // 타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        // 타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if (isOnTowerButton == false)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();
        
        // 2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설을 하지 않음
        if (tile.IsBuildTower == true)
        {
            // 현재 위치에 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        // 다시 타워 건설 버튼을 눌러서 타워를 건설할 수 있도록 false로 바꿈
        isOnTowerButton = false;
        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        // 타워 건설에 필요한 만큼 골드를 소모
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // 선택한 타일의 위치에 타워 건설 (타일보다 z축 -1의 위치에 배치)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        // 타워 무기에 자신과 enemySpawner, playerGold, tile 정보 전달
        clone.GetComponent<TowerWeapon>().SetUp(this, enemySpawner, playerGold, tile);
        // 새로 배치되는 타워가 버프 타워 주변에 배치될 경우 버프 효과를 받을 수 있도록 모든 버프 타워의 버프 효과 갱신
        OnBuffAllBuffTowers();
        // 타워를 배치했기 때문에 마우스를 따라다니는 임시 객체를 삭제한다
        Destroy(followTowerClone);
        // 타워 건설을 취소할 수 있는 코루틴 함수 중지 (이미 건설이 되었으므로)
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            // ESC키 또는 마우스 오른쪽 버튼을 눌렀을 때 타워 건설 취소
            if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) )
            {
                isOnTowerButton = false;
                // 마우스를 따라다니는 임시 객체 삭제
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        // 맵에 배치된 모든 타워를 찾아 저장
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        // 배열을 순회
        for (int i = 0; i < towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();
            // 만약 버프 타워인 경우 함수를 호출해 버프 정보를 갱신한다.
            if (weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
