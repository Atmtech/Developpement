﻿using System.Collections.Generic;
using ATMTECH.Web.Services.DTO;

namespace ATMTECH.Web.Services.Interface
{
    public interface INavigationService
    {
        void Redirect(string page);
        IList<QueryString> GetQueryString();
        string GetQueryStringValue(string key);
        void Redirect(string page, IList<QueryString> queryString);
        void Refresh();
        void Refresh(IList<QueryString> queryStrings);
        CountryIp GetInformationIpInfoDb();
    }
}