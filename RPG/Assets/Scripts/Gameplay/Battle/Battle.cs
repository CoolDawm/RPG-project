using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _enemySpawnPoints;
    [SerializeField]
    private Transform _playerSpawnPoint1;
    [SerializeField]
    private Transform _playerSpawnPoint2;
    [SerializeField]
    private Transform _playerSpawnPoint3;
    [SerializeField] 
    private GameObject turnOrderPanel; 
    [SerializeField] 
    private GameObject playerIconPrefab; 
    [SerializeField]
    private GameObject enemyIconPrefab; 
    public Drone playerDrone;
    public Drone enemyDrone;
    public ActionPanel actionPanel;
    private bool _playerTurn = true;
    private List<QueuedAction> _actionQueue = new List<QueuedAction>();
    private EnemiesSpawn _enemiesSpawn;
    private CamerasManager _cameraManager;
    private PlayerMovement _playerMovement;
    private GameObject _currentTrigger;
    private MissionManager _missionManager;
    private FloatTextManager _floatTextManager;
    void Start()
    {
        _enemiesSpawn = FindAnyObjectByType<EnemiesSpawn>();
        _cameraManager = FindAnyObjectByType<CamerasManager>();
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        _missionManager = FindAnyObjectByType<MissionManager>();
        _floatTextManager=FindAnyObjectByType<FloatTextManager>();
        //StartCoroutine(BattleSequence());
    }
    
    public void StartBattle(GameObject trigger)
    {
        SpawnPlayerCreatures();
        SpawnEnemyCreatures();
        StartCoroutine(BattleSequence());
        _playerMovement.ChangeBattleStatus();
        _currentTrigger = trigger;
        actionPanel.transform.root.gameObject.SetActive(true);

    }
    private void UpdateTurnOrderUI()
    {
        // ќчистить старые иконки
        foreach (Transform child in turnOrderPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // —оздать новые иконки в пор€дке хода из _actionQueue
        foreach (var action in _actionQueue)
        {
            GameObject icon;
            if (playerDrone.activeCreatures.Contains(action.creature))
            {
                icon = Instantiate(playerIconPrefab, turnOrderPanel.transform);
            }
            else
            {
                icon = Instantiate(enemyIconPrefab, turnOrderPanel.transform);
            }
            // ћожно добавить дополнительную настройку дл€ иконки здесь, если нужно
        }
    }
    private void SpawnEnemyCreatures()
    {
        enemyDrone.activeCreatures = _enemiesSpawn.SpawnEnemies(3, _enemySpawnPoints);
    }
    private void SpawnPlayerCreatures()
    {
        List<Transform> spawnPoints = new List<Transform> { _playerSpawnPoint1, _playerSpawnPoint2, _playerSpawnPoint3 };
        for (int i = 0; i < playerDrone.cards.Count && i < spawnPoints.Count; i++)
        {
            GameObject creatureObj = Instantiate(playerDrone.cards[i].creaturePrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            Creature creature = creatureObj.GetComponent<Creature>();
            playerDrone.activeCreatures.Add(creature);
            creature.Initialize($"Players {creature.creatureName} " + (i + 1));
        }
    }

    private IEnumerator AddActionsToQueue()
    {
        foreach (var creature in playerDrone.activeCreatures)
        {
            if (!creature.IsFainted())
            {
                actionPanel.ShowActions(creature, OnAbilityChosen, OnItemChosen);
                yield return new WaitUntil(() => _actionQueue.Exists(a => a.creature == creature));
            }
        }

        foreach (var creature in enemyDrone.activeCreatures)
        {
            if (!creature.IsFainted())
            {
                if (Random.Range(0, 2) == 0 && creature.abilities.Count > 0)
                {
                    Ability enemyAbility = creature.abilities[Random.Range(0, creature.abilities.Count)];
                    var target = GetRandomAliveCreature(playerDrone.activeCreatures);
                    _actionQueue.Add(new QueuedAction(creature, enemyAbility, target));
                }
                else if (enemyDrone.items.Count > 0)
                {
                    Item enemyItem = enemyDrone.items[Random.Range(0, enemyDrone.items.Count)];
                    _actionQueue.Add(new QueuedAction(creature, enemyItem));
                }
            }
        }
    }
    private void SortActionQueue()
    {
        _actionQueue = _actionQueue.OrderByDescending(a => a.creature.speed)
                                   .ThenBy(a => Random.value)
                                   .ToList();
    }
    IEnumerator BattleSequence()
    {
        yield return new WaitForSeconds(0.5f);

        while (AreCreaturesAlive(playerDrone) && AreCreaturesAlive(enemyDrone))
        {
            _actionQueue.Clear();

            // ƒобавл€ем действи€ всех существ в очередь
            yield return StartCoroutine(AddActionsToQueue());

            // —ортируем очередь по скорости и случайным образом дл€ одинаковой скорости
            SortActionQueue();

            // ќбновл€ем UI после формировани€ очереди действий
            UpdateTurnOrderUI();

            // ¬ыполн€ем действи€ в отсортированном пор€дке
            yield return ExecuteTurn();

            _playerTurn = !_playerTurn;
        }

        Debug.Log("Battle over!");
        if (!AreCreaturesAlive(playerDrone))
        {
            Debug.Log($"{enemyDrone.droneName} wins!");
        }
        else
        {
            Debug.Log($"{playerDrone.droneName} wins!");
        }

        EndBattle();
    }

    void EndBattle()
    {

        Debug.Log("Removing player's creatures...");
        foreach (var creature in playerDrone.activeCreatures)
        {
            Debug.Log($"Destroying {creature.creatureName}");
            Destroy(creature.gameObject);
        }
        playerDrone.activeCreatures.Clear();
        Debug.Log("Removing enemy's creatures...");
        foreach (var creature in enemyDrone.activeCreatures)
        {
            Debug.Log($"Destroying {creature.creatureName}");
            Destroy(creature.gameObject);
        }
        enemyDrone.activeCreatures.Clear();

        Debug.Log("All creatures have been removed and lists cleared.");
        actionPanel.transform.root.gameObject.SetActive(false);
        _cameraManager.ActivatePseudo2DCamera();
        _playerMovement.ChangeBattleStatus();
        Destroy(_currentTrigger);
        _missionManager.InvokeCheckingAfterTime();
    }

    IEnumerator ExecuteTurn()
    {
        foreach (var action in _actionQueue)
        {
            if (action.creature.IsFainted())
            {
                // ѕропускаем действи€ мертвых существ
                continue;
            }

            if (action.ability != null)
            {
                var target = action.target; // Use the selected target
                if (target != null)
                {
                    action.creature.GetComponentInChildren<Animator>().SetTrigger("Melee Attack");
                    yield return new WaitForSeconds(1.3f);

                    int damage = CalculateDamage(action.creature, target, action.ability);
                    target.TakeDamage(damage);
                    target.GetComponentInChildren<Animator>().SetTrigger("Take Damage");
                    yield return new WaitForSeconds(1f);

                    _floatTextManager.ShowFloatingNumbers(target.gameObject.transform,damage,Color.red);
                    Debug.Log($"{action.creature.creatureName} used {action.ability.abilityName}! It dealt {damage} damage to {target.creatureName}.");
                    Debug.Log(target.ToString());

                    if (target.IsFainted())
                    {
                        target.GetComponentInChildren<Animator>().SetTrigger("Die");
                        CheckForWinner();
                    }
                }
            }
            else if (action.item != null)
            {
                playerDrone.UseItem(action.creature, action.item);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    int CalculateDamage(Creature attacker, Creature defender, Ability ability)
    {
        // ”прощенна€ формула расчета урона
        return (int)((2 * attacker.attack / (double)defender.defense) * ability.power);
    }

    bool AreCreaturesAlive(Drone drone)
    {
        foreach (var creature in drone.activeCreatures)
        {
            if (!creature.IsFainted())
            {
                return true;
            }
        }
        EndBattle();
        return false;
    }

    void OnAbilityChosen(Creature creature, Ability chosenAbility, Creature target)
    {
        _actionQueue.Add(new QueuedAction(creature, chosenAbility, target));
    }

    void OnItemChosen(Creature creature, Item chosenItem)
    {
        _actionQueue.Add(new QueuedAction(creature, chosenItem));
    }

    void CheckForWinner()
    {
        if (!AreCreaturesAlive(playerDrone))
        {
            Debug.Log("Battle over!");
            Debug.Log($"{enemyDrone.droneName} wins!");
            StopAllCoroutines();
        }
        else if (!AreCreaturesAlive(enemyDrone))
        {
            Debug.Log("Battle over!");
            Debug.Log($"{playerDrone.droneName} wins!");
            StopAllCoroutines();
        }
    }

    Creature GetRandomAliveCreature(List<Creature> creatures)
    {
        var aliveCreatures = creatures.FindAll(c => !c.IsFainted());
        if (aliveCreatures.Count > 0)
        {
            return aliveCreatures[Random.Range(0, aliveCreatures.Count)];
        }
        return null;
    }
}

public class QueuedAction
{
    public Creature creature;
    public Ability ability;
    public Creature target;
    public Item item;

    public QueuedAction(Creature creature, Ability ability, Creature target)
    {
        this.creature = creature;
        this.ability = ability;
        this.target = target;
    }

    public QueuedAction(Creature creature, Item item)
    {
        this.creature = creature;
        this.item = item;
    }
}
