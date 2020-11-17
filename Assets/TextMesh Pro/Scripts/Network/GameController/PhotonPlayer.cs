using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            int positionSpawn;
            Debug.Log("Instantiate an player now");
            if (WaitingRoomController.waitingRoom.photonPlayers[0].IsLocal)
            {
                positionSpawn = 0;
                Debug.Log("Entrou aqui");
                myAvatar = PhotonNetwork.Instantiate("PlayerOnline",
                    GameSetup.GS.spawnPoints[positionSpawn].position, GameSetup.GS.spawnPoints[positionSpawn].rotation, 0);

                myAvatar.GetComponent<PlayerBase>().SetPhotonView(PV);
                //GameSetup.GS.player1 = myAvatar.GetComponent<PlayerBase>();
            } else
            {
                positionSpawn = 1;
                Debug.Log("Player 2 entrou na sala");
                myAvatar = PhotonNetwork.Instantiate("PlayerOnline",
                    GameSetup.GS.spawnPoints[positionSpawn].position, GameSetup.GS.spawnPoints[positionSpawn].rotation, 0);

                Vector3 newRotation = Vector3.zero;
                newRotation.y = 180;
                myAvatar.transform.eulerAngles = newRotation;
                myAvatar.GetComponent<PlayerBase>().SetPhotonView(PV);
                //GameSetup.GS.player2 = myAvatar.GetComponent<PlayerBase>();
            }
            
            /*
            player.deck = GameObject.Find("Deck P2").GetComponentInChildren<DeckController>();
            GameSetup.GS.photonPlayers.Add(this);
            */
        }

        //GameSetup.GS.mainCamera.gameObject.SetActive(false);
    }
}
