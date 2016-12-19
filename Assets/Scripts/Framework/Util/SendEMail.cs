//// exam
//// SendEMail.Send("보낼 메일주소", "메일내용");

//#if !UNITY_WEBPLAYER

//using UnityEngine;
//using System;
//using System.Net;
//using System.Net.Mail;
//using System.Net.Security;
//using System.Collections;
//using System.Security.Cryptography.X509Certificates;

//public class SendEMail
//{
//    static public void Send(string mailAddress, string mailDesc)
//    {
//        MailMessage mail = new MailMessage();

//        //os.
//        string osString = "PC";
//        #if UNITY_OIS && !UNITY_EDITOR
//            osString = "Ios";
//        #elif UNITY_ANDROID && !UNITY_EDITOR
//            osString = "Android";
//        #endif

//        string mailBody = string.Format("OS :{0}" + Environment.NewLine + Environment.NewLine +
//                                        "[ Mail Desc ]" + Environment.NewLine + Environment.NewLine + 
//                                        "{1}", osString, mailDesc);

//        mail.From = new MailAddress("보내는사람 메일주소");
//        mail.To.Add(mailAddress);
//        mail.Subject = "메일 제목";

//        // 보내는 메일주소의 smtp 값을 적어주시면 됩니다.
//        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
//        smtpServer.Port = 587;
//        smtpServer.Credentials = new System.Net.NetworkCredential("보내는사람 메일주소", "보내는사람 메일 비밀번호") as ICredentialsByHost;
//        smtpServer.EnableSsl = true;
//        ServicePointManager.ServerCertificateValidationCallback =
//            delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
//            {
//                return true;
//            };
//        smtpServer.Send(mail);
//    }
//}


//#endif