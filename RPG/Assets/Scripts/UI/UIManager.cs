using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diamondsText;
    private CurrencyManager _currencyManager;
    void Start()
    {
        _currencyManager= GameObject.FindObjectOfType<CurrencyManager>();
        goldText = GameObject.Find("Gold").GetComponent<TextMeshProUGUI>();
        diamondsText = GameObject.Find("Diamonds").GetComponent<TextMeshProUGUI>();
        _currencyManager.onCurrencyChange += UpdateCurrencyUI;
        UpdateCurrencyUI();
    }
    private void UpdateCurrencyUI()
    {
        if (goldText != null)
            goldText.text = "Gold: " + _currencyManager.Gold;

        if (diamondsText != null)
            diamondsText.text = "Diamonds: " + _currencyManager.Diamonds;
    }
   
}
