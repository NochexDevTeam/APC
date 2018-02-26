using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Collections.Specialized;

public partial class nochexapccsharp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	

                NameValueCollection nvc = Request.Form;
                string postdetails = nvc.ToString();
 
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create("https://www.nochex.com/apcnet/apc.aspx");
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
 
 
                byte[] byteArray = Encoding.UTF8.GetBytes(postdetails);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
 
                // Get the response.
                WebResponse response = request.GetResponse();
 
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
 
 
               
                if (responseFromServer == "AUTHORISED")
                {
                   //lbltransaction.Text = "Authorised";
                    MailMessage mail = new MailMessage("apc@nochex.com", "james.lugton@nochex.com");
                    SmtpClient client = new SmtpClient();
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "mail.nochex.com";
                    mail.Subject = "Callback was " + responseFromServer;
 
                    mail.Body = "Callback Response was: " + responseFromServer + ", for order:" + Request.Form["order_id"] + ". amount:" + Request.Form["amount"];
 
                    client.Send(mail);
 
                }
                else // If the Callback response is DECLINED email results and investigate
                {
                    //lbltransaction.Text = "Declined";
                    MailMessage mail = new MailMessage("apc@nochex.com", "james.lugton@nochex.com");
                    SmtpClient client = new SmtpClient();
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "mail.nochex.com";
                    mail.Subject = "Callback was " + responseFromServer;
 
                    mail.Body = "Callback Response was: " + responseFromServer + " for order:" + Request.Form["order_id"] + ", amount:" + Request.Form["amount"];
 
                    client.Send(mail);
 
                }
 
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
 
 
          

	
	
//			NameValueCollection nvc = Request.Form;
//			string postdetails = nvc.ToString();
//			
//			
//		 // Create a request using a URL that can receive a post. 
//            WebRequest request = WebRequest.Create("https://www.nochex.com/apcnet/apc.aspx");
//            // Set the Method property of the request to POST.
//            request.Method = "POST";
//            // Create POST data and convert it to a byte array.
//		   
//            byte[] byteArray = Encoding.UTF8.GetBytes(postdetails);
//            // Set the ContentType property of the WebRequest.
//            request.ContentType = "application/x-www-form-urlencoded";
//            // Set the ContentLength property of the WebRequest.
//            request.ContentLength = byteArray.Length;
//            // Get the request stream.
//            Stream dataStream = request.GetRequestStream();
//            // Write the data to the request stream.
//            dataStream.Write(byteArray, 0, byteArray.Length);
//            // Close the Stream object.
//			dataStream.Close();
//		 
//            // Get the response.
//            WebResponse response = request.GetResponse();
//
//            // Get the stream containing content returned by the server.
//            dataStream = response.GetResponseStream();
//            // Open the stream using a StreamReader for easy access.
//            StreamReader reader = new StreamReader(dataStream);
//            // Read the content.
//            string responseFromServer = reader.ReadToEnd();
//           
//		   
//            
//		if (responseFromServer == "AUTHORISED")
//				{
//
//				MailMessage mail = new MailMessage("james.lugton@nochex.com", "james.lugton@nochex.com");
//				SmtpClient client = new SmtpClient();
//				client.Port = 25;
//				client.DeliveryMethod = SmtpDeliveryMethod.Network;
//				client.UseDefaultCredentials = false;
//				client.Host = "mail.nochex.com";
//				mail.Subject = "APC was " + responseFromServer;
//				
//				mail.Body = "APC Response was: " + responseFromServer + ", for order:" + Request.Form["order_id"] + ". amount:" + Request.Form["amount"] + ". This was a " + Request.Form["status"] + " transaction";
//				
//				client.Send(mail);			
//	
//		}
//        else // If the APC response is DECLINED email results and investigate
//        {
//				
//			MailMessage mail = new MailMessage("james.lugton@nochex.com", "james.lugton@nochex.com");
//			SmtpClient client = new SmtpClient();
//			client.Port = 25;
//			client.DeliveryMethod = SmtpDeliveryMethod.Network;
//			client.UseDefaultCredentials = false;
//			client.Host = "mail.nochex.com";
//			mail.Subject = "APC was " + responseFromServer;
//			
//			mail.Body = "APC Response was: " + responseFromServer + " for order:" + Request.Form["order_id"] + ", amount:" + Request.Form["amount"] + ". This was a " + Request.Form["status"] + " transaction";
//			 
//			client.Send(mail);
//		
//		}
//			
//			// Clean up the streams.
//            reader.Close();
//            dataStream.Close();
//            response.Close();
    }
}
