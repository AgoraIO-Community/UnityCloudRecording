<?php
require_once("config.php");

$curl = curl_init();


// Receive ResourceID from client
$ResourceID = $_POST["resourceId"];
$Channel = $_POST["channel"];


curl_setopt_array($curl, array(
  CURLOPT_URL => "https://api.agora.io/v1/apps/$AppID/cloud_recording/resourceid/$ResourceID/mode/mix/start",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "POST",
  CURLOPT_POSTFIELDS => '{
	"cname":"'.$Channel.'",
	"uid":"'.$RecUID.'",
	"clientRequest":{
		"recordingConfig":{
			"maxIdleTime":120,
			"streamTypes":2,
			"audioProfile":1,
			"channelType":1,
			"videoStreamType":0,
			"transcodingConfig":{
   			  "width":720,
  			  "height":640,
  			  "fps":30,
  			  "bitrate":2000,
  			  "mixedVideoLayout":1,
  			  "maxResolutionUid":"1"
			}
		},
		"storageConfig":{
			"vendor":1,
			"region":'.$S3Region.',
			"bucket":"'.$S3Bucket.'",
			"accessKey":"'.$S3AccessKey.'",
			"secretKey":"'.$S3SecretKey.'"
		}	
	}
  }',
  CURLOPT_HTTPHEADER => array(
    "Content-Type: application/json",
    "Authorization: Basic $AuthSecret"
  ),
));

$response = curl_exec($curl);

curl_close($curl);
echo $response;
/**
*  Returns json {"resourceId":"xxxx", "sid":"yyy"}
*/
