﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ATMTECH.Entities
{
    [Serializable]
    public class BaseEntity : ICloneable
    {
        public BaseEntity()
        {

        }



        public const string ID = "Id";
        public const string LANGUAGE = "Language";
        public const string IS_ACTIVE = "IsActive";
        public const string DESCRIPTION = "Description";
        public const string ORDER_ID = "OrderId";
        public const string SEARCH = "Search";
        public const string COMBOBOX_DESCRIPTION = "ComboboxDescription";
        public const string DATE_CREATED = "DateCreated";
        public const string DATE_MODIFIED = "DateModified";

        [Category("UniqueKey")]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public string Language { get; set; }
        public int OrderId { get; set; }
        public string Search { get; set; }
        public string ComboboxDescription { get; set; }

        public object Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }

    }
}