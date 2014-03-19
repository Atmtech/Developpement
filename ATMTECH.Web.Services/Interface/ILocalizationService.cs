﻿using System.Collections.Generic;
using System.Web.UI;

namespace ATMTECH.Web.Services.Interface
{
    public interface ILocalizationService
    {
        void Localize(IList<Control> Controls, string language);
        string Localize(string controlId, string currentLanguage);
        string CurrentLanguage { get; set; }
    }
}
