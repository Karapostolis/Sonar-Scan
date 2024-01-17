using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour,IPointerClickHandler
{

    public string itemName;
    public int quantity;
    public int itemWeight;
    public int itemValue;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public ItemType itemType;
/*
    [SerializeField]
    private int maxNumberOfItems;



*/

    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;


    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;



    public GameObject selectedShader;
    public bool thisItemSelected;

    private Inventory inventoryManager;


    

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }

    
    
    public int AddItem(string itemName, int quantity,int itemWeight,int itemValue, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        // Check to see if thr slot is already full
        if(isFull)
        {
            return quantity;
        }

        // Update ITEM TYPE 
        this.itemType = itemType;

        // Update Name
        this.itemName = itemName;

        this.itemWeight = itemWeight;
        this.itemValue = itemValue;

        // Update Image
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        // Update Description
        this.itemDescription = itemDescription;

        // Update Quantity
        this.quantity += quantity;
        //if(this.quantity >= inventoryManager.maxNumberOfItems)
        if(inventoryManager.CheckCapacity(this.quantity))
        {
            quantityText.text = inventoryManager.Capacity.ToString();
            quantityText.enabled = true;
            isFull = true;
        

            //Return the LEFTOVERS
            int extraItems = this.quantity - inventoryManager.Capacity;
            this.quantity = inventoryManager.Capacity;
            return extraItems;

        }

        // Update quantity text
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }


    public void OnLeftClick()
    {
        if(thisItemSelected)
        {
            bool usable = inventoryManager.UseItem(itemName);
            if(usable)
            {
                this.quantity -=1;
                quantityText.text = this.quantity.ToString();
                if(this.quantity <=0)
                    EmptySlot();
            }
            
        }

        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
            if(itemDescriptionImage.sprite == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }

        }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;

        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
    }

    public void OnRightClick()
    {
        if (quantity >= 0 )
        {
            //Create a new item
        GameObject itemToDrop = new GameObject(itemName);
        Debug.Log("The item Name is: "+ itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.ItemName = itemName;
        newItem.quantity = 1;
        newItem.sprite = itemSprite;
        newItem.ItemWeight = itemWeight;
        newItem.ItemValue = itemValue;
        newItem.ItemDescription = itemDescription;

        // Create and modifu the SR
        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemSprite;
        sr.sortingOrder = 5;
        sr.sortingLayerName = "Background";

        // Add colider
        itemToDrop.AddComponent<BoxCollider2D>();

        //Set the location
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(1, 0,0);
        itemToDrop.transform.localScale = new Vector3(.5f,.5f,.5f);

        //Subtract the item
        this.quantity -=1;
        quantityText.text = this.quantity.ToString();
        if(quantity <=0)
            EmptySlot();
        }
        
    }



}
