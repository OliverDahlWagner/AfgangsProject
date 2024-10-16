using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    public GameObject avatarHalfCircle;
    
    public int maxHP;
    public int currentHP;

    public int currentMana;
    
    public Text maxHPHUD;
    public Text currentHPHUD;
    public Text currentManaHUD;

    public List<GameObject> attackEffects;

    public AudioClip hitSound;

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
        var settingVolumeScale = SettingsData.settingVolumeScale;
        GetComponent<AudioSource>().PlayOneShot(hitSound, (float) settingVolumeScale);

        for (int i = 0; i < attackEffects.Count; i++)
        {
            attackEffects[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
            attackEffects[i].SetActive(false);
        }

        yield return null;
    }
    
    
}
