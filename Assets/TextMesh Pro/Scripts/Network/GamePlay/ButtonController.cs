using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private PlayerBase playerCurrent;
    private string battle;

    public void SetPlayer(PlayerBase playerSet)
    {
        playerCurrent = playerSet;
    }

    public void onClickAttack()
    {
        if (CardBase.selectedCard != null)
        {
            playerCurrent.hand.playCard(playerCurrent.canPlayerControl, CardBase.selectedCard);
            battle = "attack";
            playerCurrent.hand.ReorganizeCards();

            // Passando a vez para o outro jogador
            playerCurrent.btnAttack.MovingNextPlayer();
        }
    }

    public void MovingNextPlayer()
    {
        if (GameSetup.GS.vezRodada == 1)
        {
            GameSetup.GS.vezRodada = 2;
        }
        else if (GameSetup.GS.vezRodada == 2)
        {
            GameSetup.GS.vezRodada = 1;
        }
    }

    public void onClickDefense()
    {
        if (CardBase.selectedCard != null)
        {
            playerCurrent.hand.playCard(playerCurrent.canPlayerControl, CardBase.selectedCard);
            battle = "defense";
            playerCurrent.hand.ReorganizeCards();
            playerCurrent.btnDefense.MovingNextPlayer();
        }
    }


}
