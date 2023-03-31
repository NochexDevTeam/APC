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
                WebRequest request = WebRequest.Create("https://secure.nochex.com/apc/apc.aspx");
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
 
                    MailMessage mail = new MailMessage("apc@nochex.com", "your_email@example.com");
                    SmtpClient client = new SmtpClient();
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "mail.nochex.com";
               
                if (responseFromServer == "AUTHORISED")
                {
                   //lbltransaction.Text = "Authorised";
                    mail.Subject = "Callback was " + responseFromServer;
                    mail.Body = "Callback Response was: " + responseFromServer + ", for order:" + Request.Form["order_id"] + ". amount:" + Request.Form["amount"];
                    client.Send(mail);
 
                }
                else // If the Callback response is DECLINED email results and investigate
                {
                    //lbltransaction.Text = "Declined";                  
                    mail.Subject = "Callback was " + responseFromServer; 
                    mail.Body = "Callback Response was: " + responseFromServer + " for order:" + Request.Form["order_id"] + ", amount:" + Request.Form["amount"];
                    client.Send(mail);
 
                }
 
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
         
    }
}
