using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadImageFromURL : MonoBehaviour
{
    [Header("Main")]
    public string url = "http://127.0.0.1:8125/image.png";
    
    public Texture2D texture2D;
    public SpriteRenderer sr;

    public string error = "";

    [Header("Settings")]
    public string setting1Name = "Scale";
    public float setting1Value = 1f;

    void Start()
    {
        error = "start";
        Debug.Log(error);

        StartCoroutine(GetText());

        //error = "after coroutine";
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            uwr.certificateHandler = null;
            
            yield return uwr.SendWebRequest();

            error = "send";
            Debug.Log(error);

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);

                error = "error" + uwr.error;
                Debug.Log(error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                error = "downloaded";
                Debug.Log(error);

                texture2D = (Texture2D)texture;

                sr.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }
    }

    private void Update()
    {
        transform.localScale = setting1Value * transform.localScale;
    }
}
