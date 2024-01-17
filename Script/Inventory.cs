using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// Inventory class, representing a player's inventory
public class Inventory : MonoBehaviour
{/*
    public List<Item> Items;
    public int Capacity;


*/

    public GameObject InventoryMenu;
    private bool menuActivated;
    
    public int Capacity;
    public ItemSlot[] itemSlot;

    public Item[] Items;


    private CanvasGroup inventoryCanvasGroup;

    void Start()
    {
        // Get the Renderer component from the GameObject
        inventoryCanvasGroup = transform.Find("InventoryMenu")?.GetComponent<CanvasGroup>();

        // Ensure that the inventoryMenu panel starts as invisible
        SetInventoryMenuVisibility(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && menuActivated)
        {
            Time.timeScale = 1;
            //InventoryMenu.SetActive(false);
            SetInventoryMenuVisibility(false);
            menuActivated = false;
        }

        else if(Input.GetKeyDown(KeyCode.E) && !menuActivated)
        {
            Time.timeScale = 0;
            //InventoryMenu.SetActive(true);
            SetInventoryMenuVisibility(true);
            menuActivated = true;
        }
        else if(Input.GetKeyDown(KeyCode.R) && menuActivated)
        {
            RemoveItem();
            
        }
    }

    void SetInventoryMenuVisibility(bool isVisible)
    {
        // Set the alpha and interactable properties based on the visibility flag
        inventoryCanvasGroup.alpha = isVisible ? 1f : 0f;
        inventoryCanvasGroup.interactable = isVisible;
        inventoryCanvasGroup.blocksRaycasts = isVisible;
    }


    public bool UseItem(string itemName)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if(Items[i].ItemName == itemName)
            {
                bool usable = Items[i].Use();
                return usable;
            }
        }
        return false;
    }
    public int AddItem(string itemName, int quantity,int itemWeight,int itemValue, Sprite itemSprite, string itemDescription,ItemType itemType)
    {

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName,quantity,itemWeight,itemValue,itemSprite,itemDescription,itemType);
                if(leftOverItems > 0)
                    leftOverItems = AddItem(itemName, leftOverItems,itemWeight,itemValue, itemSprite, itemDescription,itemType);

                return leftOverItems;
            }
        }
        return quantity;
        
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }



    public void RemoveItem()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].thisItemSelected == true)
            {
                
                itemSlot[i].OnRightClick();
            }
        }
        
    }




    public bool CheckCapacity(int quantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(quantity >= Capacity)
            {
                return true;
            }
        }
        return false;
    }

}
