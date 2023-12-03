using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoomHelperScript : MonoBehaviour
{
    [SerializeField] private GameObject theZoomCard;

    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;

    [SerializeField] private GameObject championStatShower;
    [SerializeField] private GameObject supportStatShower;

    [SerializeField] private TMP_Text championCost;
    [SerializeField] private TMP_Text championHealth;
    [SerializeField] private TMP_Text championPower;

    [SerializeField] private TMP_Text supportCost;

    [SerializeField] private GameObject supportTypeInstant;
    [SerializeField] private GameObject supportTypeSpecific;
    [SerializeField] private GameObject supportTypeLasting;

    [SerializeField] private GameObject supportPowerIcon;
    [SerializeField] private GameObject supportHealthIcon;

    [SerializeField] private GameObject supportPowerModifierTextContainer;
    [SerializeField] private GameObject supportHealthModifierTextContainer;
    [SerializeField] private Text supportPowerModifierText;
    [SerializeField] private Text supportHealthModifierText;

    [SerializeField] private GameObject lastingBrick;
    [SerializeField] private Text roundsLeft;


    public void SetZoomCardActive(GameObject card)
    {
        theZoomCard.SetActive(true);

        name.SetText(card.GetComponent<Card>().cardName);
        description.SetText(card.GetComponent<Card>().cardDescription);

        if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            championStatShower.SetActive(true);
            supportStatShower.SetActive(false);

            championCost.SetText(card.GetComponent<Card>().cardCost.ToString());
            championHealth.SetText(card.GetComponent<ChampionCard>().cardHealth.ToString());
            championPower.SetText(card.GetComponent<ChampionCard>().cardPower.ToString());
        }
        else
        {
            championStatShower.SetActive(false);
            supportStatShower.SetActive(true);

            supportCost.SetText(card.GetComponent<Card>().cardCost.ToString());
            
            // if instant
            if (card.GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT)
            {
                supportTypeInstant.SetActive(true);
            }
            
            // if specific
            if (card.GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
            {
                supportTypeSpecific.SetActive(true);
            }
            
            // if lasting
            if (card.GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
            {
                supportTypeLasting.SetActive(true);
                lastingBrick.SetActive(true);
                roundsLeft.text = $"{card.GetComponent<SupportCard>().roundCounter.ToString()}";
            }
            
            if (card.GetComponent<SupportCard>().supportModifierType == SupportModifierType.PLUS)
            {
                if (card.GetComponent<SupportCard>().attackModifierValue > 0)
                {
                    supportPowerModifierTextContainer.SetActive(true);
                    supportPowerIcon.SetActive(true);
                    supportPowerModifierText.text = $"+{card.GetComponent<SupportCard>().attackModifierValue}";
                }

                if (card.GetComponent<SupportCard>().healthModifierValue > 0)
                {
                    supportHealthModifierTextContainer.SetActive(true);
                    supportHealthIcon.SetActive(true);
                    supportHealthModifierText.text = $"+{card.GetComponent<SupportCard>().healthModifierValue}";
                }
            }

            if (card.GetComponent<SupportCard>().supportModifierType == SupportModifierType.MULTIPLY)
            {
                if (card.GetComponent<SupportCard>().attackModifierValue > 1)
                {
                    supportPowerIcon.SetActive(true);
                    supportPowerModifierTextContainer.SetActive(true);
                    supportPowerModifierText.text = $"x{card.GetComponent<SupportCard>().attackModifierValue}";
                }

                if (card.GetComponent<SupportCard>().healthModifierValue > 1)
                {
                    supportHealthIcon.SetActive(true);
                    supportHealthModifierTextContainer.SetActive(true);
                    supportHealthModifierText.text = $"x{card.GetComponent<SupportCard>().healthModifierValue}";
                }
            }
        }
        
    }

    public void ClearTheZoomCardArea()
    {
        theZoomCard.SetActive(false);
        
        supportPowerModifierTextContainer.SetActive(false);
        supportPowerIcon.SetActive(false);
        
        supportHealthModifierTextContainer.SetActive(false);
        supportHealthIcon.SetActive(false);
        
        supportTypeInstant.SetActive(false);
        supportTypeSpecific.SetActive(false);
        supportTypeLasting.SetActive(false);

        
    }

}


