using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Computer : MonoBehaviour
{
    // Instancia dos players
    private PlayerBaseSingle player;
    private PlayerBaseSingle cpu;

    // variável auxiliar para decisão de batalha
    private bool typeBattleCurrent;

    public GameObject defesaCpu;

    // construtor inicializando os players e a variável auxiliar
    public Computer(PlayerBaseSingle player, PlayerBaseSingle cpu)
    {
        this.player = player;
        this.cpu = cpu;
        typeBattleCurrent = false;
    }

    public void Start()
    {
        defesaCpu = player.defesaOponente;
    }

    // Primeira resposta da cpu na escolha das cartas, utilizando escolha randômica
    public bool cpuResposta()
    {
        int random = UnityEngine.Random.Range(0, cpu.hand.Cards.Count);

        // Pega a carta selecionada
        CardBase.selectedCard = cpu.hand.Cards[random];

        // verifica se não é nula e jogar na mesa
        return JogandoCardNaMesa(0);
    }

    /* Segunda opção da cpu de escolha das cartas está consiste
     * pegar a carta com o maior atributo, serve tanto para ataque
     * quanto para defesa.
     * Recebe um parâmetro chamado estilo, responsável por escolher entre ataque e defesa.
    */
    public bool cpuRespostaMedium(int estilo)
    {
        int atributoAlto = 0;
        int valorAux = 0;
        CardAttack cardAux;
        if (ButtonControllerSingle.actionController.IsAttack)
        {
            for (int i = 0; i < cpu.hand.Cards.Count; i++)
            {
                cardAux = cpu.hand.Cards[i].GetComponent<CardAttack>();
                if(cardAux.attack > atributoAlto)
                {
                    atributoAlto = cardAux.attack;
                    valorAux = i;
                }
            }
            CardBase.selectedCard = cpu.hand.Cards[valorAux];
        } else
        {
            for(int i = 0; i < cpu.hand.Cards.Count; i++)
            {
                cardAux = cpu.hand.Cards[i].GetComponent<CardAttack>();
                if(cardAux.totalResistance > atributoAlto)
                {
                    atributoAlto = cardAux.totalResistance;
                    valorAux = i;
                }
            }
            CardBase.selectedCard = cpu.hand.Cards[valorAux];
        }
        
        // verifica se a carta selecionada não é nula, jogar na mesa e retorna true
        return JogandoCardNaMesa(estilo);
    }

    /*
     * Terceira opção da cpu de escolha de cartas, equivalente a anterior o que muda
     * é o acréscimo de mana.
     * Recebe um parâmetro chamado estilo, responsável por escolher entre ataque e defesa
     * */
    public bool cpuRespostaHard(int estilo)
    {   
        // soma um de mana
        cpu.CustoMana(-3);

        /* executa a resposta da cpu e retorna verdadeiro se
           estiver selecionado uma carta */
        if (cpuRespostaMedium(estilo))
        {
            return true;
        }
        return false;
    }

    /* Método que verifica se a carta selecionada é nula.
     * Joga a carta na mesa
     * */
    private bool JogandoCardNaMesa(int estilo)
    {
        if (CardBase.selectedCard != null)
        {
            /* Estilo de batalha selecionado de acordo com o valor passado no
             * parâmetro.
             * */
            string battle = SelectBattle(estilo);
            Debug.Log("Estilo de batalha escolhido cpu: " + battle);
            Vector3 newRotation = Vector3.zero;
            newRotation.x = -90;
            CardBase.selectedCard.transform.eulerAngles = newRotation;

            if (battle == "defense")
            {
                defesaCpu.SetActive(true);
                defesaCpu.GetComponentInChildren<TextMeshProUGUI>().text =
                    CardBase.selectedCard.GetComponent<CardAttack>().getCurrentResistance.ToString();
            }

            // Joga a carta na mesa
            ButtonControllerSingle.actionController.JogarCard(cpu, battle);
            

            return true;
        }
        return false;
    }

    /* Com o parâmetro de estilo será escolhido a forma
     * que a cpu se comportará a respeito da escolha das jogadas.
     * Por exemplo, 0 - é randômico. Retornando a string attack ou defense
     * */
    private string SelectBattle(int estilo)
    {
        int random = UnityEngine.Random.Range(0, 2);
        string battle = null;
        switch (estilo)
        {
            case 0:
                battle = (random == 0) ? "attack" : "defense";
            break;

            case 1:
                Debug.Log("Valor anterior: " + typeBattleCurrent);
                battle = (typeBattleCurrent) ? "attack" : "defense";
                typeBattleCurrent = !typeBattleCurrent;
                Debug.Log("Valor após: " + typeBattleCurrent);
            break;

            case 2:
                foreach(CardAttack card in player.table.CardNaMesa)
                {
                    if(card.attack <= 4 && card.ConditionBattle == "attack")
                    {
                        Debug.Log("Encontrou o attack");
                        battle = "attack";
                    } else if((card.getCurrentResistance >= 6 || card.getCurrentResistance <= 4) && card.ConditionBattle == "defense")
                    {
                        battle = "attack";
                        Debug.Log("Encontrou o attack 2");
                    } else
                    {
                        battle = "defense";
                        Debug.Log("Encontrou o defesa");
                    }
                }
            break;

            case 3:
                battle = (ButtonControllerSingle.actionController.IsAttack) ? "attack" : "defense";
            break;
        }
        return battle;
    }
}
