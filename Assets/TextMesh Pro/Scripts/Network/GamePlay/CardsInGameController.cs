using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsInGameController : MonoBehaviour
{
    private List<CardAttack> cardinGame = new List<CardAttack>();

    public Transform positionCardsAttack;
    public Transform minCardsPosition;
    public Transform maxCardsPosition;

    public void AddInGame(CardBase card)
    {
        if (card != null)
        {
            cardinGame.Add(card as CardAttack);
            card.SetStartPosition(positionCardsAttack.position);
            ReorganizeTable();
        }
    }

    public void ReorganizeTable()
    {
        Vector3 position = transform.position;
        for (int i = 1; i < cardinGame.Count + 1; i++)
        {
            position = CalcDistanceTablePosition(i, cardinGame.Count + 1);
            if (i - 1 < cardinGame.Count)
            { 
                cardinGame[i - 1].SetStartPosition(position);
            }
        }
    }


    private Vector3 CalcDistanceTablePosition(int indice, int limit)
    {
        float distance = indice / (float)(limit);
        return Vector3.Lerp(minCardsPosition.position, maxCardsPosition.position, distance);
    }

}
