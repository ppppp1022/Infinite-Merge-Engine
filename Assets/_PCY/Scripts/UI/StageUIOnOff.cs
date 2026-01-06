using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUIOnOff : MonoBehaviour
{
    public void OnandOff(GameObject stageUI)
    {
        if(stageUI.activeSelf)
        {
            stageUI.SetActive(false);
        }
        else
        {
            stageUI.SetActive(true);
        }
    }
}
