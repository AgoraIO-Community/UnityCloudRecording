using UnityEngine;
using System;


// Returns json {"resourceId":"xxxx", "sid":"yyy", "serverResponse":{"status":5,"fileList":"zzzz.m3u8","fileListMode":"string","sliceStartTime":1606357122528} } 

[Serializable]
public class CloudRecordResponseModel
{
    public string resourceId;
    public string sid;
    public ServerResponseModel serverResponse;


    public static CloudRecordResponseModel CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<CloudRecordResponseModel>(jsonString);
    }

    public override string ToString()
    {
        string str = $"resourceId:{resourceId} sid:{sid} serverResponse:{serverResponse}";
        return str;
    }
}

[Serializable]
public class ServerResponseModel
{
    public int status;
    public string filelist;
    public string fileListMode;
    public long sliceStartTime;
    public string uploadingStatus;

    public override string ToString()
    {
        string str = $"status:{status} fileList:{filelist} fileListMode:{fileListMode} uploadingStatus:{uploadingStatus} sliceStartTime:{sliceStartTime}";
        return str;
    }
}