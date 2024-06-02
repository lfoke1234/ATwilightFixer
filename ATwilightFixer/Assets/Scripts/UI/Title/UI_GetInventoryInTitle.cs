using UnityEngine;
using UnityEngine.UI;

public class UI_GetInventoryInTitle : MonoBehaviour
{
    [SerializeField] private Button[] getInventoryButtons;

    void Start()
    {
        foreach (Button button in getInventoryButtons)
        {
            button.onClick.AddListener(GetPlayerInventory);
        }
    }

    private void GetPlayerInventory()
    {
        UI.instance.SwitchTo(UI.instance.GetInventory());
    }
}
