using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionChain", menuName = "Evolution Chain")]
public class EvolutionChain : ScriptableObject
{
    public List<EvolutionPair> evolutionPairs;

    [System.Serializable]
    public class EvolutionPair
    {
        public GameObject currentPrefab;
        public GameObject nextPrefab;
    }

    public GameObject GetNextEvolution(GameObject currentPrefab)
    {
        foreach (var pair in evolutionPairs)
        {
            if (pair.currentPrefab == currentPrefab)
            {
                return pair.nextPrefab;
            }
        }
        return null;
    }
}
