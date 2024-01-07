using UnityEngine;
using TMPro;

public enum SystemType { Money = 0, Build }

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;
    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        // 시스템 텍스트를 출력한다.
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System : Not enough money...";
                break;
            case SystemType.Build:
                textSystem.text = "System : Invaild build tower...";
                break;
        }
        // 시스템 텍스트가 출력된 뒤 서서히 사라지게 한다.
        tmpAlpha.FadeOut();
    }
}
