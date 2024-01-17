/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    
    public int maxNumberOfItems;
    public ItemSlot[] itemSlot;

    public Item[] itemSOs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if(Input.GetKeyDown(KeyCode.E) && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
        else if(Input.GetKeyDown(KeyCode.R) && menuActivated)
        {
            RemoveItem();
            
        }
    }


    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if(itemSOs[i].ItemName == itemName)
            {
                bool usable = itemSOs[i].Use();
                return usable;
            }
        }
        return false;
    }
    public int AddItem(string itemName, int quantity,int itemWeight,int itemValue, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName,quantity,itemWeight,itemValue,itemSprite,itemDescription,itemType);
                if(leftOverItems >0)
                    leftOverItems = AddItem(itemName, leftOverItems,itemWeight,itemValue, itemSprite, itemDescription,itemType );

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




    public bool CheckCapacity()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].quantity > maxNumberOfItems)
            {
                return true;
            }
        }
        return false;
    }

    
}
*/