using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GoldCurrent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentGold;

    private void Update()
    {
        currentGold.text = PlayerManager.instance.GetCurrency().ToString("N0");
    }
}
