using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HttpRequester : MonoBehaviour
{

    public RequestType requestType;
    public string url;
    public string body = "{}";
    public Action<DownloadHandler> complete;
    public Action<DownloadHandler> failed;
    public void Settting(RequestType requestType, string url)
    {
        this.requestType = requestType;
        this.url = url;
    }

    public void Complete(DownloadHandler result)
    {
        if(complete != null)
        {
            complete(result);
        }
    }

    public void Failed(DownloadHandler result)
    {
        if (complete != null)
        {
            complete(result);
        }
    }


}
