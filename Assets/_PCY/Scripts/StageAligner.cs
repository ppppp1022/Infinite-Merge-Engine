using UnityEngine;

// 이 속성이 있어야 게임 실행을 안 해도 에디터에서 작동합니다.
[ExecuteInEditMode] 
public class StageAligner : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("패널 사이의 간격")]
    public float interval = 10f; // 원하는 간격 입력
    
    [Tooltip("체크하면 세로로 배치, 끄면 가로로 배치")]
    public bool isVertical = false;

    // 인스펙터 값이 바뀔 때마다 자동으로 실행되는 함수
    void OnValidate()
    {
        AlignStages();
    }

    // 오브젝트가 변경될 때(자식이 추가되거나 할 때) 실행
    void Update()
    {
        // 성능을 위해 자식 개수가 변했을 때만 정렬하고 싶다면 로직 추가 가능
        // 지금은 편의상 매 프레임(에디터 상) 정렬 체크 (무거운 작업 아니므로 괜찮음)
        if (!Application.isPlaying) 
        {
            AlignStages();
        }
    }

    // 실제 정렬 로직
    public void AlignStages()
    {
        // 내 자식 오브젝트들을 전부 훑습니다.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            
            // 좌표 계산: 순서(i) * 간격(interval)
            float pos = i * interval;

            if (isVertical)
            {
                // 세로 배치 (Y축)
                child.localPosition = new Vector3(0, -pos, 0); 
            }
            else
            {
                // 가로 배치 (X축)
                child.localPosition = new Vector3(pos, 0, 0);
            }
        }
    }
}