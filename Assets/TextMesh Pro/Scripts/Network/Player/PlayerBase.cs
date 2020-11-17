using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviourPun
{
    public bool canPlayerControl;
    public CardsInGameController table;
    public Camera[] cameras = new Camera[2];
    public GameObject painel;

    public PhotonView view;

    [SerializeField]
    private DeckController deck;
    public HandPlayer hand;
    public ButtonController btnAttack;
    public ButtonController btnDefense;
    public ButtonController btnSemMana;
    public int totalLife;
    public int totalMana;
    public GameObject positionCardsTable;
    public int pontos;


    private int currentMana;
    private int currentLife;
    private bool semMana;
    public TextMeshProUGUI textLife;
    public TextMeshProUGUI textMana;
    private bool init = false;
    private int i;
    private double time = 0;
    public int vezJogar;

    void Start()
    {
        cameras = GetComponentsInChildren<Camera>();

        view = GetComponent<PhotonView>();

        GameSetup.GS.mainCamera.gameObject.SetActive(false);

        DesativarComponentes(true);

        this.currentLife = totalLife;
        this.currentMana = totalMana;
        hand.SetPlayer(this);
        btnAttack.SetPlayer(this);
        btnSemMana.SetPlayer(this);
        btnDefense.SetPlayer(this);

        canPlayerControl = view.IsMine;

        btnSemMana.GetComponent<Button>().interactable = false;
        ControleBotoes(false);

        semMana = false;

        init = true;
        i = 0;

        if (!view.IsMine && PhotonNetwork.IsConnected)
        {
            DesativarComponentes(false);
        }

        atualizarAtributos();
        pontos = 0;
    }

    void Update()
    {        
        if(!semMana)
        {
            btnSemMana.GetComponent<Button>().interactable = false;
        }

        if(PhotonNetwork.IsMasterClient && GameSetup.GS.vezRodada == 1)
        {
            this.ControleBotoes(true);
        } else
        {
            this.ControleBotoes(false);
            if(GameSetup.GS.vezRodada == 2)
            {
                this.ControleBotoes(true);
            }
        }
    }

    public void atualizarAtributos()
    {
        if(view.IsMine)
        {
            textLife.SetText(currentLife.ToString());
            textMana.SetText(currentMana.ToString());
        }
    }

    public void ControleBotoes(bool entrada)
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length - 1; i++)
        {
            buttons[i].interactable = entrada;
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

    public int ContadorI
    {
        get
        {
            return i;
        }
        set
        {
            i = value;
        }
    }

    public void SetPhotonView(PhotonView myPhotonView)
    {
        this.view = myPhotonView;
    }

    private void DesativarComponentes(bool entrada)
    {
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(entrada);
        }

        painel.SetActive(entrada);
    }

    public DeckController SetDeck
    {
        set
        {
            this.deck = value;
        }
        get
        {
            return this.deck;
        }
    }
}
