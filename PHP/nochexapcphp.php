<?php 
// Payment confirmation from http post 
  
$your_email = 'your_email@example.com';  // your merchant account email address

  	
function http_post($server, $port, $url, $vars) { 
    // get urlencoded vesion of $vars array 
    $urlencoded = ""; 
    foreach ($vars as $Index => $Value) // loop round variables and encode them to be used in query
    $urlencoded .= urlencode($Index ) . "=" . urlencode($Value) . "&"; 
    $urlencoded = substr($urlencoded,0,-1);   // returns portion of string, everything but last character

    $headers = "POST $url HTTP/1.0\r\n";  // headers to be sent to the server
    $headers .= "Content-Type: application/x-www-form-urlencoded\r\n";
	$headers .= "Host: secure.nochex.com\r\n";
    $headers .= "Content-Length: ". strlen($urlencoded) . "\r\n\r\n";  // length of the string
		
	//$hostip = @gethostbyname("www.nochex.com");

	/*echo "Nochex IP Address = " . $hostip . "<br/><br/>";
	
	echo "Headers = " . $headers . "";*/
	
    $fp = fsockopen($server, $port, $errno, $errstr, 20);  // returns file pointer
    if (!$fp) return "ERROR: fsockopen failed.\r\nError no: $errno - $errstr";  // if cannot open socket then display error message
	
    fputs($fp, $headers);  //writes to file pointer

    fputs($fp, $urlencoded);  
  
    $ret = ""; 
    while (!feof($fp)) $ret .= fgets($fp, 1024); // while it’s not the end of the file it will loop 
    fclose($fp);  // closes the connection
    return $ret; // array 
} 

// uncomment below to force a DECLINED response 
//$_POST['order_id'] = "1"; 

//HTTPS
$response = http_post("ssl://secure.nochex.com", 443, "/apc/apc.aspx", $_POST); 

// HTTP  
//$response = http_post("secure.nochex.com", 80, "/apc/apc.aspx", $_POST); 

// stores the response from the Nochex server 
$debug = "IP -> " . $_SERVER['REMOTE_ADDR'] ."\r\n\r\nPOST DATA:\r\n"; 
foreach($_POST as $Index => $Value) 
$debug .= "$Index -> $Value\r\n"; 
$debug .= "\r\nRESPONSE:\r\n$response"; 

echo $debug;
  
if (!strstr($response, "AUTHORISED")) {  // searches response to see if AUTHORISED is present if it isn’t a failure message is displayed
    $msg = "APC was not AUTHORISED.\r\n\r\n$debug";  // displays debug message
} 
else { 
    $msg = "APC was AUTHORISED."; // if AUTHORISED was found in the response then it was successful
    // whatever else you want to do 
} 
 
mail($your_email, "APC Debug", $msg);  // sends an email explaining whether APC was successful or not, the subject will be “APC Debug” but you can change this to whatever you want.
?>  
