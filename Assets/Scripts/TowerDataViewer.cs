using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;                       // 타워의 이미지
    [SerializeField]
    private TextMeshProUGUI textDamage;             // 타워의 대미지
    [SerializeField]
    private TextMeshProUGUI textRate;               // 타워의 공격 간격
    [SerializeField]
    private TextMeshProUGUI textRange;              // 타워의 공격 범위
    [SerializeField]
    private TextMeshProUGUI textLevel;              // 타워의 레벨
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;        // 타워의 업그레이드 비용
    [SerializeField]
    private TextMeshProUGUI textSellCost;           // 타워의 판매 비용
    [SerializeField]
    private TowerAttackRange towerAttackRange;      // 타워의 공격범위를 표시하기 위한 변수
    [SerializeField]
    private Button buttonUpgrade;                   // 업그레이드 버튼을 담는 변수
    [SerializeField]
    private SystemTextViewer systemTextViewer;      // 시스템 메시지를 출력하기 위한 변수

    private TowerWeapon currentTower;
    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // 출력해야하는 타워 정보를 받아와서 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        // 타워 정보 Panel On
        gameObject.SetActive(true);
        // 타워 정보 갱신
        UpdateTowerData();
        // 타워 오브젝트 주변에 표시되는 타워 공격범위 Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel()
    {
        // 타워 정보 Panel Off
        gameObject.SetActive(false);
        // 타워 공격범위 Sprite Off
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            // 타워의 이미지 크기
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            // 타워의 대미지 출력(버프에 의한 것은 빨간색으로)
            textDamage.text = "Damage : " + currentTower.Damage
                            + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
        }
        else
        {
            // 타워의 이미지 크기
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            // 감속 타워의 경우 - 대미지 대신 감속률을 출력함
            if (currentTower.WeaponType == WeaponType.Slow)
            {
                // 타워의 감속률 출력
                textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
            }
            // 버프 타워의 경우 - 대미지 대신 공격력 증가율을 출력함
            else if (currentTower.WeaponType == WeaponType.Buff)
            {
                // 공격력 증가율 출력
                textDamage.text = "Buff : " + currentTower.Buff * 100 + "%";
            }
            
        }
        // 타워의 이미지 UI
        imageTower.sprite = currentTower.TowerSprite;
        // 타워의 나머지 정보들을 텍스트로 표시
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();

        // 업그레이드가 불가능하면(최대 레벨일 때) 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        // 타워 업그레이드 시도 (성공 : true, 실패 : false)
        bool isSuccess = currentTower.Upgrade();

        if ( isSuccess == true )
        {
            // 타워가 업그레이드 되었기 때문에 타워 정보 갱신
            UpdateTowerData();
            // 타워 주변에 보이는 공격범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            // 타워 업그레이드 비용이 부족하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        // 타워 판매
        currentTower.Sell();
        // 선택한 타워가 사라졌으므로 Panel, 공격범위를 Off한다.
        OffPanel();
    }
}
