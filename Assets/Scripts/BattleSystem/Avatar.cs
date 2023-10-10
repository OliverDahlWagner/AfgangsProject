using System.Collections;
using System.Collections.Generic;
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
        currentHP -= damage;
        SetCurrentHP(currentHP);
    }
    
    
}
