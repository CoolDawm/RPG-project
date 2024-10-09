using UnityEngine;

public class SyncCardsWithDrone : MonoBehaviour
{
    public GameObject sourceObject; 
    public GameObject targetObject; 

    void FixedUpdate()
    {
        Sync();
    }

    void Sync()
    {
        for (int i = targetObject.transform.childCount - 1; i >= 0; i--)
        {
            Transform targetChild = targetObject.transform.GetChild(i);
            if (!HasChildWithName(sourceObject.transform, targetChild))
            {
                Destroy(targetChild.gameObject);
            }
        }

        for (int i = 0; i < sourceObject.transform.childCount; i++)
        {
            Transform sourceChild = sourceObject.transform.GetChild(i);
            Card sourceCard = sourceChild.GetComponent<Card>();

            if (sourceCard != null)
            {
                Transform targetChild = null;

                foreach (Transform child in targetObject.transform)
                {
                    Card targetCard = child.GetComponent<Card>();
                    if (targetCard != null && targetCard.cardId == sourceCard.cardId)
                    {
                        targetChild = child;
                        break;
                    }
                }

                if (targetChild == null)
                {

                    GameObject newChild = Instantiate(sourceChild.gameObject, targetObject.transform);
                    newChild.name = sourceChild.name; // Сохранение имени, если нужно
                    newChild.GetComponent<Card>().cardId = sourceCard.cardId;
                    newChild.AddComponent<Draggable>();
                }
                else
                {
                    // UpdateChild(targetChild, sourceChild);
                }
            }
        }
    }

    bool HasChildWithName(Transform parent, Transform obj2)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<Card>().cardId == obj2.GetComponent<Card>().cardId)
            {
                return true;
            }
        }
        return false;
    }

    void UpdateChild(Transform targetChild, Transform sourceChild)
    {
        targetChild.position = sourceChild.position;
        targetChild.rotation = sourceChild.rotation;
        targetChild.localScale = sourceChild.localScale;
    }
}
