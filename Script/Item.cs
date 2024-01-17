using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string ItemName;
    public int quantity;
    public Sprite sprite;

    public int ItemWeight;

    public int ItemValue;

    public int durability;

    [TextArea]
    [SerializeField]
    public string ItemDescription;

    private Inventory inventoryManager;



    public Player playerScript;
    public Weapon weaponScript;
    public Armor armorScript;






    //========== Stats to change =======//

    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public ItemType itemType;
    public int amountToChangeEquipment;

    public enum StatToChange
    {
        none,
        health,
        mana
    };

    

    

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            int leftOverItems = inventoryManager.AddItem(ItemName, quantity,ItemWeight,ItemValue,sprite,ItemDescription,itemType);
            if (leftOverItems <= 0)
                //Debug.Log("Will be destroyed");
                Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }

    

    public virtual bool Use()
    {
        //Player playerScript = GetComponent<Player>();
        if(itemType == ItemType.consumable && statToChange == StatToChange.health)
        {
            if(playerScript.HealthPoints == playerScript.MaxHealthPoints)
            {
                return false;
            }
            else
            {
                playerScript.Heal(amountToChangeStat);
                return true;
            }
        }
        else if(itemType == ItemType.consumable && statToChange == StatToChange.mana)
        {
            MagicSystem.Instance.RegenerateMana(amountToChangeStat);
        }
        else if (itemType == ItemType.mainHand)
        {
            Weapon.DamagePoints = amountToChangeEquipment;
            return weaponScript.Use();
        }
        else if (itemType == ItemType.body)
        {
            armorScript.DefensePoints = amountToChangeEquipment;
            return armorScript.Use();
        }
        return false;
    }
}

public enum ItemType
    {
        none,
        consumable,
        mainHand,
        body
    };