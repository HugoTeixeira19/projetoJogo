using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   public void PlayScene()
    {
        SceneManager.LoadScene("");
    }
    public void CreditoScene()
    {
        SceneManager.LoadScene("");
    }
    public void TutorialScene()
    {
        SceneManager.LoadScene("");
    }
    public void RankingScene()
    {
        SceneManager.LoadScene("Scenes/RankingScene");
    }
    public void Sair()
    {
        Application.Quit();
    }
}
