<?php
require_once("config.php");

$Channel = $_POST["channel"];

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "https://api.agora.io/v1/apps/$AppID/cloud_recording/acquire",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "POST",
  CURLOPT_POSTFIELDS =>"{\n  \"cname\": \"$Channel\",\n  \"uid\": \"$RecUID\",\n  \"clientRequest\":{\n  }\n}",
  CURLOPT_HTTPHEADER => array(
    "Content-Type: application/json",
    "Authorization: Basic $AuthSecret"
  ),
));

$response = curl_exec($curl);

curl_close($curl);
echo $response;

/**
*  Returns json {"resourceId":"xxxx"}
*/
