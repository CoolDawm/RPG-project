using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    public List<Button> abilityButtons;
    public Button itemButton;
    public TextMeshProUGUI creatureNameText;

    private Creature _currentCreature;
    private System.Action<Creature, Ability> _onAbilityChosen;
    private System.Action<Creature, Item> _onItemChosen;

    public void ShowActions(Creature creature, System.Action<Creature, Ability> onAbilityChosen, System.Action<Creature, Item> onItemChosen)
    {
        this._currentCreature = creature;
        this._onAbilityChosen = onAbilityChosen;
        this._onItemChosen = onItemChosen;

        creatureNameText.text = creature.creatureName;

        for (int i = 0; i < abilityButtons.Count; i++)
        {
            if (i < creature.abilities.Count)
            {
                abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = creature.abilities[i].abilityName;
                abilityButtons[i].gameObject.SetActive(true);
                int index = i;
                abilityButtons[i].onClick.RemoveAllListeners();
                abilityButtons[i].onClick.AddListener(() => OnAbilityButtonClicked(creature.abilities[index]));
            }
            else
            {
                abilityButtons[i].gameObject.SetActive(false);
            }
        }

        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(OnItemButtonClicked);

        gameObject.SetActive(true);
    }

    void OnAbilityButtonClicked(Ability ability)
    {
        _onAbilityChosen?.Invoke(_currentCreature, ability);
        gameObject.SetActive(false);
    }

    void OnItemButtonClicked()
    {
        // Здесь вы можете выбрать предмет из списка доступных предметов
        // Для примера:
        Item chosenItem = FindObjectOfType<Battle>().playerDrone.items[0];
        _onItemChosen?.Invoke(_currentCreature, chosenItem);
        gameObject.SetActive(false);
    }
}
