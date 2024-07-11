using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public string creatureName;
    public int maxHP;
    public int attack;
    public int defense;
    public int speed;
    public List<Ability> abilities;

    private int currentHP;

    void Start()
    {
    }

    public void Initialize(string name)
    {
        creatureName = name;
        currentHP = maxHP;
    }

    public bool IsFainted()
    {
        return currentHP <= 0;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
        {
            currentHP = 0;

        }
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public override string ToString()
    {
        return $"{creatureName} (HP: {currentHP}/{maxHP})";
    }
}
