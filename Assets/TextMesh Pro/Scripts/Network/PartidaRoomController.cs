using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartidaRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int waitingRoomSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() // Redireciona para a sala de espera
    {
        SceneManager.LoadScene(waitingRoomSceneIndex);
    }
}
