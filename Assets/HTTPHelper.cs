using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPHelper : MonoBehaviour
{
    public static HTTPHelper Instance;

    public int port;

    private HttpListener listener;
    private List<Thread> listenerThreads;

    private void Awake()
    {
        Instance = this;

    }

    public void StartNewListener(System.AsyncCallback Callback)
    {
        Thread listenerThread = new Thread(startListener);
        listenerThread.Start(Callback);
        listenerThreads.Add(listenerThread);
    }

    private void startListener(object CallbackData)
    {
        System.AsyncCallback Callback = (AsyncCallback)CallbackData;
        if (Callback != null)
        {
            while (true)
            {
                var result = listener.BeginGetContext(Callback, listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }
    }

    public static HttpListenerContext EndContext(System.IAsyncResult result)
    {
        return Instance.EndContextFromInstance(result);
    }

    public HttpListenerContext EndContextFromInstance(System.IAsyncResult result)
    {
        return listener.EndGetContext(result);
    }

    public static IEnumerator GetAvatar(string UserName, Action<Sprite> callback)
    {
        string URL = "https://decapi.me/twitch/avatar/" + UserName;
        using (UnityWebRequest Request = UnityWebRequest.Get(URL))
        {
            yield return Request.SendWebRequest();

            string[] pages = URL.Split('/');
            int page = pages.Length - 1;
            if (Request.error == null)
            {
                Debug.Log(pages[page] + ":\nReceived: " + Request.downloadHandler.text);
                HTTPHelper.Instance.StartCoroutine(GetImage(Request.downloadHandler.text, callback));
            }
        }
    }
    public static IEnumerator GetAvatar(string UserName, Action<Texture2D> callback)
    {
        string URL = "https://decapi.me/twitch/avatar/" + UserName;
        using (UnityWebRequest Request = UnityWebRequest.Get(URL))
        {
            yield return Request.SendWebRequest();

            string[] pages = URL.Split('/');
            int page = pages.Length - 1;
            if (Request.error == null)
            {
                Debug.Log(pages[page] + ":\nReceived: " + Request.downloadHandler.text);
                HTTPHelper.Instance.StartCoroutine(GetImage(Request.downloadHandler.text, callback));
            }
        }
    }

    public static IEnumerator GetImage(string URL, Action<Sprite> callback)
    {
        UnityWebRequest Request = UnityWebRequestTexture.GetTexture(URL);
        yield return Request.SendWebRequest();
        if (Request.error == null)
        {
            Texture2D texture = ((DownloadHandlerTexture)Request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 299, 299), Vector2.zero);
            callback?.Invoke(sprite);
        }
    }

    public static IEnumerator GetImage(string URL, Action<Texture2D> callback)
    {
        UnityWebRequest Request = UnityWebRequestTexture.GetTexture(URL);
        Request.timeout = 1;
        yield return Request.SendWebRequest();
        Texture2D texture = null;
        if (Request.error == null)
        {
            texture = ((DownloadHandlerTexture)Request.downloadHandler).texture;
        }
        callback?.Invoke(texture);
    }
}
