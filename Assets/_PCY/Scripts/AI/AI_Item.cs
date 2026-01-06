using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

using UnityEngine.Networking;
using UnityEngine.Animations;

[System.Serializable]
public class AIResponseCandidate
{
    public AIContent content;
}

[System.Serializable]
public class AIContent
{
    public AIPart[] parts;
}

[System.Serializable]
public class AIPart
{
    public string text;
}

[System.Serializable]
public class AIResponse
{
    public AIResponseCandidate[] candidates;
}

[System.Serializable]
public class RequestBody
{
    public int type;
    public string message;
}
[System.Serializable]
public class LLMResponseBody
{
    public string name;
    public int price;
    public string description;
    public string category;
    public string reasoning;
}

public class AI_Item : MonoBehaviour
{
    private string llmserverUrl = "http://0.0.0.0:8000";

    [Header("Reference script")]
    [SerializeField]
    private ScrollView scrollView;
    [SerializeField]
    private CategoryManager categoryManager;
    [SerializeField]
    private AI_Image imageGenerator;

    public IEnumerator RequestNewMetarial(int type, string inputItems)
    {
        Debug.Log("in request");
        var data = new RequestBody {type = type, message = inputItems};
        string jsonBody = JsonUtility.ToJson(data);
        using(UnityWebRequest request = UnityWebRequest.Post(llmserverUrl+"/create_item", jsonBody, "application/json"))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response from LLM Server: " + responseText);
                LLMResponseBody response = JsonUtility.FromJson<LLMResponseBody>(responseText);

                int categoryIndex = categoryManager.FindCategoryAboutItem(response.category);
                ItemData newItem = new ItemData {
                    id = 0, 
                    name = response.name, 
                    price = response.price, 
                    description = response.description,
                    category = categoryIndex
                };
                StartCoroutine(imageGenerator.GenerateImageCoroutine(response.reasoning));
                scrollView.AddNewItemSlot(newItem);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
