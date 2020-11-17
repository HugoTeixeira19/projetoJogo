using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public PhotonView myPhotonView;
    public static WaitingRoomController waitingRoom;

    // Contagem dos players
    private int playerCount;
    private int roomSize;
    [SerializeField]
    private int minPlayerToStart;

    private bool playerEntrance = false;

    // Textos de contagem de players e timer para início de jogo
    [SerializeField]
    private Text numeroPlayers;
    [SerializeField]
    private Text contagemInicial;

    // Valores booleanos definindo quando o temporizador pode iniciar
    private bool prontoParaContagem;
    private bool prontoParaComecar;
    private bool iniciandoJogo;

    // Variáveis de contagem regressiva
    private float temporizadorParaIniciarJogo;
    private float temporizadorNaoCompleto;
    private float temporizadorCompleto;

    // Reset das variáveis
    [SerializeField]
    private float tempoMaximoEspera;
    [SerializeField]
    private float tempoMaximoCompleto;

    public int currentScene;

    //Player info
    public Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;
    public int playersInGame;
    public GameObject prefabPlayer;


    // Start is called before the first frame update
    private void Start()
    {
        // Inicializando variáveis
        temporizadorCompleto = tempoMaximoCompleto;
        temporizadorNaoCompleto = tempoMaximoEspera;
        temporizadorParaIniciarJogo = tempoMaximoEspera;

        PlayerContagemAtualizar();
    }

    private void Awake()
    {
        // set up singleton
        if (WaitingRoomController.waitingRoom == null)
        {
            WaitingRoomController.waitingRoom = this;
        }
        else
        {
            if (WaitingRoomController.waitingRoom != this)
            {
                Destroy(WaitingRoomController.waitingRoom.gameObject);
                WaitingRoomController.waitingRoom = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);

        myPhotonView = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void PlayerContagemAtualizar()
    {
        // Atualiza a quantidade de players, quando um novo player entra na sala
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        numeroPlayers.text = playerCount + " : " + roomSize;

        if(playerCount == roomSize)
        {
            prontoParaComecar = true;
        }
        else if(playerCount >= minPlayerToStart)
        {
            prontoParaContagem = true;
        }
        else
        {
            prontoParaContagem = false;
            prontoParaComecar = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        PlayerContagemAtualizar();
        if(PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, temporizadorParaIniciarJogo);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        temporizadorParaIniciarJogo = timeIn;
        temporizadorNaoCompleto = timeIn;
        if(timeIn < temporizadorCompleto)
        {
            temporizadorCompleto = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerContagemAtualizar();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!iniciandoJogo)
        {
            WaitingForMorePlayers();
        }
    }

    void WaitingForMorePlayers()
    {
        // troca de playerCount <= 1 por playerCount == 0
        if(playerCount == 0)
        {
            ResetarTemporizador();
        }
        if(prontoParaComecar)
        {
            temporizadorCompleto -= Time.deltaTime;
            temporizadorParaIniciarJogo = temporizadorCompleto;
        }
        else if(prontoParaContagem)
        {
            temporizadorNaoCompleto -= Time.deltaTime;
            temporizadorParaIniciarJogo = temporizadorNaoCompleto;
        }

        // Formatar string do timer como cronometro
        string tempTimer = string.Format("{0:00}", temporizadorParaIniciarJogo);
        contagemInicial.text = tempTimer;

        if(temporizadorParaIniciarJogo <= 0f)
        {
            if(iniciandoJogo)
            {
                return;
            }
            OnJoinedRoom();
        }
    }

    void ResetarTemporizador()
    {
        temporizadorParaIniciarJogo = tempoMaximoEspera;
        temporizadorNaoCompleto = tempoMaximoEspera;
        temporizadorCompleto = tempoMaximoCompleto;
    }

    public override void OnJoinedRoom()
    {
        iniciandoJogo = true;
        Debug.Log("We are now in a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        //if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
        //{
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
        //}
    }

    public void Cancelar()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            myPhotonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);

            //RPC_CreatePlayer();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
            myPhotonView.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPC_CreatePlayer()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Scenes/Lobby Room");
            return;
        }

        GameObject player;
        Debug.Log("Instantiate an player now");
        if (PlayerManager.LocalPlayerInstance == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Entrou aqui");
                player = PhotonNetwork.Instantiate(prefabPlayer.name,
                    GameSetup.GS.spawnPoints[0].position, GameSetup.GS.spawnPoints[0].rotation, 0);

                player.GetComponent<PlayerBase>().SetDeck = GameObject.Find("Deck P1").GetComponentInChildren<DeckController>();
                player.transform.position = Vector3.zero;
            }
            else
            {
                Debug.Log("Player 2 entrou na sala");
                player = PhotonNetwork.Instantiate(prefabPlayer.name,
                    GameSetup.GS.spawnPoints[1].position, GameSetup.GS.spawnPoints[1].rotation, 0);

                // Ajustando a rotação do prefab
                Vector3 newRotation = Vector3.zero;
                newRotation.y = 180;
                player.transform.eulerAngles = newRotation;

                player.GetComponent<PlayerBase>().SetDeck = GameObject.Find("Deck P2").GetComponentInChildren<DeckController>();
            }
        }
    }
}