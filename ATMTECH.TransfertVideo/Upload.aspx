﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ATMTECH.TransfertVideo.Upload" %>

<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader, Version=4.0.0.0, Culture=neutral, PublicKeyToken=bc00d4b0e43ec38d" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="centerPanelAdmin">
        <div style="text-align: center">
            <h2>PLEASE UPLOAD YOUR MOVIE</h2>
            <div class="innerPanel">
                <h4>The maximum file size for a file is 300 mo</h4>
                <h4>Only file with these format: mp4,wmv,mpg</h4>
                <CuteWebUI:Uploader runat="server" ID="Uploader1" InsertText="UPLOAD YOUR MOVIE FILE" OnFileUploaded="Uploader_FileUploaded">
                    <ValidateOption AllowedFileExtensions="mp4,wmv,mpg" MaxSizeKB="300000" />
                </CuteWebUI:Uploader>
            </div>

            <h2>OR IF YOU HAVE A <img src="Images/Youtube.png" /> &nbsp;&nbsp;MOVIE
                    
            </h2>

            <div class="innerPanel">
                <div class="label">ENTER YOUR YOUTUBE URL</div>
                <asp:TextBox runat="server" Width="100%" ID="txtYoutube" CssClass="textBox"></asp:TextBox>
                <br />
                <br />
                <asp:Button runat="server" ID="btnSaveYoutube" OnClick="btnSaveYoutubeClick" Text="SAVE YOUTUBE URL" CssClass="bouton" />

            </div>

        </div>
    </div>
</asp:Content>