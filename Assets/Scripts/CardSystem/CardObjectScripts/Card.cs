using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardTypes
{
    CHAMPION,
    SUPPORT
}

public class Card : MonoBehaviour
{
    public GameObject battleSystem;
    
    
    
    public int cardId;
    public CardTypes cardType;

    [SerializeField] public string cardName;
    [SerializeField] public string cardDescription;
    [SerializeField] private Sprite cardArtwork;
    [SerializeField] public int cardCost;

    public TMP_Text nameText;
    public Text descriptionText;
    public Image cardArtworkShower;
    public TMP_Text costText;

    public GameObject cardBackSide;

    public void Awake()
    {
        battleSystem = GameObject.Find("Battle System");
    }
    
    private void Start()
    {
        AssignValuesToCard();

        var dnd = GetComponent<DragDrop>();
        var cardTargeting = GetComponent<CardTargeting>();
        var boxCollider2D = GetComponent<BoxCollider2D>();
        if (gameObject.CompareTag("EnemyCard")) // things enemy cards dont need
        {
            dnd.enabled = false;
            cardTargeting.enabled = false;
            boxCollider2D.enabled = false;
            /*cardBackSide.SetActive(false);*/
            // this bad boy of a line will override everything to do with control of the cards backside (FOR TESTING)
        }
    }

    private void Update()
    {
        AssignValuesToCard();
    }

    private void AssignValuesToCard()
    {
        nameText.SetText(cardName);
        descriptionText.text = cardDescription;
        costText.SetText(cardCost.ToString());
        cardArtworkShower.sprite = cardArtwork;
        if (cardType == CardTypes.CHAMPION)
        {
            GetComponent<ChampionCard>().AssignChampionValues();
        }
    }

    public void SetBackSideFalse()
    {
        cardBackSide.SetActive(false);
    }

    public void SetBackSideTrue()
    {
        cardBackSide.SetActive(true);
    }

    public int GetValue()
    {
        if (GetComponent<Card>().cardType == CardTypes.SUPPORT)
        {
            return GetSupportCardValue();
        }

        if (GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            return GetChampCardValue();
        }

        return -100;
    }

    private int GetChampCardValue()
    {
        const int cardCostWeight = -1;
        const int cardHealthWeight = 1;
        const int cardPowerWeight = 2;

        return cardCost * cardCostWeight +
               GetComponent<ChampionCard>().cardHealth * cardHealthWeight +
               GetComponent<ChampionCard>().cardPower * cardPowerWeight;
    }

    private int GetSupportCardValue()
    {
        const int cardCostWeight = 1;
        const int cardHealthWeight = 2;
        const int cardPowerWeight = 1;

        if (GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT)
        {
            var amountOfCards = battleSystem.GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count;
            var timesFactor = 0;

            switch (amountOfCards)
            {
                case 0:
                    timesFactor = 0;
                    break;
                case 1:
                    timesFactor = 1;
                    break;
                case 2:
                    timesFactor = 2;
                    break;
                case 3:
                    timesFactor = 3;
                    break;
                default:
                    timesFactor = -1;
                    break;
            }

            // can be changed if needed
            return cardCost * cardCostWeight +
                   GetComponent<SupportCard>().healthModifierValue * cardHealthWeight +
                   GetComponent<SupportCard>().attackModifierValue * cardPowerWeight * timesFactor;
        }

        if (GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
        {
            var amountOfCards = battleSystem.GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count;
            int timesFactor;
            int roundFactor;

            switch (amountOfCards)
            {
                case 0:
                    timesFactor = 0;
                    roundFactor = 0;
                    break;
                case 1:
                    timesFactor = 1;
                    roundFactor = 1;
                    break;
                case 2:
                    timesFactor = 2;
                    roundFactor = 2;
                    break;
                case 3:
                    timesFactor = 3;
                    roundFactor = 3;
                    break;
                default:
                    timesFactor = -1;
                    roundFactor = -1;
                    break;
            }

            // can be changed if needed
            return cardCost * cardCostWeight +
                   GetComponent<SupportCard>().healthModifierValue * cardHealthWeight +
                   GetComponent<SupportCard>().attackModifierValue * cardPowerWeight * timesFactor * roundFactor +
                   5;
        }

        // can do something in targeting that picks the strongest card
        if (GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
        {
            // can be changed if needed
            return cardCost * cardCostWeight +
                   GetComponent<SupportCard>().healthModifierValue * cardHealthWeight +
                   GetComponent<SupportCard>().attackModifierValue * cardPowerWeight *
                   3; // times three to be generous
        }

        return -1; // This case sudden happen
    }
}