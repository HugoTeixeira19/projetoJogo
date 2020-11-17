using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    private Vector3 initialSize;
    private int totalInitialCards;
    public List<CardBase> initListCards;
    private PlayerBase player;

    private GameObject tempCard;

    //animation variables
    public float timeToShowPlayer;
    public float dumbGetCard;
    private float currentTimeToShowPlayer;
    private bool moveToHand;
    private Vector3 positionShowPlayer;
    private Vector3 positionHand;
    private Vector3 targetPosition;

    private Vector3 newRotation;

    private bool canPlayerControl;

    // Start is called before the first frame update
    void Start()
    {
        initialSize = transform.localScale;
        totalInitialCards = initListCards.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            positionShowPlayer = player.hand.positionToShowPlayer.position;
            canPlayerControl = player.canPlayerControl;
        }
        if (moveToHand && tempCard != null)
        {
            currentTimeToShowPlayer += Time.deltaTime;
            if (currentTimeToShowPlayer > timeToShowPlayer)
            {
                positionHand = player.hand.positionNextCard;
                targetPosition = positionHand;
            }

            tempCard.transform.position = Vector3.Lerp(tempCard.transform.position, targetPosition, dumbGetCard * Time.deltaTime);

            if (Vector3.Distance(tempCard.transform.position, positionHand) < 2)
            {
                CardBase tempCardComponent = tempCard.GetComponent<CardBase>();

                if (player == GameSetup.GS.player1)
                {
                    Debug.Log("Entrou aqui cartas: " + tempCardComponent.zoomPosition);
                    tempCardComponent.zoomPosition = tempCardComponent.SetandoPlayerZoomPosition(positionHand);
                } else
                {
                    tempCardComponent.SetStartPosition(positionHand);
                }

                tempCardComponent.SetOnHand();

                tempCard = null;
                player.SetInit = true;
            }
        }        
    }

    public void GetCard(PlayerBase player)
    {
        if (initListCards.Count > 0)
        {
            int randCardIndex = UnityEngine.Random.Range(0, initListCards.Count);
            CardBase selectedCard = initListCards[randCardIndex];
            initListCards.RemoveAt(randCardIndex);

            selectedCard.transform.localEulerAngles = newRotation;

            if (PhotonNetwork.IsConnected)
            {
                Debug.Log(selectedCard.name);
                tempCard = (PhotonNetwork.Instantiate(Path.Combine("CardsOnline", selectedCard.name), transform.position, selectedCard.transform.rotation)) as GameObject;
            }
            else
            {
                tempCard = (Instantiate(selectedCard.gameObject, transform.position, selectedCard.transform.rotation)) as GameObject;
            }
            ResizeDeck();

            moveToHand = true;
            targetPosition = positionShowPlayer;
            currentTimeToShowPlayer = 0;

            Debug.Log("Carta é minha: " + player.view.IsMine);
            player.hand.AddCard(tempCard.GetComponent<CardBase>(), tempCard.GetComponent<CardBase>().GetComponent<PhotonView>().IsMine);
        }
    }

    public void setarRotacaoCard(Vector3 rotacao)
    {
        this.newRotation = rotacao;
    }

    private void ResizeDeck()
    {
        Vector3 newSize = transform.localScale;
        Vector3 aux = transform.localScale;
        newSize = ((initListCards.Count * initialSize) / totalInitialCards);

        newSize.x = aux.x;
        newSize.z = aux.z;
        transform.localScale = newSize;

        if (initListCards.Count == 0)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    public void SetupDeck(PlayerBase playerToSet)
    {
        player = playerToSet;
    }

    public float getCurrentTimeShowPlayer {
        get {
            return currentTimeToShowPlayer;
        }
    }
}
