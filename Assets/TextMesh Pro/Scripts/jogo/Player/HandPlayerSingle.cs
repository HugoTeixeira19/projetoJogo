using System.Collections.Generic;
using UnityEngine;

public class HandPlayerSingle : MonoBehaviour
{
    private PlayerBaseSingle player;
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
        // definindo a posição mínima e máxima da mão do player
        minPosition = transform.position - rangeCardPosition;
        maxPosition = transform.position + rangeCardPosition;
    }

    // Update is called once per frame
    void Update()
    {
        /*if ((!(cards.Count == 0)) && Input.GetKeyDown(KeyCode.U) && player.canPlayerControl)
        {
            player.table.AddInGame(cards[0]);
            cards.RemoveAt(0);
            ReorganizeCards();
        }*/
    }


    // método que joga carta na mesa
    public void playCard(bool canPlayerControl, CardBase card)
    {
        if (card != null)
        {
            player.table.AddInGame(card);
            cards.RemoveAt(cards.IndexOf(card));
            ReorganizeCards();
        }
    }

    // Setando player atual
    public void SetPlayer(PlayerBaseSingle playerToSet)
    {
        player = playerToSet;
    }

    // Reorganiza os cards em sua mão
    public void ReorganizeCards()
    {
        Vector3 position = transform.position;
        for(int i = 1; i < cards.Count; i++)
        {
            position = CalcDistanceHandPosition(i, cards.Count + 1);
            if(i - 1 < cards.Count)
            {
                cards[i - 1].SetStartPositionOffline(position);
            }
        }

        positionNextCard = CalcDistanceHandPosition(cards.Count, cards.Count + 1);
    }

    // calcula a distância mínima e máxima da mão, para que seja feita a distribuição das cartas corretamente
    private Vector3 CalcDistanceHandPosition(int indice, int limit)
    {
        float distance = indice / (float) (limit);
        return Vector3.Lerp(minPosition, maxPosition, distance);
    }

    // Adiciona a card na mão do player
    public void AddCard(CardBase card, bool canPlayerControl)
    {
        card.SetOwer(canPlayerControl);

        if (cards.Count < 3)
        {
            cards.Add(card);
            ReorganizeCards();
        } else
        {
            Destroy(card.gameObject, 0.7f);
        }
    }

    public List<CardBase> Cards
    {
        get {
            return cards;
        }
    }
}
