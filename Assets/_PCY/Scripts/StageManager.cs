using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;  

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private List<Sprite> imageList;
    [SerializeField]
    private List<ItemData> itemDataList;
    public int startItem=4;
    [SerializeField]
    private ScrollView scrollView;
    public void GoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void StageOver(bool isWin)
    {
        if(isWin)
        {
            winPanel.SetActive(true);
            FindObjectOfType<DataSaver>().userData.maximumLevel += 1;
            FindObjectOfType<Manager>().victoryGame();
        }
        else
        {
            losePanel.SetActive(true);
        }
    }
    public (List<Sprite>,List<ItemData>) RandomGenerateItemFromSet(int n)
    {
        List<Sprite> result_sprite = new List<Sprite>();
        List<ItemData> result_itemdata = new List<ItemData>();
    
        HashSet<int> selectedIndices = new HashSet<int>();

        if (n > imageList.Count) n = imageList.Count;

        while (selectedIndices.Count < n)
        {
            int randomIndex = Random.Range(0, imageList.Count);
            selectedIndices.Add(randomIndex); // HashSet은 중복된 값은 알아서 무시함
            
        }

        // 2. 뽑힌 인덱스에 해당하는 이미지를 결과 리스트에 담기
        foreach (int index in selectedIndices)
        {
            result_sprite.Add(imageList[index]);
            result_itemdata.Add(itemDataList[index]);
        }
        return (result_sprite, result_itemdata);

    }

    void Awake()
    {
        var (sprites, itemDatas) = RandomGenerateItemFromSet(startItem);
        for(int i = 0; i<sprites.Count;i++)
        {
            scrollView.AddNewItemSlot(itemDatas[i], sprites[i]);
        }
    }
}
