using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

[System.Serializable]
public struct stage_description
{
    public int buildingTypeIndex;
    public int stageLevel;
}
public class Manager : MonoBehaviour
{
    public static Manager instance;
    public List<stage_description> stage_Descriptions = new List<stage_description>();
    public List<BuildingData> buildingDatas = new List<BuildingData>();
    public BuildingData currentBuildingData;
    public stage_description currentStageDescription;
    private int stageId;

    public void LoadGameScene(int id)
    {
        try
        {
            currentStageDescription = stage_Descriptions[id];
            currentBuildingData = buildingDatas[currentStageDescription.buildingTypeIndex];
            stageId = id;

            SceneManager.LoadScene("Lab");
        }
        catch
        {
            Debug.Log("Can't enter: "+id);
            return;
        }
    }
    public void LoadUserData()
    {
        DataSaver dataSaver = GetComponent<DataSaver>();
        dataSaver.userData = dataSaver.LoadUserData();
        Debug.Log("user data loaded: "+dataSaver.userData);
        GameObject.Find("PreLobby").SetActive(false);
        UnlockMaximumLevelStageButton();
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            instance.LoadUserData();
            Destroy(gameObject);
            return;
        }

        // 최초 인스턴스이면 유지
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void victoryGame()
    {
        DataSaver dataSaver = GetComponent<DataSaver>();
        dataSaver.SaveUserData();
    }
    public List<GameObject> stageList = new List<GameObject>();
    public void UnlockMaximumLevelStageButton()
    {
        StageButton[] stages = FindObjectsByType<StageButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach(StageButton stage in stages)
        {
            // 안전 장치: stageList 배열 크기 확인 필요
            if (stage.stageIndex < stageList.Count)
            {
                stageList[stage.stageIndex] = stage.gameObject;
            }
        }
        for(int i = 0; i<GetComponent<DataSaver>().userData.maximumLevel; i++)
        {
            stageList[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            stageList[i].GetComponentInChildren<Button>().enabled = true;
        }
    }
    void Start()
    {
        UnlockMaximumLevelStageButton();
    }
}
