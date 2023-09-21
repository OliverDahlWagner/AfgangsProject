using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{

    public List<GameObject> theCards; // if we do it like this all the cards will be in there. ON THE BUTTON
                                        // could maybe in future be made to a game-manager or a deck/hand manager (the button object in general)
                                        

    public GameObject PlayerArea;
    public GameObject EnemyArea;

    
    
    void Start()
    {
        
    }

    public void OnClick()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject playerCard = Instantiate(theCards[Random.Range(0,theCards.Count)], new Vector3(0,0,0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false); // sets the parent to the PlayerArea transform. So it will show
            playerCard.tag = "PlayerZone";
            playerCard.name = "PlayerCard " + (i+1);
            
            GameObject enemyCard = Instantiate(theCards[Random.Range(0,theCards.Count)], new Vector3(0,0,0), Quaternion.identity);
            enemyCard.transform.SetParent(EnemyArea.transform, false); // sets the parent to the PlayerArea transform. So it will show
            enemyCard.tag = "EnemyZone";
            enemyCard.name = "EnemyCard " + (i+1);

        }

        Debug.Log( PlayerArea.transform.childCount); // can get the amount of cards in the fields. (It did make a API update or something idk)
    
    }
    
    
    
    
}
