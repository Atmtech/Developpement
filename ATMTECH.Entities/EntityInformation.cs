﻿using System;
using System.Collections.Generic;

namespace ATMTECH.Entities
{
    [Serializable]
    public class EntityInformation : BaseEntity
    {
        public const string NAMESPACE = "NameSpace";
        public string PageTitle { get; set; }
        public string PageAspx { get; set; }
        public string NameSpace { get; set; }
        public IList<EntityProperty> EntityProperties { get; set; }
    }
}