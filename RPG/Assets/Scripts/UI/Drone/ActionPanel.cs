using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToHide;  // UI
    [SerializeField]
    private List<GameObject> objectsToShow;  // UI
    [SerializeField]
    private GameObject _creatureHPBar;
    public List<Button> abilityButtons;
    public Button itemButton;
    public TextMeshProUGUI creatureNameText;
    public GameObject targetPanel; 
    public Button targetButtonPrefab; 
    public GameObject itemButtonPrefab; 
    public Transform itemsPanel;
    public bool isAwaitingForTarget=false;
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
        if (_currentCreature.maxHP != _currentCreature.GetCurrentHP())
        {
           
            _creatureHPBar.GetComponent<Image>().fillAmount = (float)(_currentCreature.maxHP - _currentCreature.GetCurrentHP()) / _currentCreature.maxHP;

        }
       
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

        foreach (Transform child in itemsPanel)
        {
            Destroy(child.gameObject);
        }

        var items = FindObjectOfType<Battle>().playerDrone.items;
        var uniqueItems = new Dictionary<string, int>(); 
        foreach (var item in items)
        {
            if (uniqueItems.ContainsKey(item.itemName))
            {
                uniqueItems[item.itemName]++;
            }
            else
            {
                uniqueItems[item.itemName] = 1;
            }
        }

        foreach (var uniqueItem in uniqueItems)
        {
            GameObject newItemButton = Instantiate(itemButtonPrefab, itemsPanel);
            newItemButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{uniqueItem.Key} x{uniqueItem.Value}";
            newItemButton.GetComponent<Button>().onClick.AddListener(() => OnItemButtonClicked(uniqueItem.Key));
        }

        gameObject.SetActive(true);

    }

    void OnAbilityButtonClicked(Ability ability)
    {
        _chosenAbility = ability;
        isAwaitingForTarget = true;
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
    public void ChooseTarget(Creature target)
    {
        _onAbilityChosen?.Invoke(_currentCreature, _chosenAbility, target);
        isAwaitingForTarget = false;

        foreach (var obj in objectsToHide)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectsToShow)
        {
            obj.SetActive(true);
        }
        targetPanel.SetActive(false);
        gameObject.SetActive(false);
    }
    void OnTargetButtonClicked(Creature target)
    {
        _onAbilityChosen?.Invoke(_currentCreature, _chosenAbility, target);
        isAwaitingForTarget = false;

        foreach (var obj in objectsToHide)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectsToShow)
        {
            obj.SetActive(true);
        }
        targetPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnItemButtonClicked(string itemName)
    {
        var items = FindObjectOfType<Battle>().playerDrone.items;
        Item chosenItem = items.FirstOrDefault(item => item.itemName == itemName);

        if (chosenItem != null)
        {
            _onItemChosen?.Invoke(_currentCreature, chosenItem);
        }

        gameObject.SetActive(false);
    }
}
