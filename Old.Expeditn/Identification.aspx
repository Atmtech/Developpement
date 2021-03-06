﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Identification.aspx.cs" Inherits="ATMTECH.Expeditn.WebSite.Identification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="identification">
        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnConnecterLogin">
            <div class="loginConnecter">
                <div class="titre">
                    <asp:Label runat="server" ID="lblEntrerInformationLogin" Text="Information de connection"></asp:Label>
                </div>
                <div>
                    <asp:Label runat="server" ID="lblCourriel" Text="Courriel" CssClass="labelLogin"></asp:Label>
                </div>
                <div>
                    <asp:TextBox runat="server" ID="txtCourriel" CssClass="textBox" Width="300px"></asp:TextBox>
                </div>
                <div style="padding-top: 20px;">
                    <asp:Label runat="server" ID="lblMotDePasse" Text="Mot de passe" CssClass="labelLogin"></asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="txtMotDePasse" runat="server" CssClass="textBox" Width="300px" TextMode="Password"></asp:TextBox>
                </div>
                <div style="padding-top: 20px;">
                    <asp:Button runat="server" ID="btnOublieMotDePasse" Text="J'ai oublié mon mot de passe ?" CssClass="bouton" OnClick="btnOublieMotDePasseClick" />
                </div>

                <div style="padding-top: 20px;">
                    <asp:Button runat="server" ID="btnConnecterLogin" Text="Se connecter" CssClass="bouton" Width="200px" OnClick="btnConnecterLoginClick"></asp:Button>
                </div>
            </div>
        </asp:Panel>
        <div class="loginCreerLogin">
            <div class="titre">
                <asp:Label runat="server" ID="lblCreerCompte" Text="Creer compte"></asp:Label>
            </div>
            <div>
                <asp:Label runat="server" ID="lblPrenom" Text="Prénom" CssClass="labelLogin"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtPrenom" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
            </div>
            <div style="padding-top: 20px;">
                <asp:Label runat="server" ID="lblNom" Text="Nom" CssClass="labelLogin"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtNom" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
            </div>

            <div style="padding-top: 20px;">
                <asp:Label runat="server" ID="lblCourrielCreer" Text="Courriel" CssClass="labelLogin"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtCourrielCreer" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
            </div>

            <div style="padding-top: 20px;">
                <asp:Label runat="server" ID="lblMotDePasseCreer" Text="Mot de passe" CssClass="labelLogin"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtMotDePasseCreer" runat="server" CssClass="textBox" Width="300px" TextMode="Password"></asp:TextBox>
            </div>
            <div style="padding-top: 20px;">
                <asp:Label runat="server" ID="lblMotDePasseCreerConfirmation" Text="Confirmation" CssClass="labelLogin"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtMotDePasseCreerConfirmation" runat="server" CssClass="textBox" Width="300px" TextMode="Password"></asp:TextBox>
            </div>

            <div style="padding-top: 20px;">
                <asp:Button runat="server" ID="btnCreerLogin" Text="Créer" CssClass="bouton" Width="200px" OnClick="btnCreerLoginClick"></asp:Button>
            </div>


        </div>

        <div style="clear: both;"></div>
    </div>
</asp:Content>
