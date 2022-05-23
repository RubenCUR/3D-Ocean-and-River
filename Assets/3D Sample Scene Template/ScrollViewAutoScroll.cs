using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

using UnityEngine.Networking;

public class ScrollViewAutoScroll : MonoBehaviour
{
    [Header("Main")]
    public string url1 = "http://192.168.0.26:8079/0croppedImage.jpg";
    public string url2 = "http://192.168.0.26:8079/1croppedImage.jpg";
    public string url3 = "http://192.168.0.26:8079/2croppedImage.jpg";

    public string[] url = new string[100];

    public Texture2D texture2D;

    public SpriteRenderer sr;

    public SpriteRenderer[] srrs = new SpriteRenderer[100];

    public SpriteRenderer sr1;
    public SpriteRenderer sr2;
    public SpriteRenderer sr3;

    public float Timer = 0;
    public int SecondsDelay = 2;

    public int SpriteCounter = 0;

    public string error = "";

    [Header("Settings")]
    public string setting1Name = "Scale";
    public float setting1Value = 1f;

    //public ScrollView sv;
    //public ScrollRect sr;

    public float counter = 0;

    public RectTransform rt;

    public float sizedeltay;

    // Start is called before the first frame update
    void Start()
    {
        //counter = -(rt.sizeDelta.y / 2);

        error = "start";
        Debug.Log(error);

        //StartCoroutine(GetTexture(url1, sr1));
        //StartCoroutine(GetTexture(url2, sr2));
        //StartCoroutine(GetTexture(url3, sr3));

        if(sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (sr != null && sr1 != null)
            sr.sprite = sr1.sprite;

        for (int i = 0; i < url.Length; i++)
        {
            url[i] = "http://192.168.0.26:8079/" + i + "croppedImage.jpg";
        }

        for (int i = 0; i < srrs.Length; i++)
        {
            GameObject go = new GameObject();

            go.AddComponent<SpriteRenderer>();

            srrs[i] = go.GetComponent<SpriteRenderer>();

            Destroy(go);
        }

        for (int i = 0; i < url.Length; i++)
        {
            int result = 0;

            //StartCoroutine(GetTexture(url[i], srrs[i], OnComplete));

            StartCoroutine(GetTexture(url[i], srrs[i], (didError) => {
                Debug.Log("Did we get an error?" + didError);
                result = didError;
            }));

            if (result == 1)
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////sizedeltay = rt.sizeDelta.y;

        ////counter++;

        //if (counter > (rt.sizeDelta.y / 2))
        //{
        //    counter = -(rt.sizeDelta.y / 2);
        //}

        ////if (counter > 1000)
        ////    counter = 0;

        //sr.verticalScrollbar.value = counter * 1;
        //sr.verticalNormalizedPosition = 1000 / counter;
        //sr.verticalScrollbar.value = 1000/counter;

        ////rt.SetPositionAndRotation(new Vector3(rt.position.x, counter, rt.position.z), rt.rotation);

        transform.localScale = setting1Value * transform.localScale;


        if (Timer >= SecondsDelay)
        {
            Timer = 0;

            if (srrs[SpriteCounter] != null)
                if (srrs[SpriteCounter].sprite != null)
                {
                    sr.sprite = srrs[SpriteCounter].sprite;
                    SpriteCounter++;
                }

            //SpriteCounter++;

            //if (sr != null && sr1 != null && sr2 != null && sr3 != null && sr1.sprite != null && sr2.sprite != null && sr3.sprite != null)
            //{
            //    if (sr.sprite == sr1.sprite)
            //    {
            //        sr.sprite = sr2.sprite;
            //    }
            //    else if (sr.sprite == sr2.sprite)
            //    {
            //        sr.sprite = sr3.sprite;
            //    }
            //    else if (sr.sprite == sr3.sprite || sr.sprite == null)
            //    {
            //        sr.sprite = sr1.sprite;
            //    }
            //    else
            //    {
            //        sr.sprite = sr1.sprite;
            //    }
            //}
        }

        Timer += Time.deltaTime;
    }

    IEnumerator GetTexture(string url, SpriteRenderer srr, System.Action<int> callbackOnFinish)
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

                callbackOnFinish(1);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                error = "downloaded";
                Debug.Log(error);

                texture2D = (Texture2D)texture;

                srr.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);

                callbackOnFinish(0);
            }
        }
    }

    public void OnComplete(int didError)
    {
        Debug.Log("Did we get an error?" + didError);

        //return didError;
    }
}