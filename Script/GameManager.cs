using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        

        // Create a player
        var player1 = FindAnyObjectByType<Player>();
        //player1.CharacterName = "Hero";
        player1.Level = 1;
        player1.ExperiencePoints = 0;
        player1.HealthPoints = 100;
        player1.ManaPoints = 50;
        player1.Gold = 10;

        
    }
}
