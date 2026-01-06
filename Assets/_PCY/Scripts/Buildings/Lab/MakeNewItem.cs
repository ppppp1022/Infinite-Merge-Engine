using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNewItem : MonoBehaviour
{
    public List<GameObject> inputItemSlots;
    public int makeItemCost;
    public GameObject outputItemSlot;
    public GameObject Gemini;
    public GameObject llm;
    void Start()
    {
        if (llm == null)
        {
            llm = GameObject.Find("LLM");
        }
        if(Gemini==null)
        {
            Gemini = GameObject.Find("Gemini");
        }
    }
    public IEnumerator GenerateNewItemForManually(List<ItemData> newItemDatas)
    {
        GameObject.Find("Scroll View").SetActive(false);
        string inputText = "합성할 아이템 수: " + newItemDatas.Count + "\n";
        for (int i = 0; i < newItemDatas.Count; i++)
        {
            var itemData = newItemDatas[i];
            string category = GameObject.FindObjectOfType<CategoryManager>().categories[itemData.category];
            inputText += $"아이템 정보 {i + 1}: {itemData.name}, {itemData.price}, {itemData.description}, {category}\n";
        }
        
        if(FindObjectOfType<GoldManagement>().SubstractGoldAtBuildingFacility(makeItemCost))
        {
            Debug.Log("input text: "+inputText);
            StartCoroutine(llm.GetComponent<AI_Item>().RequestNewMetarial(type: FindObjectOfType<Manager>().currentStageDescription.buildingTypeIndex, inputItems: inputText));
        }
        else
        {
            Debug.Log("you can't make new Item!");
        }

        yield return null;
        Debug.Log("sss");
    }
}
