using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChampionCard : MonoBehaviour
{
    [SerializeField] public int cardPower;
    [SerializeField] public int cardHealth;

    public TMP_Text powerText;
    public TMP_Text healthText;

    public bool hasAttacked = false;
    public bool hasBeenPlaced = false;
    public bool isOnBoard = false;

    public List<GameObject> attackEffects;
    public List<GameObject> buffEffects;

    public AudioClip hitSound;
    public AudioClip buffSound;


    private GameObject battleSystem;

    private void Start()
    {
        battleSystem = GameObject.Find("Battle System");
    }

    public void AssignChampionValues()
    {
        powerText.SetText(cardPower.ToString());
        healthText.SetText(cardHealth.ToString());
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
        StartCoroutine(TakeDamegeEffect(1));
    }

    private IEnumerator TakeDamegeEffect(float audioLevel)
    {
        var settingVolumeScale = SettingsData.settingVolumeScale;
        
        GetComponent<AudioSource>().PlayOneShot(hitSound, (float)(audioLevel * settingVolumeScale));

        for (int i = 0; i < attackEffects.Count; i++)
        {
            attackEffects[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
            attackEffects[i].SetActive(false);
        }

        
        if (cardHealth <= 0)
        {
            battleSystem.GetComponent<BattleSystem>().UpdatePLayerHand(this.GameObject());
            Destroy(gameObject);
        }

        yield return null;

    }

    public void PlayGetBuffEffect(float audioLevel)
    {
        StartCoroutine(GetBuffEffect(audioLevel));
    }

    private IEnumerator GetBuffEffect(float audioLevel)
    {
        var settingVolumeScale = SettingsData.settingVolumeScale;

        Debug.Log("Audio level " + (float)(audioLevel * settingVolumeScale));
        GetComponent<AudioSource>().PlayOneShot(buffSound, (float)(audioLevel * settingVolumeScale));
        
        for(int i=0; i<buffEffects.Count; i++)
        {
            buffEffects[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
            buffEffects[i].SetActive(false);
        }

        yield return null;
    }
}