using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    public int currentMana;
    
    public Text maxHPHUD;
    public Text currentHPHUD;
    public Text currentManaHUD;

    public GameObject attack1;
    public GameObject attack2;

    public void SetHUD()
    {
        maxHPHUD.text = maxHP.ToString();
        currentHPHUD.text = currentHP.ToString();
        currentManaHUD.text = currentMana.ToString();
    }

    public void SetCurrentHP(int currentHP)
    {
        currentHPHUD.text = currentHP.ToString();
    }
    
    public void SetCurrentMana(int currentMana)
    {
        currentManaHUD.text = currentMana.ToString();
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(TakeDamegeEffect());
        currentHP -= damage;
        SetCurrentHP(currentHP);
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
    
    
}
