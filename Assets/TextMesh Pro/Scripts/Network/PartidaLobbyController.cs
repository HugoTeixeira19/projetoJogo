using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartidaLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject StartButton; // botão usado para criar e entrar no jogo
    [SerializeField]
    private GameObject CancelButton; // botão usado para pausar a procura por um jogo
    [SerializeField]
    private int RoomSize; // Seta o numero de players que estão na sala


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        StartButton.SetActive(true);
    }

    public void Iniciar()
    {
        StartButton.SetActive(false);
        CancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Partida aleatória");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // Caso não houver nenhuma sala disponível
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom() // Criar uma nova sala
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Falha ao tentar criar uma sala... Tentar novamente");
        CreateRoom(); // Tenta criar uma nova sala com nome diferente
    }

    public void Cancelar()
    {
        CancelButton.SetActive(false);
        StartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
