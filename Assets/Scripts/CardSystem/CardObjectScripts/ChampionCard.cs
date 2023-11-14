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

    public GameObject attack1;
    public GameObject attack2;

    public GameObject buff1;
    public GameObject buff2;

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

        if (cardHealth <= 0)
        {
            battleSystem.GetComponent<BattleSystem>().UpdatePLayerHand(this.GameObject());
            Destroy(gameObject);
        }
    }

    private IEnumerator TakeDamegeEffect(float audioLevel)
    {
        var settingVolumeScale = SettingsData.settingVolumeScale;
        
        GetComponent<AudioSource>().PlayOneShot(hitSound, (float)(audioLevel * settingVolumeScale));
        attack1.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        attack1.SetActive(false);
        attack2.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        attack2.SetActive(false);

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
        buff1.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        buff1.SetActive(false);
        buff2.SetActive(true);
        yield return new WaitForSeconds((float)0.5);
        buff2.SetActive(false);

        yield return null;
    }
}