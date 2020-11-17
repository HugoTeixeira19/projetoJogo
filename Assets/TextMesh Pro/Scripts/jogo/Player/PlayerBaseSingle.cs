using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBaseSingle : MonoBehaviour
{ 
    // variável de controle para o player, impedindo o controle do segundo player
    public bool canPlayerControl;
    public bool jogadaVez;

    // variável de controle das cartas na mesa
    public CardsInGameControllerSingle table;

    // definindo os componentes do player
    public DeckControllerSingle deck;
    public HandPlayerSingle hand;
    public ButtonControllerSingle btnAttack;
    public ButtonControllerSingle btnDefense;

    public ButtonControllerSingle btnSemMana;


    public int totalLife;
    public int totalMana;
    public int pontos;


    public int currentMana;
    [SerializeField]
    private int currentLife;
    private bool semMana;

    public TextMeshProUGUI textLife;
    public TextMeshProUGUI textMana;
    public GameObject defesaJogador;
    public GameObject defesaOponente;

    private bool init = false;
    private int i;

    void Start()
    {
        // inicializando os componentes do player
        this.currentLife = totalLife;
        this.currentMana = totalMana;
        deck.SetupDeck(this);
        hand.SetPlayer(this);
        btnAttack.SetPlayer(this);
        btnDefense.SetPlayer(this);
        table.SetPlayer(this);

        ControleBotoes(false);
        btnSemMana.SetPlayer(this);

        btnSemMana.GetComponent<Button>().interactable = false;

        semMana = false;

        //btnSemMana.interactable = false;

        init = true;
        i = 0;

        // inicializa os valores da vida e mana
        atualizarAtributos();
    }

    // Update is called once per frame
    void Update()
    {
        // distribuir os cards ao player, máximo de três cards
        if (init && i < 3)
        {
            deck.GetCard(hand);
            init = false;
            i++;
        }
        if (i == 3)
            ControleBotoes(true);

        if(!this.SemMana)
        {
            btnSemMana.GetComponent<Button>().interactable = false;
        }

        if(currentLife < 0)
        {
            currentLife = 0;
            atualizarAtributos();
        }
    }

    // Responsável pelo controle de interação com os botões
    public void ControleBotoes(bool entrada)
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length - 1; i++) {
            buttons[i].interactable = entrada;
        }
    }

    // Atualiza os valores da vida e mana do player
    public void atualizarAtributos()
    {
        if (this.canPlayerControl)
        {
            textLife.SetText(currentLife.ToString());
            textMana.SetText(currentMana.ToString());
        }
    }

    public bool SetInit
    {
        set
        {
            init = value;
        }
        get
        {
            return init;
        }
    }

    public bool CustoMana(int valor)
    {
        if(GastoMana(valor))
        {
            this.currentMana -= valor;
            return true;
        }
        return false;
    }

    private bool GastoMana(int valor)
    {
        if ((this.currentMana - valor) >= 0)
        {
            return true;
        }
        return false;
    }

    public void CustoLife(int valor)
    {
        this.currentLife -= valor;
    }

    public int getLife
    {
        get
        {
            return currentLife;
        }
    }

    public int getMana
    {
        get
        {
            return currentMana;
        }
    }

    public bool SemMana
    {
        get
        {
            return semMana;
        }
        set
        {
            semMana = value;
        }
    }

    public void ZerarCountCards()
    {
        this.i = 0;
        table.CardNaMesa.Clear();
    }

    public void RestaurarMana()
    {
        this.currentMana = totalMana;
    }
}
