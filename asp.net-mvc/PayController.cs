using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jr.Web.Areas.Profile.Controllers
{
    [AllowAnonymous]
    public class PayController : Controller
    {

        [HttpPost]
        public void NoChexCallback()
        {
            var form = Request.Form;
            try
            {
                var formData = GetFormKeyValuePairs(form);
                var responseFromServer = SendFormDataToNoChexAndGetResponse(formData.ToString());

                if (responseFromServer == "AUTHORISED")
                {
                    var orderId = form["order_id"].ToString();
                    // TODO: process authorised response
                }
                else
                {
                    // TODO: log the issue and process declined response
                }
            }
            catch (Exception e)
            {
                // TODO: log the issue
            }
        }

        private static StringBuilder GetFormKeyValuePairs(IFormCollection form)
        {
            var formData = new StringBuilder();
            foreach (var key in form.Keys)
            {
                var value = form[key];
                formData.Append($"{key}={value}&");
            }
            if (formData.Length > 0)
                formData.Remove(formData.Length - 1, 1);
            return formData;
        }

        private static string SendFormDataToNoChexAndGetResponse(string formData)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.nochex.com/apcnet/apc.aspx");
            request.Method = "POST";
            var byteArray = Encoding.UTF8.GetBytes(formData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            using (var requestDataStream = request.GetRequestStream())
            {
                requestDataStream.Write(byteArray, 0, byteArray.Length);
            }

            var responseFromServer = string.Empty;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    if (dataStream == null) return responseFromServer;
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                    }
                }
            }
            return responseFromServer;
        }
    }
}