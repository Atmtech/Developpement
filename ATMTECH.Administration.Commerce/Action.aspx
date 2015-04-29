﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Action.aspx.cs" Inherits="ATMTECH.Administration.Commerce.Action" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="pnlConfirmerCommande" Visible="false">
        <div class="titrePage">
            Confirmer la commande
        </div>
        Numéro de commande
        <atmtech:Numeric runat="server" ID="txtNoCommandeConfirmer" NoDecimal="True"></atmtech:Numeric>
        <asp:Button ID="btnConfirmerCommande" runat="server" Text="Confirmer la commande" CssClass="bouton" OnClick="btnConfirmerCommandeClick" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlAjusterCommande" Visible="false">
        <div class="titrePage">
            Ajuster une commande
        </div>
        Numéro de commande
        <atmtech:Numeric runat="server" ID="txtNoCommandeAjustement" NoDecimal="true"></atmtech:Numeric>
        <asp:Button ID="btnAfficherCommandeAjustement" runat="server" Text="Afficher commande à ajuster" CssClass="bouton" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlRestaureCopie" Visible="false">
        <div class="titrePage">
            Restaurer une copie de sauvegarde
        </div>
        Copie:
        <asp:DropDownList runat="server" ID="ddlListeCopieSauvegarde" AutoPostBack="True" />
        <br />
        <br />
        <asp:Button runat="server" ID="btnRestaurerCopieSauvegarde" Text="Restaurer la copie de sauvegarde" OnClick="btnRestaurerCopieSauvegardeClick" CssClass="bouton" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEditionCourriel" Visible="False">
        <div class="titrePage">
            Éditer un courriel
        </div>
        Courriel:
        <asp:DropDownList runat="server" ID="ddlListeCourriel" />
        <asp:Button runat="server" ID="btnAfficherCourriel" Text="Afficher le courriel" OnClick="btnAfficherCourrielClick" CssClass="bouton" /><br />
        <br/>
        Code: <asp:TextBox runat="server" ID="txtCode" Enabled="False" Width="100%"></asp:TextBox>
        <br/>
        <table style="width:100%">
            <tr>
                <td style="font-weight: bold;text-transform: uppercase;">Français</td>
                <td style="font-weight: bold;text-transform: uppercase;">Anglais</td>
            </tr>
            <tr>
                <td>Sujet:<br />
                    <asp:TextBox runat="server" ID="txtSujetFr" Width="100%"></asp:TextBox>
                    Corps:<br />
                    <asp:TextBox runat="server" ID="txtCorpsFr" Width="100%" Rows="15" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:Button runat="server" ID="btnApercuCourrielFrancais" OnClick="btnApercuCourrielFrancaisClick" CssClass="bouton" Text="Apercu" />
                </td>
                <td>Sujet:<br />
                    <asp:TextBox runat="server" ID="txtSujetEn" Width="100%"></asp:TextBox>
                    Corps:<br />
                    <asp:TextBox runat="server" ID="txtCorpsEn" Width="100%" Rows="15"  TextMode="MultiLine"></asp:TextBox><br />
                    <asp:Button runat="server" ID="btnApercuCourrielAnglais" OnClick="btnApercuCourrielAnglaisClick" CssClass="bouton"  Text="Apercu"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblApercuCourrielFrancais" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApercuCourrielAnglais" Text=""></asp:Label>
                </td>
            </tr>
        </table>

        <asp:Button runat="server" ID="btnSauvegarderCourriel" Text="Enregistrer" OnClick="btnSauvegarderCourrielClick" CssClass="bouton" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlAppliquerPourcentage" Visible="False">
        <div class="titrePage">
            Appliquer un pourcentage aux prix unitaire de chaque produit versus prix coutant
        </div>
        Pourcentage:<atmtech:Numeric runat="server" ID="txtPourcentage" NoDecimal="True" />
        %<br />
        <br />
        <asp:Button runat="server" ID="btnAppliquerPourcentage" Text="Appliquer pourcentage" OnClick="btnAppliquerPourcentageClick" CssClass="bouton" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlVerifierInventaire" Visible="false">
        <div class="titrePage">
            Vérifier un inventaire
        </div>

        Identification produit:
        <asp:TextBox runat="server" ID="txtIdentProduit"></asp:TextBox>
        Ex: (12000)<br />
        Taille:
        <asp:TextBox runat="server" ID="txtTaille"></asp:TextBox>
        Ex: (2XL)<br />
        Couleur en anglais:
        <asp:TextBox runat="server" ID="txtCouleurAnglais"></asp:TextBox>
        Ex: (Cardinal Red)<br />
        <asp:Button runat="server" ID="btnVerifierInvenaire" Text="Vérifier inventaire" OnClick="btnVerifierInvenaireClick" CssClass="bouton" />
        <br />
        <br />
        <div>
            Nombre en inventaire:
            <asp:Label runat="server" ID="lblNombreEnInventaire"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlValiderPourPaypal" Visible="False">
        <div class="titrePage">
            Sortir la liste des commandes finalisé pour valider avec paypal
        </div>
        <table>
            <tr>
                <td>Date début:</td>
                <td>
                    <atmtech:DatePicker runat="server" ID="txtDateDepart" />
                </td>
            </tr>
            <tr>
                <td>Date fin: </td>
                <td>
                    <atmtech:DatePicker runat="server" ID="txtDateFin" />
                </td>
            </tr>
        </table>
        <asp:Button runat="server" ID="btnValiderPaypal" Text="Valider" OnClick="btnValiderPaypalClick" CssClass="bouton" />
        <asp:Label runat="server" ID="lblRetourValidationPaypal"></asp:Label>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlEnvoiCourriel" Visible="False">
        <div class="titrePage">
            Tester l'envoi de courriel
        </div>
        Courriel:
        <asp:TextBox runat="server" ID="txtCourriel" Text="atmtech.vincent@gmail.com"></asp:TextBox><br />
        <br />
        <asp:Button runat="server" ID="btnEnvoiCourriel" Text="Envoyer courriel" OnClick="btnEnvoiCourrielClick" CssClass="bouton" />
        <br />
        <asp:Label runat="server" ID="lblStatutEnvoiCourriel"></asp:Label>
    </asp:Panel>
</asp:Content>
