using Photon.Pun;
using UnityEngine;

public abstract class CardBase : MonoBehaviour
{
    // Variáveis genéricas das cartas
    public string type;
    public int CostMana;

    [SerializeField]
    private string battle;

    public static CardBase selectedCard;

    public float dumbDragMovimentation;
    public Vector3 offsetZoomPosition;

    private Camera mainCamera, perspectiveCamera;
    private bool isDragging;
    private Vector3 startPosition;
    public Vector3 zoomPosition;
    private Vector3 positionToGo;
    private bool onHand;
    private bool canPlayerControl;
    private bool isFaceShowing = true;


    // Variáveis para setar as informações na carta
    public TextMesh textTypeCard;
    public TextMesh textCostManaCard;


    protected void initialize()
    {
        mainCamera = Camera.main;
        perspectiveCamera = Camera.allCameras[0];
        GetComponentInChildren<Canvas>().worldCamera = perspectiveCamera;
        //transform position, posição onde o objeto está renderizado

        // Setando as informações nas cartas
        textTypeCard.text = type;
        textCostManaCard.text = CostMana.ToString();
    }

    public void Update()
    {
        if (transform.position != positionToGo && onHand)
        {
            transform.position = Vector3.Lerp(transform.position, positionToGo, Time.deltaTime * dumbDragMovimentation);
        }
    }

    public void OnClick()
    {
        selectedCard = this;
    }

    public void OnMouseMover()
    {
        if (onHand && !isDragging && canPlayerControl)
        {
            positionToGo = zoomPosition;
        }
        Debug.Log("Estou com o mouse em cima: " + canPlayerControl);
    }
    public void OnMouseSair()
    {
        if(onHand && !isDragging && canPlayerControl)
        {
            positionToGo = startPosition;
        }
        Debug.Log("Tirei o mouse de cima");
    }
    
    public void flipCard()
    {
        isFaceShowing = !isFaceShowing;
        // pega a rotação de 0 a 360;
        Vector3 newRotation = transform.eulerAngles;
        if (isFaceShowing)
        {
            newRotation.z = -90;
        } else
        {
            newRotation.x = 90;
        }

        transform.eulerAngles = newRotation;
    }
    

    public void ToggleLayer()
    {
        int newLayer;
        if(gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            newLayer = LayerMask.NameToLayer("Default");
        } else
        {
            newLayer = LayerMask.NameToLayer("UI");
        }

        Transform[] transformsCard = GetComponentsInChildren<Transform>();

        foreach(Transform t in transformsCard)
        {
            t.gameObject.layer = newLayer;
        }
    }

    public void SetOnHand()
    {
        onHand = true;
    }

    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
        positionToGo = startPosition;
        Vector3 positionRelative = new Vector3(0, 3, -1.80f);
        zoomPosition = (startPosition + positionRelative);
    }

    public void SetStartPositionOffline(Vector3 position)
    {
        startPosition = position;
        positionToGo = startPosition;
        zoomPosition = startPosition + offsetZoomPosition;
    }

    public void SetOwer(bool canPlayerControl)
    {
        this.canPlayerControl = canPlayerControl;

        if (!canPlayerControl)
        {
            flipCard();
        }
    }

    public string ConditionBattle
    {
        get
        {
            return battle;
        }
        set
        {
            battle = value;
        }
    }

    
    public Vector3 SetandoPlayerZoomPosition(Vector3 position)
    {
        startPosition = position;
        positionToGo = startPosition;
        return (startPosition + offsetZoomPosition);
    }
}
