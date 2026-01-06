using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

[System.Serializable]
public struct UserData
{
    public string userName;
    public int maximumLevel;
    public int countOfVisits;
    public string userGeminiContentAPIKey;
    public string userGeminiImagenAPIKey;
}
[System.Serializable]
public class ItemDataWrapper
{
    public List<ItemData> items;
}
public class DataSaver : MonoBehaviour
{
    public List<ItemData> itemDataList = new List<ItemData>();
    public UserData userData;
    public void SaveUserData(UserData data)
    {
        string json = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, "userdata.json");
        File.WriteAllText(filePath, json);
    }
    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(userData);
        string filePath = Path.Combine(Application.persistentDataPath, "userdata.json");
        File.WriteAllText(filePath, json);
    }
    public UserData LoadUserData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "userdata.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            Debug.LogWarning("User data file not found.");
            return default(UserData);
        }
    }
    public void SaveItemWithImage(ItemData item, int index)
    {
        try
        {
            // 1. 이미지 저장
            if (item.itemImage != null)
            {
                string imageName = $"item_image_{index}.png";
                byte[] imageBytes = item.itemImage.EncodeToPNG();
                string imagePath = Path.Combine(Application.persistentDataPath, imageName);
                File.WriteAllBytes(imagePath, imageBytes);

                item.imagePath = imageName; // 상대 경로만 저장
            }

            // 2. 데이터 저장
            string json = JsonUtility.ToJson(item, true);
            string dataPath = Path.Combine(Application.persistentDataPath, $"itemdata_{index}.json");
            File.WriteAllText(dataPath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"저장 실패: {e.Message}");
        }
    }
    public Texture2D LoadImage(string fileName)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2); // 임시 크기
                texture.LoadImage(bytes); // 자동으로 크기 조정됨
                return texture;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"이미지 로드 실패: {e.Message}");
        }

        return null;
    }
    public ItemData LoadItemWithImage(int index)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, $"itemdata_{index}.json");
        string json = File.ReadAllText(dataPath);
        ItemData item = JsonUtility.FromJson<ItemData>(json);

        // 이미지 로드
        if (!string.IsNullOrEmpty(item.imagePath))
        {
            item.itemImage = LoadImage(item.imagePath);
        }

        return item;
    }

    void Awake()
    {
        userData = LoadUserData();
        userData.countOfVisits+=1;
        if(userData.maximumLevel == 0)
        {
            userData.maximumLevel +=1;
        }
    }
    public void SetUsername(TMP_Text text)
    {
        userData.userName = text.text;
        SaveUserData();  
    }
}
