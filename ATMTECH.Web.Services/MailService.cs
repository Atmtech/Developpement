﻿using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using ATMTECH.DAO;
using ATMTECH.Entities;
using ATMTECH.Web.Services.Base;
using ATMTECH.Web.Services.Interface;

namespace ATMTECH.Web.Services
{
    public class MailService : BaseService, IMailService
    {
        public IParameterService ParameterService { get; set; }
        public IMessageService MessageService { get; set; }

        public bool SendEmail(string to, string from, string subject, string body)
        {
            return ParameterService.GetValue("Environment") != "PROD" ? SendDeveloppement(subject, body, null, string.Empty) : SendProduction(to, from, subject, body, null, string.Empty);
        }

        public bool SendEmail(string to, string from, string subject, string body, Stream file, string fileName)
        {
            return ParameterService.GetValue("Environment") != "PROD" ? SendDeveloppement(subject, body, file, fileName) : SendProduction(to, from, subject, body, file, fileName);
        }

        private bool SendProduction(string to, string from, string subject, string body, Stream file, string fileName)
        {
            return Send(from, to, subject, body, file, fileName);
        }
        private bool SendDeveloppement(string subject, string body, Stream file, string fileName)
        {
            return true;
        }

        private bool Send(string from, string to, string subject, string body, Stream file, string fileName)
        {
            if (ParameterService.GetValue("SendMail") == "1")
            {

                if (string.IsNullOrEmpty(to))
                {
                    MessageService.ThrowMessage(Common.ErrorCode.ADM_NO_EMAIL_TO);
                    return false;
                }
                
                SmtpClient client = new SmtpClient(ParameterService.GetValue("SmtpServer"),
                                                   Convert.ToInt32(ParameterService.GetValue("SmtpServerPort"))) { EnableSsl = true };
                MailAddress fromx = new MailAddress(from, "");
                MailAddress tox = new MailAddress(to, "");
                string subjectFormat = Utils.Web.Pages.RemoveHtmlTag(subject);
                MailMessage message = new MailMessage(fromx, tox) { IsBodyHtml = true, Body = body, Subject = subjectFormat };
                NetworkCredential myCreds = new NetworkCredential(ParameterService.GetValue("SmtpServerLogin"), ParameterService.GetValue("SmtpServerPassword"), "");
                client.Credentials = myCreds;

                if (file != null)
                {
                    Attachment data = new Attachment(file, fileName);
                    message.Attachments.Add(data);
                }

                try
                {
                    client.Send(message);
                }
                catch (System.Exception exception)
                {
                    DAOLogException daoLogException = new DAOLogException();
                    LogException logException = new LogException();
                    logException.InnerId = "INTERNAL";
                    logException.Page = Utils.Web.Pages.GetCurrentUrl() + Utils.Web.Pages.GetCurrentPage();
                    logException.Description = exception.Message + " => SendMail " + client.Host + " " + client.Port + " " + myCreds.UserName + " " + myCreds.Password;
                    logException.StackTrace = exception.StackTrace;
                    daoLogException.Save(logException);
                    return false;
                }

            }
            return true;
        }
    }
}
