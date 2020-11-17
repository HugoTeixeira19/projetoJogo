using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonControllerSingle : MonoBehaviour
{
    public static ButtonControllerSingle actionController;
    private PlayerBaseSingle playerCurrent;
    // variável auxiliar para cpu, definir qual condição de batalha deve escolher
    private bool isAttack;
    private string battle;


    public GameObject menuConfigure;
    public Sprite iconeSomMute;
    public Sprite iconeSom;
    public Slider barraVolume;

    private void Start()
    {
        actionController = this;
    }

    public void SetPlayer(PlayerBaseSingle playerSet)
    {
        playerCurrent = playerSet;
    }

    /* INICIO DO MENU PAUSE */
    public void onClickPause()
    {
        menuConfigure.SetActive(true);
        if (playerCurrent != null)
        {
            playerCurrent.ControleBotoes(false);
            playerCurrent.btnSemMana.GetComponent<Button>().interactable = false;
        }
    }

    public void onClickPlayAgain()
    {
        menuConfigure.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onClickMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void onClickMute()
    {
        SpriteRenderer imgIconeSom = GetComponent<SpriteRenderer>();
        imgIconeSom.sprite = iconeSomMute;
        barraVolume.value = 0f;
    }

    public void onClickClosePause()
    {
        menuConfigure.SetActive(false);
        if (playerCurrent != null)
        {
            playerCurrent.ControleBotoes(true);
            playerCurrent.btnSemMana.GetComponent<Button>().interactable = true;
        }
    }
    /* FIM DO MENU DE PAUSE */

    public void onClickAttack()
    {
        if (CardBase.selectedCard != null && playerCurrent.jogadaVez)
        {
            battle = "attack";
            JogarCard(playerCurrent, this.battle);
            isAttack = false;
        }
    }

    public void JogarCard(PlayerBaseSingle player, string battleCondition)
    {
        if (!player.CustoMana(CardBase.selectedCard.CostMana))
        {
            Debug.Log("Mana Insuficiente");
            playerCurrent.SemMana = true;
            playerCurrent.btnSemMana.GetComponent<Button>().interactable = true;
        }
        else
        {
            player.hand.playCard(player.canPlayerControl, CardBase.selectedCard);
            player.atualizarAtributos();
            CardBase.selectedCard.ConditionBattle = battleCondition;
            CardBase.selectedCard.GetComponentInChildren<Canvas>().enabled = false;
            CardBase.selectedCard = null;
            player.jogadaVez = false;
        }
    }

    public void onClickManaEmpty()
    {
        playerCurrent.SemMana = true;
        GameControllerSingle.instance.PlayerSemMana();
        playerCurrent.SemMana = false;
        playerCurrent.RestaurarMana();
        playerCurrent.atualizarAtributos();
        playerCurrent.ControleBotoes(false);
    }

    public void onClickDefense()
    {
        if (CardBase.selectedCard != null && playerCurrent.jogadaVez)
        {
            battle = "defense";
            playerCurrent.defesaJogador.SetActive(true);
            playerCurrent.defesaJogador.GetComponentInChildren<TextMeshProUGUI>().text = 
                CardBase.selectedCard.GetComponent<CardAttack>().getCurrentResistance.ToString();
            JogarCard(playerCurrent, this.battle);
            isAttack = true;
        }
    }

    public bool IsAttack
    {
        get
        {
            return isAttack;
        }
    }
}
