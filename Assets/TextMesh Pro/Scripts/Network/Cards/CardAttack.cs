using UnityEngine;

public class CardAttack : CardBase
{
    public int attack;
    public int totalResistance;
    private int currentResistance;

    public TextMesh textAttackCard;
    public TextMesh textResistanceCard;

    void Start()
    {
        initialize();
        this.currentResistance = totalResistance;
        AtualizarAtributosCard();
    }

    public int getCurrentResistance
    {
        get
        {
            return currentResistance;
        }
        set
        {
            currentResistance = value;
        }
    }

    public void CustoDefesa(int valor)
    {
        this.currentResistance = this.currentResistance - valor;
        AtualizarAtributosCard();
    }

    public void AtualizarAtributosCard()
    {
        textAttackCard.text = attack.ToString();
        textResistanceCard.text = currentResistance.ToString();
    }
}
