using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Skill class, representing a skill that can be used by players
public class Skill : MagicSystem
{
    //public static Skill Instance;
    public string SkillName { get; private set; }
    public string SkillDescription { get; private set; }
    public string SkillEffect { get; private set; }
    public int SkillDamage { get; private set; }
    public int ManaCost { get; private set; }

    // Reference to the Player prefab
    //public GameObject playerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    


    public void Initialize(string name, string description, string effect, int damage, int manaCost)
    {
        SkillName = name;
        SkillDescription = description;
        SkillEffect = effect;
        SkillDamage = damage;
        ManaCost = manaCost;
    }

    public void ActivateSkill()
    {
        Player.Instance.damage = SkillDamage;
    }
}