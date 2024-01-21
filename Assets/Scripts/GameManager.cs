using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public int teamOneScore = 0; //T‰m‰ on pelaaja.
    public int teamTwoScore = 0; //T‰m‰ on AI.
    
    private void Awake() 
    {
         Instance = this;
    }

    public void UpdateGameState(GameState newState)
    {
        State = (newState);
        
        switch (newState) {



            case GameState.Victory:
                break;
            case GameState.Lose:
                break;

            // T‰nne mahollisesti tavarat alhaalta enum GameStatesta


        }
        

            

    }
    /* Ei tee mit‰‰n viel‰. Testi
  
    public void UpdatePlayerScore(int score)
    {
      teamOneScore += score;
        Debug.Log("Player Score: " + teamOneScore);
    
    }
    */




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum GameState
{
    Victory,

    Lose,
}
