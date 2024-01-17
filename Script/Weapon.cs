using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon class, a type of equipment
public class Weapon : Item
{
    public static int DamagePoints;
    public int Durability;

    public GameObject playerPrefab; 
    
    
    public override bool Use()
    {
        // Custom behavior for using a weapon
        Attack();
        return true;
    }

    public void Attack()
    {
        Player prefabScript = playerPrefab.GetComponent<Player>();
        /*GameObject player = GameObject.FindGameObjectWithTag("Player");
        Player Player = player.GetComponent<Player>();*/
        //Player player = new Player();
        Player.Instance.damage += DamagePoints;
        prefabScript.damage = DamagePoints;
        Debug.Log("The damage changed" + Player.Instance.damage);
    }
}

