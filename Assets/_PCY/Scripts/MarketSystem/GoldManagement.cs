using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManagement : MonoBehaviour
{
    [SerializeField]
    GameObject ItemSlotUI;
    [SerializeField]
    TMP_Text goldAmountText;
    [SerializeField]
    StageManager stageManager;
    public int currentGoldAmount=0;
    [Header("골드 차감 비율 설정")]
    public int ratioOfTurnPenalty;
    public int ratioOfBuildingPenalty;
    private List<int> buildingPenaltyByLevel = new List<int> {2, 3, 3};
    private List<int> turnPenaltyByLevel = new List<int> {500, 1000, 3000};
    private List<int> baseMoneyByLevel = new List<int> {100000, 50000, 30000};
    public int SumOfAllItemValues()
    {
        ManageItem[] manageItems = ItemSlotUI.GetComponentsInChildren<ManageItem>();
        float totalValue = 0;
        foreach (ManageItem item in manageItems)
        {
            if(item.gameObject.activeSelf)
            {
                totalValue += item.currentItemData.price;
            }
        }
        return (int)totalValue;
    }
    public int SubstractGoldAtFinishedTurn(int turnCount)
    {
        return turnCount * ratioOfTurnPenalty;
    }
    public bool SubstractGoldAtBuildingFacility(int facilityCost)
    {
        int netGoldChange = currentGoldAmount - facilityCost * ratioOfBuildingPenalty;
        if(netGoldChange>=0)
        {
            currentGoldAmount = netGoldChange;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ChangeGoldAmount(int turnCount)
    {
        int goldToSubtract = SubstractGoldAtFinishedTurn(turnCount);
        int goldToEarn = SumOfAllItemValues();
        int netGoldChange = currentGoldAmount + goldToEarn - goldToSubtract;
        if(netGoldChange < 0)
        {
            Debug.Log("game over....");
            stageManager.StageOver(false);
        }
        int futureEarn = -SubstractGoldAtFinishedTurn(GetComponent<TurnManagement>().currentTurn+1) + SumOfAllItemValues();
        currentGoldAmount = netGoldChange;
        string futureMinus = futureEarn>=0 ? "+"+futureEarn.ToString() : futureEarn.ToString();
        goldAmountText.text = "골드: " + netGoldChange.ToString() +" "+ futureMinus;
    }
    void Start()
    {
        try
        {
            Manager manager = FindObjectOfType<Manager>();
            int level = manager.currentStageDescription.stageLevel-1;
            ratioOfBuildingPenalty = buildingPenaltyByLevel[level];
            ratioOfTurnPenalty = turnPenaltyByLevel[level];
            currentGoldAmount = baseMoneyByLevel[level];
            int futureEarn = -SubstractGoldAtFinishedTurn(GetComponent<TurnManagement>().currentTurn+1) + SumOfAllItemValues();
            string futureMinus = futureEarn>=0 ? "+"+futureEarn.ToString() : futureEarn.ToString();
            goldAmountText.text = "골드: " + currentGoldAmount.ToString()+" "+ futureMinus;
        }
        catch
        {
            Debug.Log("It is test");
        }
    }
}
