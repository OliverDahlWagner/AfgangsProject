using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{

    public GameObject Card1;
    public GameObject Card2;

    public GameObject PlayerArea;
    public GameObject EnemyArea;

    private List<GameObject> cards = new();
    
    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    public void OnClick()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0,0,0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false); // sets the parent to the PlayerArea transform. So it will show
            playerCard.tag = "PlayerZone";
            playerCard.name = "PlayerCard " + (i+1);
            
            GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0,0,0), Quaternion.identity);
            enemyCard.transform.SetParent(EnemyArea.transform, false); // sets the parent to the PlayerArea transform. So it will show
            enemyCard.tag = "EnemyZone";
            enemyCard.name = "EnemyCard " + (i+1);

        }

        Debug.Log( PlayerArea.transform.childCount); // can get the amount of cards in the fields. (It did make a API update or something idk)

    }
    
    
    
    
}
