<?php

require_once("config.php");

$curl = curl_init();

// Receive ResourceID and sid from client

$ResourceID = $_POST["resourceId"];
$Channel = $_POST["channel"];
$sid = $_POST["sid"];

curl_setopt_array($curl, array(
  CURLOPT_URL => "https://api.agora.io/v1/apps/$AppID/cloud_recording/resourceid/$ResourceID/sid/$sid/mode/mix/stop",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "POST",
  CURLOPT_POSTFIELDS =>"{\n  \"cname\": \"$Channel\",\n  \"uid\": \"$RecUID\",\n  \"clientRequest\":{\n  }\n}",
  CURLOPT_HTTPHEADER => array(
    "Content-Type: application/json;charset=utf-8",
    "Authorization: Basic $AuthSecret"
  ),
));

$response = curl_exec($curl);

curl_close($curl);
echo $response;

/**
*  Returns json {"resourceId":"xxxx", "sid":"yyy", "serverResponse":{"fileListMode":"string","fileList":"zzzz.m3u8","uploadingStatus":"uploaded"}}
*   m3u8 file name :=  <sid>_<channel>.m3u8
*/
