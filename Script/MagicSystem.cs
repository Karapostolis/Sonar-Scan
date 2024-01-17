using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "MagicSystem", menuName = "Custom/Create MagicSystem Data")]
// MagicSystem class, representing the magic system of a character
public class MagicSystem : ScriptableObject
{

    //public static MagicSystem Instance;
    public List<Skill> Spells;
    public int ManaPool;
    public static MagicSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Initialize the MagicSystem with default values
    public void Initialize()
    {
        Spells = new List<Skill>();
        // Add initial skills to the list if needed
        Spells.Add(CreateSkill("Fireball", "Burn", "Burn", 5, 10));
        Spells.Add(CreateSkill("Ice Shard", "Damage", "Damage", 10, 15));
        Spells.Add(CreateSkill("Fire Wall", "Burn", "Burn", 20, 12));
        Spells.Add(CreateSkill("Ice Wall", "Damage", "Damage", 30, 20));
        ManaPool = 100;
    }

    // Method to create and add a new skill to the list
    public Skill CreateSkill(string name, string description, string effect, int damage, int manaCost)
    {
        Skill newSkill = ScriptableObject.CreateInstance<Skill>();
        newSkill.Initialize(name, description, effect, damage, manaCost);
        //Instance = this;

        // Save the new skill asset to the Skills folder
        string folderPath = "Assets/Skills";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Skills");
        }

        string assetPath = $"{folderPath}/{name}.asset";
        AssetDatabase.CreateAsset(newSkill, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        //Spells.Add(newSkill);
        return newSkill;
    }

    public void CastSpell(Skill spell)
    {
        if (Spells.Contains(spell) && ManaPool >= spell.ManaCost)
        {
            spell.ActivateSkill();
            ManaPool -= spell.ManaCost;
        }
        else
        {
            Debug.Log("Not enough mana or spell not learned.");
        }
    }

    public void LearnSpell(Skill spell)
    {
        Spells.Add(spell);
    }

    public void RegenerateMana(int mana)
    {
        ManaPool += mana;
    }
}