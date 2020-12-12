using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CloudRecordController : MonoBehaviour
{
    [SerializeField]
    string ServerURL = default;
    [SerializeField]
    Button recordButton = default;
    [SerializeField]
    Button queryButton = default;


    string ChannelName { get; set; }
    string ResourceId { get; set; }
    string SID { get; set; }

    bool IsRecording { get; set; }

    private void Awake()
    {
        recordButton.onClick.AddListener(HandleStartStop);
        queryButton.onClick.AddListener(HandleQueryButton);
        queryButton.gameObject.SetActive(false);
        RestoreRecordState();
    }

    public void SetChannel(string channel)
    {
        ChannelName = channel;
    }

    #region -- UI Controls --

    void RestoreRecordState()
    {
        ResourceId = PlayerPrefs.GetString("ResourceId");
        SID = PlayerPrefs.GetString("SID");
        if (!string.IsNullOrEmpty(ResourceId) && !string.IsNullOrEmpty(SID))
        {
            // recording as going on
            SetRecordUI(true);
        }
        else
        {
            SetRecordUI(false);
        }
    }

    void SetRecordUI(bool isRecording)
    {
        IsRecording = isRecording;
        recordButton.GetComponentInChildren<Text>().text = isRecording ? "STOP\nRecording" : "START\nRecording";
        queryButton.gameObject.SetActive(recordButton.isActiveAndEnabled);
    }

    void HandleStartStop()
    {
        if (IsRecording)
        {
            Debug.LogWarning("Stopping cloud recording....");
            // signal to stop
            StartCoroutine(_Stop(HandleStopResult));
        }
        else
        {
            // signal to acquire, start will run if acquired successfully
            Debug.LogWarning("Starting cloud recording....");
            StartCoroutine(_Acquire(HandleAcquireResult));
        }
    }

    void HandleQueryButton()
    {
        StartCoroutine(_Query(HandleQueryResult));
    }

    #endregion

  
    #region -- Server API Callers --

    IEnumerator _Acquire(System.Action<CloudRecordResponseModel> onComplete)
    {
        string url = ServerURL + "/acquire.php";
        Dictionary<string, string> postDict = new Dictionary<string, string>
        {
            {"channel", ChannelName }
        };
        UnityWebRequest request = UnityWebRequest.Post(url, postDict);
        yield return request.SendWebRequest();
        Debug.Log("request error:" + request.error);
        Debug.Log("request responseCode:" + request.responseCode);
        Debug.Log("request responseText:" + request.downloadHandler.text);

        CloudRecordResponseModel responseModel = CloudRecordResponseModel.CreateFromJSON(request.downloadHandler.text);
        Debug.Log($"AcquireResponse: {responseModel}");
        onComplete(responseModel);
    }

    IEnumerator _Start(string resourceId, System.Action<CloudRecordResponseModel> onComplete)
    {
        string url = ServerURL + "/start.php";
        Dictionary<string, string> postDict = new Dictionary<string, string>
        {
            {"channel", ChannelName },
            {"resourceId", resourceId },
        };
        UnityWebRequest request = UnityWebRequest.Post(url, postDict);
        yield return request.SendWebRequest();
        Debug.Log("request error:" + request.error);
        Debug.Log("request responseCode:" + request.responseCode);
        Debug.Log("request responseText:" + request.downloadHandler.text);

        CloudRecordResponseModel responseModel = CloudRecordResponseModel.CreateFromJSON(request.downloadHandler.text);
        Debug.Log($"StartResponse: {responseModel}");
        onComplete(responseModel);
    }

    IEnumerator _Stop(System.Action<CloudRecordResponseModel> onComplete)
    {
        string url = ServerURL + "/stop.php";
        Dictionary<string, string> postDict = new Dictionary<string, string>
        {
            {"channel", ChannelName },
            {"resourceId", ResourceId },
            {"sid", SID }
        };
        UnityWebRequest request = UnityWebRequest.Post(url, postDict);
        yield return request.SendWebRequest();
        Debug.Log("request error:" + request.error);
        Debug.Log("request responseCode:" + request.responseCode);
        Debug.Log("request responseText:" + request.downloadHandler.text);

        CloudRecordResponseModel responseModel = CloudRecordResponseModel.CreateFromJSON(request.downloadHandler.text);
        Debug.Log($"StopResponse: {responseModel}");
        onComplete(responseModel);
    }


    IEnumerator _Query(System.Action<CloudRecordResponseModel> onComplete)
    {
        string url = ServerURL + "/query.php";
        Dictionary<string, string> postDict = new Dictionary<string, string>
        {
            {"resourceId", ResourceId },
            {"sid", SID }
        };
        UnityWebRequest request = UnityWebRequest.Post(url, postDict);
        yield return request.SendWebRequest();
        Debug.Log("request error:" + request.error);
        Debug.Log("request responseCode:" + request.responseCode);
        Debug.Log("request responseText:" + request.downloadHandler.text);

        CloudRecordResponseModel responseModel = CloudRecordResponseModel.CreateFromJSON(request.downloadHandler.text);
        Debug.Log($"StopResponse: {responseModel}");
        onComplete(responseModel);
    }
    #endregion

    #region -- Server Response Handlers --

    void HandleAcquireResult(CloudRecordResponseModel response)
    {
        StartCoroutine(_Start(response.resourceId, HandleStartResult));
    }

    void HandleStartResult(CloudRecordResponseModel response)
    {
        ResourceId = response.resourceId;
        SID = response.sid;

        PlayerPrefs.SetString("ResourceId", ResourceId);
        PlayerPrefs.SetString("SID", SID);
        PlayerPrefs.Save();

        SetRecordUI(true);
    }

    void HandleStopResult(CloudRecordResponseModel response)
    {
        Debug.Log("serverResponse:" + response?.serverResponse);
        PlayerPrefs.DeleteKey("ResourceId");
        PlayerPrefs.DeleteKey("SID");
        PlayerPrefs.Save();
        SetRecordUI(false);
    }

    void HandleQueryResult(CloudRecordResponseModel response)
    {
        Debug.Log("serverResponse:" + response.serverResponse);
    }
    #endregion

}
