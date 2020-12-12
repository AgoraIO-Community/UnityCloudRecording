using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class AgoraTest : MonoBehaviour
{
    public string AppID;
    public string ChannelName;

    [SerializeField]
    GameObject CloudRecordingObject = null;

    VideoSurface myView;
    VideoSurface remoteView;

    IRtcEngine mRtcEngine;

    private void Awake()
    {
        SetupUI();
    }

    private void Start()
    {
        SetupAgora();
        CloudRecordingObject.SetActive(false);
    }

    void SetupUI()
    {
        GameObject go = GameObject.Find("MyView");
        myView = go.AddComponent<VideoSurface>();

        go = GameObject.Find("LeaveButton");
        go?.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideo();
            mRtcEngine.DisableVideoObserver();
        });

        go = GameObject.Find("JoinButton");
        go?.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            mRtcEngine.EnableVideo();
            mRtcEngine.EnableVideoObserver();
            myView.SetEnable(true);
            mRtcEngine.JoinChannel(ChannelName, "", 0);
        });
    }

    void SetupAgora()
    {
        mRtcEngine = IRtcEngine.GetEngine(AppID);

        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        CloudRecordingObject.SetActive(true);
        CloudRecordController controller = CloudRecordingObject.GetComponent<CloudRecordController>();
        controller.SetChannel(ChannelName); 
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        myView.SetEnable(false);
        if (remoteView != null)
        {
            remoteView.SetEnable(false);
        }
    }

    void OnUserJoined(uint uid, int elapsed)
    {
        GameObject go = GameObject.Find("RemoteView");

        if (remoteView == null)
        {
            remoteView = go.AddComponent<VideoSurface>();
        }

        remoteView.SetForUser(uid);
        remoteView.SetEnable(true);
        remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        remoteView.SetGameFps(30);
    }

    void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        remoteView.SetEnable(false);
    }

    void UnloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }

    void OnApplicationQuit()
    {
        UnloadEngine();
    }
}
