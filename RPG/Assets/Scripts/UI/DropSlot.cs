using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    private const int MaxCreatures = 3;
    private List<GameObject> droppedObjects = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject droppedObject = eventData.pointerDrag;
            Card creatureCard = droppedObject.GetComponent<Card>();

            if (creatureCard != null)
            {
                if (droppedObjects.Count < MaxCreatures)
                {
                    droppedObjects.Add(droppedObject);
                }
                else
                {
                    // Заменяем объект
                    ReplaceDroppedObject(droppedObject);
                }

                // Обновляем родителя и позицию сброшенного объекта
                RectTransform droppedRectTransform = droppedObject.GetComponent<RectTransform>();
                droppedRectTransform.SetParent(transform);
                droppedRectTransform.anchoredPosition = Vector2.zero;

                // Обновляем Drone
                UpdateDrone(creatureCard, droppedObject);
            }
        }
    }

    private void ReplaceDroppedObject(GameObject newObject)
    {
        // Определяем индекс объекта для замены на основе позиции
        int indexToReplace = droppedObjects.Count - 1;
        GameObject oldObject = droppedObjects[indexToReplace];
        droppedObjects[indexToReplace] = newObject;

        // Удаляем старый объект (или перемещаем его куда-то еще)
        Destroy(oldObject);
    }

    private void UpdateDrone(Card newCreatureCard, GameObject droppedObject)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Drone drone = player.GetComponent<Drone>();
            if (drone != null)
            {
                if (drone.cards.Count < MaxCreatures)
                {
                    drone.cards.Add(newCreatureCard);
                }
                else
                {
                    // Находим индекс карты для замены
                    int indexToReplace = droppedObjects.IndexOf(droppedObject);

                    // Если индекс не найден, по умолчанию заменяем последнюю карту
                    if (indexToReplace < 0 || indexToReplace >= MaxCreatures)
                    {
                        indexToReplace = MaxCreatures - 1;
                    }

                    // Заменяем карту
                    drone.cards[indexToReplace] = newCreatureCard;
                }
            }
        }
    }
}
