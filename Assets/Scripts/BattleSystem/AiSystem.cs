using System.Collections;
using UnityEngine;

public class AiSystem : MonoBehaviour
{
    public GameObject battleSystem;
    public GameObject enemyDropZone; // important for placement

    public void Awake()
    {
        battleSystem = GameObject.Find("Battle System");
    }

    public IEnumerator playCards() // this is the function enemyturn uses
    {
        GetComponent<BattleSystem>().IncreaseMana( GetComponent<BattleSystem>().enemyAva);
        GetComponent<AiBasicFunctions>().LastingSupCardUseFunction();

        if (GetComponent<DrawCards>().enemyHand.Count < 7)
        {
            GetComponent<AiBasicFunctions>().DrawOneCard();
        }
        
        yield return new WaitForSeconds(0.5f); // for the feel
        GetComponent<AiBasicFunctions>().ResetChampCardActions();

        yield return StartCoroutine(HandleAiTurn());
    }

    private IEnumerator HandleAiTurn()
    {
        yield return StartCoroutine(GetComponent<AiCardPlacementHandler>().HandleCardPlacement());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GetComponent<AiCardAttackHandler>().ChooseCardForAttack());
    }












}