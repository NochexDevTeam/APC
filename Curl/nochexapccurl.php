<?php

// Get the POST information from Nochex server
$postvars = http_build_query($_POST);
ini_set("SMTP","mail.nochex.com" ); 
$header = "From: apc@nochex.com";

// Set parameters for the email
$to = 'your_email@nochex.com';
$url = "https://www.nochex.com/apcnet/apc.aspx";

// Curl code to post variables back
$ch = curl_init(); // Initialise the curl tranfer
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_VERBOSE, true);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $postvars); // Set POST fields
curl_setopt($ch, CURLOPT_HTTPHEADER, "Host: www.nochex.com");
curl_setopt($ch, CURLOPT_POSTFIELDSIZE, 0); 
curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1);
curl_setopt($ch, CURLOPT_TIMEOUT, 60); // set connection time out variable - 60 seconds	
//curl_setopt ($ch, CURLOPT_SSLVERSION, CURL_SSLVERSION_TLSv1); // set openSSL version variable to CURL_SSLVERSION_TLSv1
$output = curl_exec($ch); // Post back
curl_close($ch);

// Put the variables in a printable format for the email
$debug = "IP -> " . $_SERVER['REMOTE_ADDR'] ."\r\n\r\nPOST DATA:\r\n"; 
foreach($_POST as $Index => $Value) 
$debug .= "$Index -> $Value\r\n"; 
$debug .= "\r\nRESPONSE:\r\n$output";
 
//If statement
if (!strstr($output, "AUTHORISED")) {  // searches response to see if AUTHORISED is present if it isn’t a failure message is displayed
    $msg = "APC was not AUTHORISED.\r\n\r\n$debug";  // displays debug message
} 
else { 
    $msg = "APC was AUTHORISED.\r\n\r\n$debug"; // if AUTHORISED was found in the response then it was successful
    // whatever else you want to do 
}

//Email the response
mail($to, 'APC - After If statement', $msg, $header);
?>
