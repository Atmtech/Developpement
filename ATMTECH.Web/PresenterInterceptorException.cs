﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ATMTECH.Common.Context;
using ATMTECH.Common.Utils.Web;
using ATMTECH.DAO;
using ATMTECH.Entities;
using ATMTECH.Exception;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace ATMTECH.Web
{
    public class PresenterInterceptorException : IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.DeclaringType.Name.Contains("Presenter"))
            {
                IMethodReturn result = getNext()(input, getNext);
                if (result.Exception != null)
                {
                    ManageException(input, result);
                }
                return result;
            }
            else
            {
                return getNext()(input, getNext);
            }
        }

        private void ManageException(IMethodInvocation input, IMethodReturn result)
        {
            try
            {

                if (result.Exception is BaseException)
                {
                    CallShowMessage(input.Target, (BaseException)result.Exception);
                }
                else
                {
                    var st = new StackTrace(result.Exception, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();

                    string message = result.Exception.Message;
                    Message messageEx = new Message() { Description = message };
                    BaseException exception = new BaseException(messageEx);
                    LogException logException = new LogException()
                                                    {
                                                        Description = message + " => PresenterInterceptor",
                                                        InnerId = "INTERNAL",
                                                        Page =
                                                            Pages.GetCurrentUrl() +
                                                            Pages.GetCurrentPage(),
                                                        StackTrace = result.Exception.StackTrace
                                                    };

                    if (ContextSessionManager.Session["Internal_LoggedUser"] != null)
                    {
                        User user = (User)ContextSessionManager.Session["Internal_LoggedUser"];
                        logException.User = user;
                    }

                    DAOLogException daoLogException = new DAOLogException();
                    daoLogException.Save(logException);

                    CallShowMessage(input.Target, exception);
                }
            }
            catch (System.Exception ex)
            {
                LogFile(ex);
            }

            result.Exception = null;
        }

        public void LogFile(System.Exception exception)
        {
            //HttpApplication server = new HttpApplication();
            
            //StreamWriter log = !System.IO.File.Exists(server.Server.MapPath("Errorlogfile.log")) ? new StreamWriter(server.Server.MapPath("Errorlogfile.log")) : System.IO.File.AppendText(server.Server.MapPath("Errorlogfile.log"));
            //log.WriteLine("==============================================================================");
            //log.WriteLine("Heure:" + DateTime.Now);
            //log.WriteLine("Exception:" + exception.Message);
            //log.WriteLine("Stack:" + exception.StackTrace);
            //log.WriteLine("==============================================================================");
            //log.Close();
        }


        private void CallShowMessage(object instancePresenter, BaseException exception)
        {
            Type typePresenter = instancePresenter.GetType();
            PropertyInfo typeInstanceView = typePresenter.GetProperty("View");

            object instanceView = typeInstanceView.GetValue(instancePresenter, null);
            Type typeView = instanceView.GetType();

            MethodInfo methode = typeView.GetMethod("ShowMessage", new[] { typeof(Message) });
            if (methode != null)
            {
                Message message = new Message() { InnerId = exception.InnerId, Description = exception.DisplayMessage, MessageType = exception.MessageType };
                methode.Invoke(instanceView, new object[] { message });
            }
            else
            {
                throw exception;
            }
        }


        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
