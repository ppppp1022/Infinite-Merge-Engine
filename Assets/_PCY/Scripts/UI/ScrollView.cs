using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Video;
public class ScrollView : MonoBehaviour
{
    //1. 버튼 클릭시 마스크 온 오프
    public void ChangeDescriptionPanelActive(Mask descriptionPanel)
    {
        if (descriptionPanel.enabled == false)
        {
            descriptionPanel.enabled = true;
        }
        else
        {
            descriptionPanel.enabled = false;
        }
    }
    //2. 스크롤뷰에 아이템 슬롯 추가
    [SerializeField]
    private Transform contentPanel;
    [SerializeField]
    private GameObject itemSlotPrefab;
    public List<GameObject> itemSlots = new List<GameObject>();
    public int queueCount;
    public void AddNewItemSlot(ItemData itemData)
    {
        GameObject newItemSlot = Instantiate(itemSlotPrefab, contentPanel);
        ManageItem manageItem = newItemSlot.GetComponent<ManageItem>();
        
        int currentIndex = itemSlots.Count;
        manageItem.InitializeItem(currentIndex, itemData.name, itemData.price, itemData.description, itemData.category);

        Button slotButton = newItemSlot.GetComponent<Button>();
        slotButton.onClick.AddListener(() => OnButtonPressed(currentIndex));

        newItemSlot.GetComponent<Image>().enabled = false;

        itemSlots.Add(newItemSlot);
        newItemSlot.SetActive(false);
        
        FindObjectOfType<TurnManagement>().temporalItem = newItemSlot;
    }
    public void AddNewItemSlot(ItemData itemData, Sprite sprite)
    {
        GameObject newItemSlot = Instantiate(itemSlotPrefab, contentPanel);
        ManageItem manageItem = newItemSlot.GetComponent<ManageItem>();
        
        int currentIndex = itemSlots.Count;
        manageItem.InitializeItem(currentIndex, itemData.name, itemData.price, itemData.description, itemData.category);

        if (manageItem.itemImage != null)
        {
            manageItem.itemImage.sprite = sprite;
        }

        Button slotButton = newItemSlot.GetComponent<Button>();
        slotButton.onClick.AddListener(() => OnButtonPressed(currentIndex));

        newItemSlot.GetComponent<Image>().enabled = false;

        itemSlots.Add(newItemSlot);
    }
    //3. 스크롤 뷰 온/오프
    public void ChangeScrollViewActive(GameObject scrollView)
    {
        if (scrollView.activeSelf == false)
        {
            scrollView.SetActive(true);
        }
        else
        {
            foreach (GameObject itemSlot in itemSlots)
            {
                itemSlot.GetComponent<Image>().enabled = false;
            }
            scrollView.SetActive(false);
        }
    }
    //4. 아이템 선택
    public int maxSelectItemCount = 2;
    public Button generateButton;
    public void OnButtonPressed(int id)
    {
        Image highlight = itemSlots[id].GetComponent<Image>();
        if(highlight.enabled == false)
        {
            if(activeSlotIndecies.Count >= maxSelectItemCount)
            {
                return;
            }
            highlight.enabled = true;
            activeSlotIndecies.Add(id);
            if(activeSlotIndecies.Count == maxSelectItemCount)
            {
                generateButton.gameObject.SetActive(true);
            }
        }
        else
        {
            highlight.enabled = false;
            activeSlotIndecies.Remove(id);
            if(activeSlotIndecies.Count < maxSelectItemCount)
            {
                generateButton.gameObject.SetActive(false);
            }
        }
    }
    //5. 선택된 아이템을 makenewitem 스크립트로 전달, generate btn이 눌렸을 때
    public List<int> activeSlotIndecies = new List<int>();
    [SerializeField] private MakeNewItem makeNewItem;
    [SerializeField] private GameObject effect;
    public VideoPlayer videoPlayer;
    public GameObject videoUI; 
    public AudioSource audioSource;
    public void GenerateDecide()
    {
        effect.SetActive(true);
        FindObjectOfType<TurnManagement>().queueNewItemCount = queueCount;
        List<ItemData> selectedItems = new List<ItemData>();
        foreach(int index in activeSlotIndecies)
        {
            GameObject slot = itemSlots[index];
            string itemName = slot.GetComponentsInChildren<TMP_Text>()[0].text;
            float itemPrice = float.Parse(slot.GetComponentsInChildren<TMP_Text>()[1].text);
            string itemDescription = slot.GetComponentsInChildren<TMP_Text>()[2].text;
            Sprite itemImage = slot.GetComponentsInChildren<Image>()[1].sprite;

            ItemData itemData = new ItemData
            {
                name = itemName,
                price = itemPrice,
                description = itemDescription,
                itemImage = itemImage.texture
            };
            selectedItems.Add(itemData);
        }
        activeSlotIndecies = new List<int>(); 
        foreach (GameObject itemSlot in itemSlots)
        {
            itemSlot.GetComponent<Image>().enabled = false;
        }
        
        //PlayVideo();
        StartCoroutine(makeNewItem.GenerateNewItemForManually(selectedItems));
    }
    void OnVideoEnd(VideoPlayer vp)
    {   
        audioSource.Play();
        videoUI.SetActive(false);    // 영상 재생 끝 → 화면 숨기기
    }
    public void PlayVideo()
    {
        audioSource.Pause();
        videoUI.SetActive(true);     // 재생 시작과 동시에 화면 켜기
        videoPlayer.Play();
    }


    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;

    }
}
