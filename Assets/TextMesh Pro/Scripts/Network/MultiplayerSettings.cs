using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;
    public int maxPlayers;
    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if(MultiplayerSettings.multiplayerSettings == null)
        {
            MultiplayerSettings.multiplayerSettings = this;
        }
        else
        {
            if(MultiplayerSettings.multiplayerSettings != null)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
