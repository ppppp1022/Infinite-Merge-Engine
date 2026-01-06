using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct BuildingData
{
    public int id;
    public string name;
    public int numberofSlot;
    public string description;
    public string imagePath;
    public Sprite buildingImage;
}
public class ManageBuilding : MonoBehaviour
{
    public BuildingData currentBuildingData = new BuildingData();
    public int itemSlotCount;
    public string buildingName;
    public string buildingDescription;
    public Button button;
    public int currentActivateSlot = 0;

    [SerializeField] private MakeNewItem makeNewItem;
    public void InitializeBuilding()
    {
        try
        {
            Manager manager = FindObjectOfType<Manager>();

            buildingDescription = manager.currentBuildingData.description;
            buildingName = manager.currentBuildingData.name;
            itemSlotCount = manager.currentBuildingData.numberofSlot;

            currentBuildingData.name = buildingName;
            currentBuildingData.numberofSlot = itemSlotCount;
            currentBuildingData.description = buildingDescription;
            currentBuildingData.imagePath = manager.currentBuildingData.imagePath;

            transform.GetComponent<SpriteRenderer>().sprite = manager.currentBuildingData.buildingImage;
        }
        catch
        {
            Debug.Log("It is test");
        }
    }
    public void ChangeActivateSlot(bool isIncrease)
    {
        if (isIncrease)
        {
            currentActivateSlot++;
            if (currentActivateSlot >= itemSlotCount)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            button.gameObject.SetActive(false);
            currentActivateSlot--;
        }
    }
    void Start()
    {
        InitializeBuilding();
    }
}
