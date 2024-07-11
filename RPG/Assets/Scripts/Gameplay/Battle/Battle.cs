using System.Collections;
using System.Collections.Generic;
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
    public Drone playerDrone;
    public Drone enemyDrone;
    public ActionPanel actionPanel; 
    private bool _playerTurn = true;
    private List<QueuedAction> _actionQueue = new List<QueuedAction>();
    private EnemiesSpawn _enemiesSpawn;
   

    void Start()
    {
        _enemiesSpawn = GameObject.FindAnyObjectByType<EnemiesSpawn>();
        //StartCoroutine(BattleSequence());
    }

    public void StartBattle()
    {
        SpawnPlayerCreatures();
        SpawnEnemyCreatures();
        StartCoroutine(BattleSequence());
    }
    private void SpawnEnemyCreatures()
    {
        enemyDrone.activeCreatures= _enemiesSpawn.SpawnEnemies(3, _enemySpawnPoints);
    }
    private void SpawnPlayerCreatures()
    {
        List<Transform> spawnPoints = new List<Transform> { _playerSpawnPoint1, _playerSpawnPoint2, _playerSpawnPoint3 };
        for (int i = 0; i < playerDrone.cards.Count && i < spawnPoints.Count; i++)
        {
            GameObject creatureObj = Instantiate(playerDrone.cards[i].creaturePrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            Creature creature = creatureObj.GetComponent<Creature>();
            playerDrone.activeCreatures.Add(creature);
            creature.Initialize("Players creature number " + (i + 1));
        }
    }

    IEnumerator BattleSequence()
    {
        yield return new WaitForSeconds(0.5f); // Ждем немного для корректного добавления существ

        while (AreCreaturesAlive(playerDrone) && AreCreaturesAlive(enemyDrone))
        {
            _actionQueue.Clear();

            if (_playerTurn)
            {
                foreach (var creature in playerDrone.activeCreatures)
                {
                    if (!creature.IsFainted())
                    {
                        actionPanel.ShowActions(creature, OnAbilityChosen, OnItemChosen);
                        yield return new WaitUntil(() => _actionQueue.Exists(a => a.creature == creature));
                    }
                }
            }
            else
            {
                foreach (var creature in enemyDrone.activeCreatures)
                {
                    if (!creature.IsFainted())
                    {
                        // Вражеский дрон случайным образом выбирает действие
                        if (Random.Range(0, 2) == 0 && creature.abilities.Count > 0)
                        {
                            Ability enemyAbility = creature.abilities[Random.Range(0, creature.abilities.Count)];
                            _actionQueue.Add(new QueuedAction(creature, enemyAbility));
                        }
                        else if (enemyDrone.items.Count > 0)
                        {
                            Item enemyItem = enemyDrone.items[Random.Range(0, enemyDrone.items.Count)];
                            _actionQueue.Add(new QueuedAction(creature, enemyItem));
                        }
                    }
                }
            }

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

        // Уничтожаем всех существ игрока и очищаем список
        Debug.Log("Removing player's creatures...");
        foreach (var creature in playerDrone.activeCreatures)
        {
            Debug.Log($"Destroying {creature.creatureName}");
            Destroy(creature.gameObject);
        }
        playerDrone.activeCreatures.Clear();

        // Уничтожаем всех существ врага и очищаем список
        Debug.Log("Removing enemy's creatures...");
        foreach (var creature in enemyDrone.activeCreatures)
        {
            Debug.Log($"Destroying {creature.creatureName}");
            Destroy(creature.gameObject);
        }
        enemyDrone.activeCreatures.Clear();

        Debug.Log("All creatures have been removed and lists cleared.");
    }

    IEnumerator ExecuteTurn()
    {
        foreach (var action in _actionQueue)
        {
            if (action.creature.IsFainted())
            {
                // Пропускаем действия мертвых существ
                continue;
            }

            if (action.ability != null)
            {
                var target = GetRandomAliveCreature(_playerTurn ? enemyDrone.activeCreatures : playerDrone.activeCreatures);
                while (target != null && target.IsFainted())
                {
                    target = GetRandomAliveCreature(_playerTurn ? enemyDrone.activeCreatures : playerDrone.activeCreatures);
                }

                if (target != null)
                {
                    int damage = CalculateDamage(action.creature, target, action.ability);
                    target.TakeDamage(damage);
                    Debug.Log($"{action.creature.creatureName} used {action.ability.abilityName}! It dealt {damage} damage to {target.creatureName}.");
                    Debug.Log(target.ToString());

                    if (target.IsFainted())
                    {
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
        // Упрощенная формула расчета урона
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

    void OnAbilityChosen(Creature creature, Ability chosenAbility)
    {
        _actionQueue.Add(new QueuedAction(creature, chosenAbility));
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
    public Item item;

    public QueuedAction(Creature creature, Ability ability)
    {
        this.creature = creature;
        this.ability = ability;
    }

    public QueuedAction(Creature creature, Item item)
    {
        this.creature = creature;
        this.item = item;
    }
}
