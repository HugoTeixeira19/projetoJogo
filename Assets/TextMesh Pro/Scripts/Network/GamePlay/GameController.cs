using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerBase[] players;

    public Transform spawnPlayer1;
    public Transform spawnPlayer2;

    public int currentTurn = 1;

    public bool isPlayerOneStarting;

    public static GameController instance;

    void Start()
    {
        instance = this;
        int rand = Random.Range(1, 100);
        isPlayerOneStarting = rand > 50;
        GameSetup.GS.mainCamera.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("saiu do jogo");
        /*
         * Mudança de cena :
         * Application.LoadLevel("Nome_Cena");
        */
    }
}
