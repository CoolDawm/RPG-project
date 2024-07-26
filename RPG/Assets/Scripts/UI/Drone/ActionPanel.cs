using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    public List<Button> abilityButtons;
    public Button itemButton;
    public TextMeshProUGUI creatureNameText;
    public GameObject targetPanel; // New panel for selecting targets
    public Button targetButtonPrefab; // Prefab for target buttons

    private Creature _currentCreature;
    private Ability _chosenAbility;
    private System.Action<Creature, Ability, Creature> _onAbilityChosen;
    private System.Action<Creature, Item> _onItemChosen;

    public void ShowActions(Creature creature, System.Action<Creature, Ability, Creature> onAbilityChosen, System.Action<Creature, Item> onItemChosen)
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
        _chosenAbility = ability;
        ShowTargetPanel(); // Show the target selection panel
    }

    void ShowTargetPanel()
    {
        // Show the panel and create buttons for each alive enemy
        targetPanel.SetActive(true);
        foreach (Transform child in targetPanel.transform)
        {
            Destroy(child.gameObject);
        }

        List<Creature> aliveEnemies = FindObjectOfType<Battle>().enemyDrone.activeCreatures.FindAll(c => !c.IsFainted());
        foreach (var target in aliveEnemies)
        {
            Button targetButton = Instantiate(targetButtonPrefab, targetPanel.transform);
            targetButton.GetComponentInChildren<TextMeshProUGUI>().text = target.creatureName;
            targetButton.onClick.AddListener(() => OnTargetButtonClicked(target));
        }
    }

    void OnTargetButtonClicked(Creature target)
    {
        _onAbilityChosen?.Invoke(_currentCreature, _chosenAbility, target);
        targetPanel.SetActive(false);
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
