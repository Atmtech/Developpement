﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="ImportExcel.aspx.cs" Inherits="ATMTECH.Administration.ImportExcel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Fichier excel: <asp:FileUpload ID="FileUpload1" runat="server" /><br/>
    Importer vers: <asp:DropDownList runat="server" ID="ddlTable"/><br/>
    <asp:Button runat="server" CssClass="button" ID="btnImport" OnClick="btnImportClick" Text="Importer le fichier" />
    <asp:Label runat="server" ID="lblFileImported" Text="Le fichier a été importé avec succès" Visible="False"></asp:Label>
</asp:Content>