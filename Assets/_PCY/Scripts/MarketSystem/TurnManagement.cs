using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManagement : MonoBehaviour
{
    [SerializeField]
    TMP_Text turnText;
    public int currentTurn = 1;
    public int maximumTurn;
    [SerializeField]
    TMP_Text historyScoreText;
    int historyScore = 0;
    [SerializeField]
    GoldManagement goldManagement;
    [SerializeField]
    StageManager stageManager;
    [SerializeField]
    private AI_Image buffer;

    public int queueNewItemCount = 0;
    public GameObject temporalItem;
    public Sprite temporalSprite;
    public AudioSource audioSource;

    void Start()
    {
        try
        {
            Manager manager = FindObjectOfType<Manager>();
            int level = manager.currentStageDescription.stageLevel;
            maximumTurn = (level)*15;
        }
        catch
        {
            Debug.Log("It is test");
        }
        UpdateTurnText();
        UpdateHistoryScoreText();
    }
    [SerializeField] private GameObject effect;
    public void NextTurn()
    {
        currentTurn++;
        queueNewItemCount -=1;
        if (currentTurn % 10 == 1)
        {
            /*
            // 매 10턴마다 특정 이벤트 발생 예시
            */
            Debug.Log("특별 이벤트 발생!");
        }
        goldManagement.ChangeGoldAmount(currentTurn);
        if(queueNewItemCount<=0)
        {
            effect.SetActive(false);
            if(temporalItem!=null)
            {
                ManageItem manageItem = temporalItem.GetComponent<ManageItem>();
                manageItem.itemImage.sprite = buffer.spriteBuffer;
                buffer.spriteBuffer = null;
                temporalItem.SetActive(true);
                temporalItem = null;
                audioSource.Play();
            }
        }
        UpdateTurnText();
    }
    void UpdateTurnText()
    {
        turnText.text = "현재 턴: " + currentTurn.ToString();
        if(currentTurn>=maximumTurn)
        {
            stageManager.StageOver(true);
        }
    }
    public void AddToHistoryScore(int score)
    {
        historyScore += score;
        UpdateHistoryScoreText();
    }
    void UpdateHistoryScoreText()
    {
        historyScoreText.text = "업적 점수 " + historyScore.ToString();
    }
}
