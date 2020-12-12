<?php
require_once("config.php");

$curl = curl_init();

// Receive ResourceID and sid from client
$ResourceID = $_POST["resourceId"];
$sid = $_POST["sid"];

$CURLOPT_URL = "https://api.agora.io/v1/apps/$AppID/cloud_recording/resourceid/$ResourceID/sid/$sid/mode/mix/query";

curl_setopt_array($curl, array(
  CURLOPT_URL => $CURLOPT_URL,
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "GET",
  CURLOPT_HTTPHEADER => array(
    "Content-Type: application/json",
    "Authorization: Basic $AuthSecret"
  ),
));

$response = curl_exec($curl);

curl_close($curl);
echo $response;


