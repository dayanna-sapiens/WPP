using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
//using WPP.Entities.Objects.Generales;

namespace WPP.Helpers
{
    public static class MailHelper
    {
        public static void sendMail(string str_to_address, string str_body, string str_subject)
        {
            string senderEmail = System.Configuration.ConfigurationManager.AppSettings.Get("SenderEmail");
            string senderName = System.Configuration.ConfigurationManager.AppSettings.Get("SenderName");
            string host = System.Configuration.ConfigurationManager.AppSettings.Get("Host");
            string username = System.Configuration.ConfigurationManager.AppSettings.Get("Username");
            string password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            bool useCredentials = true;//System.Configuration.ConfigurationManager.AppSettings.Get("UseCredentials");
            bool sendMail = true;//System.Configuration.ConfigurationManager.AppSettings.Get("UseCredentials");

            try
            {
                if (sendMail)
                {
                    //Create MailMessage Object
                    MailMessage email_msg = new MailMessage();

                    //Specifying From,Sender & Reply to address
                    email_msg.From = new MailAddress(senderEmail, senderName);
                    email_msg.Sender = new MailAddress(senderEmail, senderName);
                    email_msg.ReplyToList.Add(new MailAddress(senderEmail, senderName));
                    email_msg.IsBodyHtml = true;
                    if (str_to_address != null)
                    {
                        //The To Email id
                        string[] tos = str_to_address.Split(',');
                        foreach (string to in tos)
                        {
                            email_msg.To.Add(new MailAddress(to.Trim()));
                        }

                        email_msg.Subject = str_subject;
                        email_msg.Body = str_body;
                        email_msg.Priority = MailPriority.Normal;

                        //Create an object for SmtpClient class
                        SmtpClient mail_client = new SmtpClient();
                        mail_client.Host = host; //SMTP host
                        NetworkCredential network_cdr = new NetworkCredential();
                        if (useCredentials)
                        {
                            /////////////////////////////////////////////////////
                            network_cdr.UserName = username;
                            network_cdr.Password = password;
                            mail_client.UseDefaultCredentials = false;
                            mail_client.Credentials = network_cdr;
                        }
                        else
                        {
                            mail_client.UseDefaultCredentials = true;
                        }


                        //Now Send the message
                        mail_client.Send(email_msg);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}