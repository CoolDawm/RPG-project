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
                    // �������� ������
                    ReplaceDroppedObject(droppedObject);
                }

                // ��������� �������� � ������� ����������� �������
                RectTransform droppedRectTransform = droppedObject.GetComponent<RectTransform>();
                droppedRectTransform.SetParent(transform);
                droppedRectTransform.anchoredPosition = Vector2.zero;

                // ��������� Drone
                UpdateDrone(creatureCard, droppedObject);
            }
        }
    }

    private void ReplaceDroppedObject(GameObject newObject)
    {
        // ���������� ������ ������� ��� ������ �� ������ �������
        int indexToReplace = droppedObjects.Count - 1;
        GameObject oldObject = droppedObjects[indexToReplace];
        droppedObjects[indexToReplace] = newObject;

        // ������� ������ ������ (��� ���������� ��� ����-�� ���)
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
                    // ������� ������ ����� ��� ������
                    int indexToReplace = droppedObjects.IndexOf(droppedObject);

                    // ���� ������ �� ������, �� ��������� �������� ��������� �����
                    if (indexToReplace < 0 || indexToReplace >= MaxCreatures)
                    {
                        indexToReplace = MaxCreatures - 1;
                    }

                    // �������� �����
                    drone.cards[indexToReplace] = newCreatureCard;
                }
            }
        }
    }
}
