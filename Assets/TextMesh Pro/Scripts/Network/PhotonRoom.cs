using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room informação
    public static PhotonRoom room;
    private PhotonView PV;

    public int currentScene; // cena atual
    public int multiplayScene;

    //Player info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    /*Delayed start
        private bool readyToCount;
        private bool readyToStart;
        public float startingTime;
        private float lessThanMaxPlayers;
        private float atMaxPlayers;
        private float timeToStart;
    */


    private void Awake()
    {
        // set up singleton
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
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
    // Start is called before the first frame update
    void Start()
    {
        // setando privado variaveis
        PV = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Nós estamos agora em uma sala");
        /*
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        */
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        StartGame();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Um novo jogador entrou na sala");
    }

    void StartGame()
    {
        Debug.Log("Loading Level");
        PhotonNetwork.LoadLevel(multiplayScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == multiplayScene)
        {
            CreatePlayer();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " entrou no jogo");
    }
}
