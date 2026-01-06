using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class AI_Image : MonoBehaviour
{
    [Header("API Configuration")]
    public string geminiApiKey = "YOUR_GEMINI_API_KEY_HERE";
    
    private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-image-preview:generateContent";
    
    [System.Serializable]
    public class GeminiRequest
    {
        public List<Content> contents;
    }
    
    [System.Serializable]
    public class Content
    {
        public List<Part> parts;
    }
    
    [System.Serializable]
    public class Part
    {
        public string text;
    }
    
    [System.Serializable]
    public class GeminiResponse
    {
        public List<Candidate> candidates;
    }
    
    [System.Serializable]
    public class Candidate
    {
        public Content content;
    }
    public IEnumerator GenerateImageCoroutine(string prompt)
    {
        Debug.Log("이미지 생성 중...");
        
        prompt = "다음의 설명에 부합하는 이미지를 200*200pixel로 만들어줘. 설명이 부족하다면 너가 알아서 첨부해서 만들어야해\n"+prompt;
        // API 요청 데이터 구성
        var requestData = new GeminiRequest
        {
            contents = new List<Content>
            {
                new Content
                {
                    parts = new List<Part>
                    {
                        new Part { text = prompt }
                    }
                }
            }
        };
        
        string jsonData = JsonUtility.ToJson(requestData);
        
        // HTTP 요청 생성
        using (UnityWebRequest request = new UnityWebRequest(GEMINI_API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            // 헤더 설정
            request.SetRequestHeader("x-goog-api-key", geminiApiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"API 요청 실패: {request.error}");
                Debug.Log($"Response: {request.downloadHandler.text}");
                yield return StartCoroutine(TestImage());
                yield return null;
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"Response: {responseText}");
                
                // 응답에서 이미지 데이터 추출
                yield return StartCoroutine(ExtractAndDisplayImage(responseText));
            }
        }
    }
    public Sprite spriteBuffer;
    [SerializeField]
    private Sprite testImage;
    private IEnumerator TestImage()
    {
        spriteBuffer = testImage;
        Debug.Log("임시 이미지 생성");
        yield return null;
    }
    private IEnumerator ExtractAndDisplayImage(string responseJson)
    {
        spriteBuffer = null;
        try
        {
            // JSON에서 base64 이미지 데이터 추출
            string base64Data = ExtractBase64Data(responseJson);
            
            if (string.IsNullOrEmpty(base64Data))
            {
                Debug.Log("응답에서 이미지 데이터를 찾을 수 없습니다");
                yield break;
            }
            
            // Base64 디코딩
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            
            // 텍스처 생성
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                // 스프라이트 생성 및 표시
                spriteBuffer = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                
                Debug.Log("이미지 생성 완료");
                
            }
            else
            {
                Debug.Log("이미지 로딩 실패");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Image processing error: {e}");
        }
        
        yield return null;
    }
    
    private string ExtractBase64Data(string jsonResponse)
    {
        // JSON 응답에서 "data" 필드의 base64 데이터 추출
        int dataIndex = jsonResponse.IndexOf("\"data\":");
        if (dataIndex == -1) return null;
        
        int startQuote = jsonResponse.IndexOf('"', dataIndex + 7);
        if (startQuote == -1) return null;
        
        int endQuote = jsonResponse.IndexOf('"', startQuote + 1);
        if (endQuote == -1) return null;
        
        return jsonResponse.Substring(startQuote + 1, endQuote - startQuote - 1);
    }
    // 테스트용 메서드 - Inspector에서 호출 가능
}
