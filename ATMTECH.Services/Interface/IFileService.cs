﻿using System.Collections.Generic;
using System.Web;
using ATMTECH.Entities;

namespace ATMTECH.Services.Interface
{
    public interface IFileService
    {
        File GetFile(File file);
        IList<File> GetAllFile();
        void DeleteFile(File file);
        void ResizeFile(string directory);
        int SaveFile(File file);
        int SaveFile(HttpPostedFile httpPostedFile, string type, string rootImagePath);
        File GetFile(int id);
        IList<File> GetAllFile(string rootImagePath);
    }
}