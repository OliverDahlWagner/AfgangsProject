using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SupCardTypes
{
    INSTANT,
    LASTING,
    SPECIFIC
}

public enum SupportModifierType
{
    PLUS,
    MULTIPLY
}

public class SupportCard : MonoBehaviour
{
    public SupCardTypes supCardType;
    public int roundCounter;

    public GameObject lastingTypeBorder;
    public TMP_Text roundsLeftText;

    public SupportModifierType supportModifierType;
    public int attackModifierValue;
    public int healthModifierValue;

    private List<SupCardTypes> listOfSupCardTypesList;

    [SerializedDictionary("Support card type", "Icon object")]
    public SerializedDictionary<SupCardTypes, GameObject> supportIcons;


    [SerializeField] private GameObject attackBuffIcon;
    [SerializeField] private GameObject healthBuffIcon;
    [SerializeField] private Text attackBuffText;
    [SerializeField] private Text healthBuffText;
    [SerializeField] private GameObject attackBuffTextContainer;
    [SerializeField] private GameObject healthBuffTextContainer;

    private GameObject battleSystem;

    private void Start()
    {
        battleSystem = GameObject.Find("Battle System");

        AssignSupportIcons();

        if (supCardType == SupCardTypes.LASTING)
        {
            roundsLeftText.SetText(roundCounter.ToString());
            lastingTypeBorder.SetActive(true);
        }

        AssignRoundsLeftValues();
    }

    public void AssignRoundsLeftValues()
    {
        roundsLeftText.text = roundCounter.ToString();
    }

    private void AssignSupportIcons()
    {
        supportIcons[supCardType].SetActive(true);

        if (supportModifierType == SupportModifierType.PLUS)
        {
            if (attackModifierValue > 0)
            {
                attackBuffIcon.SetActive(true);
                attackBuffTextContainer.SetActive(true);
                attackBuffText.text = $"+{attackModifierValue}";
            }
            if (healthModifierValue > 0)
            {
                healthBuffIcon.SetActive(true);
                healthBuffTextContainer.SetActive(true);
                healthBuffText.text = $"+{healthModifierValue}";
            }
        }

        if (supportModifierType == SupportModifierType.MULTIPLY)
        {
            if (attackModifierValue > 1)
            {
                attackBuffIcon.SetActive(true);
                attackBuffTextContainer.SetActive(true);
                attackBuffText.text = $"x{attackModifierValue}";
            }
            if (healthModifierValue > 1)
            {
                healthBuffIcon.SetActive(true);
                healthBuffTextContainer.SetActive(true);
                healthBuffText.text = $"x{healthModifierValue}";
            }
        }
    }

    private void LastingSupCardHelperFunction()
    {
        if (roundCounter == 0)
        {
            if (battleSystem.GetComponent<BattleSystem>().state == BattleState.ENEMYTURN)
            {
                Destroy(gameObject);
            }

            if (battleSystem.GetComponent<BattleSystem>().state == BattleState.PLAYERTURN)
            {
                battleSystem.GetComponent<BattleSystem>().UpdatePLayerHand(this.GameObject());
                Destroy(gameObject);
            }
        }
    }

    private float
        GetAudioLevel(
            List<GameObject> playedCards) // needed if type instant or lasting (in order to get the right audio level)
    {
        var cardCount = 0;

        for (int i = 0; i < playedCards.Count; i++)
        {
            var card = playedCards[i];
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                cardCount++;
            }
        }

        var audioScale = 1.0f / cardCount;

        return audioScale;
    }


    ///////////////////////////////////
    /// ///////////////////////////////////
    /// ///////////////////////////////////   all support happens under here
    public void SupportFunction(GameObject theCard, List<GameObject> listOfPlayedCards)
    {
        if (supCardType == SupCardTypes.INSTANT)
        {
            InstantSupportCardHelperFunction(listOfPlayedCards);
        }

        if (supCardType == SupCardTypes.LASTING)
        {
            LastingSupportCardHelperFunction(listOfPlayedCards);
        }

        if (supCardType == SupCardTypes.SPECIFIC)
        {
            SpecificSupportCardHelperFunction(theCard);
        }
    }

    private void InstantSupportCardHelperFunction(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                var championCard = card.GetComponent<ChampionCard>();

                if (supportModifierType == SupportModifierType.PLUS)
                {
                    championCard.cardHealth += healthModifierValue;
                    championCard.cardPower += attackModifierValue;
                }
                else if (supportModifierType == SupportModifierType.MULTIPLY)
                {
                    championCard.cardHealth *= healthModifierValue;
                    championCard.cardPower *= attackModifierValue;
                }

                championCard.AssignChampionValues();
                championCard.PlayGetBuffEffect(GetAudioLevel(listOfPlayedCards));
            }
        }
    }

    private void LastingSupportCardHelperFunction(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                var championCard = card.GetComponent<ChampionCard>();

                if (supportModifierType == SupportModifierType.PLUS)
                {
                    championCard.cardHealth += healthModifierValue;
                    championCard.cardPower += attackModifierValue;
                }
                else if (supportModifierType == SupportModifierType.MULTIPLY)
                {
                    championCard.cardHealth *= healthModifierValue;
                    championCard.cardPower *= attackModifierValue;
                }

                championCard.AssignChampionValues();
                championCard.PlayGetBuffEffect(GetAudioLevel(listOfPlayedCards));
            }
        }

        roundCounter--;
        AssignRoundsLeftValues();
        LastingSupCardHelperFunction();
    }

    private void SpecificSupportCardHelperFunction(GameObject theCard)
    {
        if (theCard.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            var championCard = theCard.GetComponent<ChampionCard>();
            
            if (supportModifierType == SupportModifierType.PLUS)
            {
                championCard.cardHealth += healthModifierValue;
                championCard.cardPower += attackModifierValue;
            }
            else if (supportModifierType == SupportModifierType.MULTIPLY)
            {
                championCard.cardHealth *= healthModifierValue;
                championCard.cardPower *= attackModifierValue;
            }

            championCard.AssignChampionValues();
            championCard.PlayGetBuffEffect(1);
        }
    }
}