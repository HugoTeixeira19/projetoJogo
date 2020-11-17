using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); //Conecta ao servidor
            Debug.Log("Conectou");
        }

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Player has connected an master " + PhotonNetwork.CloudRegion);
    } 

    public void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) MultiplayerSettings.multiplayerSettings.maxPlayers};
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
        Debug.Log("Criou a sala com sucesso");
    }


    // Função ativa quando o jogador não encontra nenhuma sala
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("A criação da sala falhou");
        CreateRoom();
    }

    
    public void JoinRoom()
    {
        if(PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            TypedLobby typedLobby = new TypedLobby("Sala 1", LobbyType.Default);
            PhotonNetwork.JoinOrCreateRoom("Sala 1", roomOptions, typedLobby);
        }
        Debug.Log("We are now in a room");
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Button was click");
        PhotonNetwork.LeaveRoom(); // função sai da sala
    }

    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("A criação da sala falhou");
    }
    
}
