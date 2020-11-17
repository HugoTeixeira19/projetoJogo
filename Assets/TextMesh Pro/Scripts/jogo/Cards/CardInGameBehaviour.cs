using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInGameBehaviour : MonoBehaviour
{
    public Renderer meshEffect;
    public Renderer meshAttackIcon;
    public Renderer meshCard;
    public TextMesh lifeText;
    public TextMesh attackText;

    private int life;
    private int attack;
    private bool isEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        meshAttackIcon.enabled = true;
        meshEffect.enabled = false;
    }
    
    public void setCardGame(int attack, int life, Texture imgCard, bool isEffect)
    {
        this.attack = attack;
        this.life = life;
        this.isEffect = isEffect;
        meshCard.materials[1].mainTexture = imgCard;

        ApplyIsEffect();
        UpdateValues();
    }

    private void UpdateValues()
    {
        lifeText.text = life.ToString();
        attackText.text = attack.ToString();
    }

    public void ApplyIsEffect()
    {
        meshEffect.enabled = isEffect;
        meshAttackIcon.enabled = isEffect;
    }
}
