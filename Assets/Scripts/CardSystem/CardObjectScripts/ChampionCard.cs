using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public GameObject attack1;
    public GameObject attack2;
    
    public GameObject buff1;
    public GameObject buff2;

    
    private GameObject battleSystem;
    private void Start()
    {
        battleSystem = GameObject.Find("Battle System");
    }
    
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
        StartCoroutine(TakeDamegeEffect());
        
        if (cardHealth <= 0)
        {
            battleSystem.GetComponent<BattleSystem>().UpdatePLayerHand(this.GameObject());
            Destroy(gameObject);
        }
    }

    private IEnumerator TakeDamegeEffect()
    {
        attack1.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        attack1.SetActive(false);
        attack2.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        attack2.SetActive(false);
        
        yield return null;
    }
    
    public void PlayGetBuffEffect()
    {
        StartCoroutine(GetBuffEffect());
    }

    private IEnumerator GetBuffEffect()
    {
        buff1.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        buff1.SetActive(false);
        buff2.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        buff2.SetActive(false);
        
        yield return null;
    }
    
    
    
}