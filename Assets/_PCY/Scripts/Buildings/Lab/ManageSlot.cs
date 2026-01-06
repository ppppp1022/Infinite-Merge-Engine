using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSlot : MonoBehaviour
{
    public bool isActive = false;
    public GameObject slot;
    public float lerpSpeed = 5f;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isActive)
            {
                isActive = true;
            }
            slot.SetActive(isActive);
        }
    }
    

    IEnumerator LerpCameraPosition(Camera cam)
    {
        Vector3 startPos = cam.transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
        
        float elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            cam.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cam.transform.position = targetPos;
    }
}
