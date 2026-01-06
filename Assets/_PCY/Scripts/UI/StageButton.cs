 using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public int stageIndex;

    void Start()
    {
        try
        {
            // 인스펙터 연결 없이, 코드로 '진짜 매니저'를 찾아서 연결
            GetComponentInChildren<Button>().onClick.AddListener(() => 
                Manager.instance.LoadGameScene(stageIndex)
            );
        }
        catch
        {
            Debug.Log("delay");
        }
        
    }
}