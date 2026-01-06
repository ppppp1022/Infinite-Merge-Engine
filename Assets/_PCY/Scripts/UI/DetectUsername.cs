using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectUsername : MonoBehaviour
{
    public TMP_Text text;
    public GameObject enter;
    void Update()
    {
        if(text.text.Count()==1)
        {
            enter.SetActive(false);
        }
        else
        {
            enter.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(text.text.Count()!=1)
            {
                enter.GetComponent<Button>().onClick.Invoke();
            }

        }
    }
}
