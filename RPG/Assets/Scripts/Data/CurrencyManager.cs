using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public int Gold { get; private set; }
    public int Diamonds { get; private set; }
 
    public Action onCurrencyChange;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Gold = 100;
        Diamonds = 100;
    }
    public void AddGold(int amount)
    {
        Gold += amount;
        onCurrencyChange?.Invoke();
    }

    public void SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            onCurrencyChange?.Invoke();

        }
        else
        {
            Debug.Log("Not enough gold!" );
        }
    }

    public void AddDiamonds(int amount)
    {     
        Diamonds += amount;
        onCurrencyChange?.Invoke();

    }

    public void SpendDiamonds(int amount)
    {
        if (Diamonds >= amount)
        {
            Diamonds -= amount;
            onCurrencyChange?.Invoke();

        }
        else
        {
            Debug.Log("Not enough diamonds!");
        }
    }

    public void ApplyMissionReward(int reward)
    {
        AddGold(reward);
    }

    
}
