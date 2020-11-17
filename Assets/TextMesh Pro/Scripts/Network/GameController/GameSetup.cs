using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviourPun
{
    public static GameSetup GS;
    public List<Transform> spawnPoints;
    public Camera mainCamera;
    public PlayerBase player1;
    public PlayerBase player2;
    public float timeRound;
    public int vezRodada;

    private bool result = false;
    private GameObject[] tempPlayers;
    private bool condition;
    private double time = 0;

    void Start()
    {
        PhotonNetwork.SendAllOutgoingCommands();
        GameSetup.GS.vezRodada = UnityEngine.Random.Range(1, 2);
        Debug.Log("Vez rodada: " + vezRodada);
    }

    void Update()
    {
        tempPlayers = GameObject.FindGameObjectsWithTag("Players");
        if(!result && tempPlayers.Length > 1)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                player1 = tempPlayers[0].GetComponent<PlayerBase>();
                player2 = tempPlayers[1].GetComponent<PlayerBase>();
            } else
            {
                player2 = tempPlayers[0].GetComponent<PlayerBase>();
                player1 = tempPlayers[1].GetComponent<PlayerBase>();
            }

            condition = (GameSetup.GS.player1 != null && GameSetup.GS.player2 != null);

            if (condition)
            {
                // setando players
                SetandoPlayer1();
                SetandoPlayer2();

                result = true;
                PhotonNetwork.IsMessageQueueRunning = true;
            }
        }
        if (condition)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                DistribuirCardsPlayers(GameSetup.GS.player1);
            }
            else
            {
                DistribuirCardsPlayers(player2);
            }

            /*
            if (GameSetup.GS.vezRodada == 1)
            {
                GameSetup.GS.player1.ControleBotoes(true);
                GameSetup.GS.player2.ControleBotoes(false);
            }
            else if (GameSetup.GS.vezRodada == 2)
            {
                GameSetup.GS.player2.ControleBotoes(true);
                GameSetup.GS.player1.ControleBotoes(false);
            }
            */
        }        
    }

    public void ControlarVez(PlayerBase player, bool entrada)
    {
        player.ControleBotoes(entrada);
    }

    public void DistribuirCardsPlayers(PlayerBase player)
    {
        if (player.ContadorI < 3 && player.SetInit && time >= 5)
        {
            if (player.canPlayerControl)
            {
                player.SetDeck.GetCard(player);
                player.ContadorI++;
                player.SetInit = false;
            }
        }
        else if (!(player.hand.getCards.Count == 3))
        {
            time += Time.deltaTime;
        }
    }

    private void SetandoPlayers(PlayerBase player, string nameDeck, Vector3 position)
    {
        player.SetDeck = GameObject.Find(nameDeck).GetComponentInChildren<DeckController>();
        player.SetDeck.setarRotacaoCard(position);
        player.SetDeck.SetupDeck(player);
    }

    [PunRPC]
    private void SetandoPlayer1()
    {
        SetandoPlayers(player1, "Deck P1", new Vector3(-90, 0, 180));
        Vector3 positionCardsTable = player1.positionCardsTable.transform.position;
        positionCardsTable.x = -0.26f;
        player1.positionCardsTable.transform.position = positionCardsTable;
    }

    [PunRPC]
    private void SetandoPlayer2()
    {
        SetandoPlayers(player2, "Deck P2", new Vector3(-90, 180, 180));
        Vector3 positionCardsTable = player2.positionCardsTable.transform.position;
        positionCardsTable.x = -0.38f;
        player2.positionCardsTable.transform.position = positionCardsTable;
    }

    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
            PhotonNetwork.AddCallbackTarget(this);
        }
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void DesativeCamera(PlayerBase player)
    {
        foreach (Camera cam in player.cameras)
        {
            cam.gameObject.SetActive(false);
        }
    }

    public bool getResult
    {
        get
        {
            return result;
        }
    }
}
