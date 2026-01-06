using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public int id;
    public string name;
    public float price;
    public string description;
    public int category;
    public string imagePath;
    [System.NonSerialized]
    public Texture2D itemImage;
}
public class ManageItem : MonoBehaviour
{
    public ItemData currentItemData = new ItemData();
    [SerializeField]
    public Image itemImage;
    [SerializeField]
    private TMP_Text priceUI;
    [SerializeField]
    private TMP_Text nameUI;
    [SerializeField]
    private TMP_Text descriptionUI;
    [SerializeField]
    private TMP_Text categoryUI;
    [SerializeField]
    private CategoryManager categoryManager;
    public void ChangeItemPrice(float newPrice)
    {
        if (newPrice <= 0)
        {
            throw new System.ArgumentOutOfRangeException("The price can't less than 0");
        }
        currentItemData.price = newPrice;
        priceUI.text = newPrice.ToString();
    }
    public void InitializeItem(int itemId ,string itemName, float itemPrice, string itemDescription, int itemCategory)
    {
        categoryManager = FindObjectOfType<CategoryManager>();
        if (itemId<0 ||itemName == null || itemPrice <= 0 || itemDescription == null || itemCategory <0)
        {
            Debug.Log(itemId+", "+itemName+", "+itemPrice+", "+itemDescription+", "+itemCategory);
            throw new System.ArgumentNullException("There are something wrong initial value settting, " +
            "check itemName, itemPrice, and itemDescription in ManageItem.cs ");
        }
        currentItemData.id = itemId;

        currentItemData.name = itemName;
        currentItemData.price = itemPrice;
        currentItemData.description = itemDescription;
        currentItemData.category = itemCategory;

        nameUI.text = itemName;
        priceUI.text = itemPrice.ToString();
        descriptionUI.text = itemDescription;
        categoryUI.text = categoryManager.categories[itemCategory];

    }
}
