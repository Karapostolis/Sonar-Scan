using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// NonPlayer class, representing a non-player character
public class NonPlayer : MonoBehaviour, Interactable
{
    public string enemyName;
    public int Level;
     public int MaxHealthPoints;
    public int HealthPoints;
    public int Damage;
    public Item[] Loot;


    // Flag to track whether loot should be dropped
    public static bool dropLootOnSceneLoad = false;


    public void Interact()
    {
        Debug.Log("You will start a battle");
        SceneManager.LoadScene("BattleScene");

    }
    public void Attack(Player player)
    {
        player.TakeDamage(Damage);
    }

    public void DropLoot()
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            // Set the flag to true when the player wins the battle
            dropLootOnSceneLoad = true;
            SceneManager.LoadScene("MainScene");            
        }
        
        
    }

    // Override TakeDamage to customize behavior if needed
    public  bool TakeDamage(int dmg)
    {
        HealthPoints -= dmg;
        if (HealthPoints <= 0)
        {
            Player.Instance.GainExperience(100);
            return true;
        }
        else
        {
            return false;
        }
    }



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe from the event when the script is disabled
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method will be called after a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene" && dropLootOnSceneLoad == true)
            {
                Debug.Log("Inside MainScene");

                // Choose a random item from the Loot array
                Item randomLoot = Loot[Random.Range(0, Loot.Length)];

                // Instantiate and configure the item object
                GameObject itemToDrop = new GameObject(randomLoot.ItemName);

                GameObject player = GameObject.FindWithTag("Player");

                Item newItem = itemToDrop.AddComponent<Item>();
                newItem.ItemName = randomLoot.ItemName;
                newItem.quantity = 1;
                newItem.sprite = randomLoot.sprite;
                newItem.ItemWeight = randomLoot.ItemWeight;
                newItem.ItemValue = randomLoot.ItemValue;
                newItem.ItemDescription = randomLoot.ItemDescription;

                // Create and modify the SR
                SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
                sr.sprite = randomLoot.sprite;
                sr.sortingOrder = 10;
                sr.sortingLayerName = "Background";

                // Add collider
                itemToDrop.AddComponent<BoxCollider2D>();

                // Set the location
                itemToDrop.transform.position = player.transform.position + new Vector3(1, 0, 0);
                itemToDrop.transform.localScale = new Vector3(.5f, .5f, .5f);

                Debug.Log($"Dropping loot: {string.Join(", ", newItem.ItemName)}");
                
            }
            else
            {
                dropLootOnSceneLoad = false;
            }

    }
}