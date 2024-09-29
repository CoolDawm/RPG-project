using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemiesPrefabs = new List<GameObject>();
    public List<Creature> SpawnEnemies(int amount,List<Transform> spawnPoints)
    {
        List<Creature> enemies = new List<Creature>();
        for(int i = 0; i < amount; i++)
        {
            GameObject creatureObj = Instantiate(_enemiesPrefabs[Random.Range(0,_enemiesPrefabs.Count)], spawnPoints[i].position, spawnPoints[i].rotation);
            creatureObj.tag = "Enemy";
            Creature creature = creatureObj.GetComponent<Creature>();
           
            creature.Initialize("Enemy creature number " + (i + 1));
            enemies.Add(creature);
        }
        return enemies;
    }
}
