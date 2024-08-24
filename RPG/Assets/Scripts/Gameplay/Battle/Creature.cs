using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public string creatureName;
    public int maxHP;
    public int attack;
    public int defense;
    public int speed;
    public int experience;
    public List<Ability> abilities;

    private int _currentHP;

    void Start()
    {
    }

    public void Initialize(string name)
    {
        creatureName = name;
        _currentHP = maxHP;
    }

    public bool IsFainted()
    {
        return _currentHP <= 0;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0)
        {
            _currentHP = 0;

        }
    }

    public int GetCurrentHP()
    {
        return _currentHP;
    }

    public void Heal(int amount)
    {
        _currentHP += amount;
        if (_currentHP > maxHP)
        {
            _currentHP = maxHP;
        }
    }

    public override string ToString()
    {
        return $"{creatureName} (HP: {_currentHP}/{maxHP})";
    }
}
