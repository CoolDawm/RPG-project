using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Creature : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private int _currentHP;
    public string creatureName;
    public int maxHP;//should be private
    public int attack;
    public int defense;
    public int speed;
    public int experience;
    public List<Ability> abilities;
    private Light _creatureHighlight;

    void Start()
    {
        _creatureHighlight= GetComponentInChildren<Light>() ;
        _creatureHighlight.enabled = false ;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("In");
        if (tag != "Enemy") return;
        Debug.Log("In 2");

        ActionPanel panel = FindAnyObjectByType<ActionPanel>();
        if (panel.isAwaitingForTarget)
        {
            Debug.Log("In 3");

            panel.ChooseTarget(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tag != "Enemy") return;

        _creatureHighlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tag != "Enemy") return;

        _creatureHighlight.enabled = false;
    }
}
