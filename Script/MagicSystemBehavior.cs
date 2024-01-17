using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MagicSystemBehavior : MonoBehaviour
{
    public MagicSystem magicSystem;

    void Start()
    {
        // Initialize the MagicSystem
        magicSystem.Initialize();

        // Learn the first skill
        magicSystem.LearnSpell(magicSystem.Spells[0]);

        // Cast a spell (assuming there's a Spell object in the scene)
        Skill spellToCast = magicSystem.Spells[0];
        magicSystem.CastSpell(spellToCast);
    }
}
