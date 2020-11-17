using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandPlayer : MonoBehaviour
{
    private PlayerBase player;
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private List<CardBase> cards = new List<CardBase>();

    // variável para setar área de visualização da carta quando for pega do deck
    public Transform positionToShowPlayer;
    public Vector3 rangeCardPosition;
    public int maxCardInHand = 3;

    // esconde atributo do inspetor
    [System.NonSerialized]
    public Vector3 positionNextCard;

    // Start is called before the first frame update
    void Start()
    {
        minPosition = transform.position - rangeCardPosition;
        maxPosition = transform.position + rangeCardPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void playCard(bool canPlayerControl, CardBase card)
    {
        player.table.AddInGame(card);
        Debug.Log(cards.IndexOf(card));
        cards.RemoveAt(cards.IndexOf(card));
        ReorganizeCards();
    }

    public void SetPlayer(PlayerBase playerToSet)
    {
        player = playerToSet;
    }

    public void ReorganizeCards()
    {
        Vector3 position = transform.position;
        for(int i = 1; i < cards.Count; i++)
        {
            position = CalcDistanceHandPosition(i, cards.Count + 1);
            if(i - 1 < cards.Count)
            {
                if (WaitingRoomController.waitingRoom.photonPlayers[0].IsLocal)
                {
                    cards[i - 1].zoomPosition = cards[i - 1].SetandoPlayerZoomPosition(position);
                }
                else
                {
                    cards[i - 1].SetStartPosition(position);
                }
            }
        }

        positionNextCard = CalcDistanceHandPosition(cards.Count, cards.Count + 1);
    }


    private Vector3 CalcDistanceHandPosition(int indice, int limit)
    {
        float distance = indice / (float) (limit);
        return Vector3.Lerp(minPosition, maxPosition, distance);
    }

    public void AddCard(CardBase card, bool isLocal)
    {
        card.SetOwer(isLocal);

        if (cards.Count < 3)
        {
            cards.Add(card);
            ReorganizeCards();
        } else
        {
            Destroy(card.gameObject, 0.7f);
        }
    }

    public List<CardBase> getCards
    {
        get
        {
            return cards;
        }
    }
}
