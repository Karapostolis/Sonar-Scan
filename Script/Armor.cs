using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Armor class, a type of Item
public class Armor : Item
{
    public int DefensePoints;
    public int Durability;

    public override bool Use()
    {
        // Custom behavior for using a weapon
        Defend();
        return true;
    }

  
    public void Defend()
    {
       GameObject player = GameObject.FindGameObjectWithTag("Player");
       Player Player = player.GetComponent<Player>();
       Player.armor += DefensePoints;
    }
}