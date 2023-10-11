using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionCard : MonoBehaviour
{
    [SerializeField] public int cardPower;
    [SerializeField] public int cardHealth;

    public Text powerText;
    public Text healthText;

    public bool hasAttacked = false;
    public bool hasBeenPlaced = false;
    public bool isOnBoard = false;

    public void AssignChampionValues()
    {
        powerText.text = cardPower.ToString();
        healthText.text = cardHealth.ToString();
    }
    
    [ContextMenu("Take Damage Test")]
    public void TakeSomeDamage()
    {
        cardHealth -= 1; // Reduce the health of the specific card instance. now it works

        if (cardHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        cardHealth -= damageAmount;

        if (cardHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}