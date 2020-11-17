using System.Collections.Generic;
using UnityEngine;

public class DeckControllerSingle : MonoBehaviour
{
    private Vector3 initialSize;
    private int totalInitialCards;
    public List<CardBase> initListCards;
    private PlayerBaseSingle player;

    private GameObject tempCard;

    //animation variables
    public float timeToShowPlayer;
    public float dumbGetCard;
    private float currentTimeToShowPlayer;
    private bool moveToHand;
    private Vector3 positionShowPlayer;
    private Vector3 positionHand;
    private Vector3 targetPosition;

    private bool canPlayerControl;

    // Start is called before the first frame update
    void Start()
    {
        initialSize = transform.localScale;
        totalInitialCards = initListCards.Count;
        positionShowPlayer = player.hand.positionToShowPlayer.position;

        canPlayerControl = player.canPlayerControl;
    }

    // Update is called once per frame
    void Update()
    {
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
                tempCardComponent.SetOnHand();
                tempCardComponent.SetStartPositionOffline(positionHand);
                tempCard = null;
                player.SetInit = true;
            }
        }
    }

    public void GetCard(HandPlayerSingle hand)
    {
        if (initListCards.Count > 0)
        {
            int randCardIndex = UnityEngine.Random.Range(0, initListCards.Count);
            CardBase selectedCard = initListCards[randCardIndex];
            initListCards.RemoveAt(randCardIndex);


            tempCard = (Instantiate(selectedCard.gameObject, transform.position, selectedCard.transform.rotation)) as GameObject;
            ResizeDeck();

            moveToHand = true;
            targetPosition = positionShowPlayer;
            currentTimeToShowPlayer = 0;

            player.hand.AddCard(tempCard.GetComponent<CardBase>(), canPlayerControl);
        }
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

    public void SetupDeck(PlayerBaseSingle playerToSet)
    {
        player = playerToSet;
    }

    public float getCurrentTimeShowPlayer {
        get {
            return currentTimeToShowPlayer;
        }
    }
}
