﻿<%@ Page Title="" Language="C#" MasterPageFile="Default.Master" AutoEventWireup="true"
    CodeBehind="DataEditor.aspx.cs" Inherits="ATMTECH.Administration.DataEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        input.textBoxAvance, textarea.textBoxAvance
        {
            border: 1px solid gray;
            padding: 0 1px;
            color: #333333;
        }

        input.textBoxAvance, textarea, .dxeListBox, input.dxeEditArea
        {
            font-size: 12px;
        }
        .ui-widget
        {
            font-size: 12px;
           
        }
        .ui-widget-content
        {
            color: black;
        }
        .ui-combobox-toggle
        {
            height: 17px;width: 17px;
        }
        .ui-corner-right {
            border-top-right-radius: 1px;
            border-bottom-right-radius: 1px
        }
          .ui-corner-left {
            border-top-left-radius: 1px;
            border-bottom-left-radius: 1px
        }
         .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
              border:1px solid #d3d3d3;
             background: white;
          }


     
    </style>
    <atmtech:FenetreDialogue runat="server" ID="windowAssociateUser" t>
    </atmtech:FenetreDialogue>
    <div class="title">
        Pilotage des données ::
        <asp:Label runat="server" ID="lblTitle"></asp:Label>
    </div>
    <div style="border: solid 1px gray; padding: 5px 5px 5px 5px; background-color: whitesmoke;">
        <asp:Panel runat="server" ID="pnlheader">
            <fieldset style="padding: 7px; border-radius: 5px; -moz-border-radius: 5px;">
                <legend><b>Critère de recherche</b></legend>
                <asp:Panel runat="server" ID="pnlEnterprise" Visible="true">
                    <table cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 175px;">Enterprise:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList runat="server" ID="cboSelectionEntreprise" Width="400px" />
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
                <asp:Panel runat="server" ID="pnlSearch" Visible="True">
                    <table cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 175px;">Rechercher:
                            </td>
                            <td style="width: 200px;">
                                <atmtech:TextBoxAvance runat="server" ID="txtSearch" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSearch" OnClick="SearchClick" Text="Rechercher"
                                    CssClass="button" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
        </asp:Panel>
        <br />
        <fieldset style="padding: 7px; border-radius: 5px; -moz-border-radius: 5px;">
            <legend><b>Liste obtenu</b></legend>
            <div style="overflow: scroll; width: 1350px">
                <asp:GridView ID="grdData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    AllowPaging="True" AutoGenerateColumns="False" PageSize="10" OnRowCommand="RowCommandClick"
                    OnPageIndexChanging="PageIndexChanging" Font-Size="11px" EmptyDataText="Aucune données ..." OnRowDataBound="RowDataBound">
                    <Columns>
                        <asp:ButtonField CommandName="Inactive" ImageUrl="Images/supprimer.gif" Text="Supprimer"
                            CausesValidation="False" ButtonType="Image" />
                        <asp:ButtonField ImageUrl="Images/edition.png" CommandName="Edition" Text="Edition"
                            CausesValidation="False" ButtonType="Image" />
                        <asp:ButtonField ImageUrl="Images/Copy.png" CommandName="Copie" Text="Copie" CausesValidation="False"
                            ButtonType="Image" />
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </div>
        </fieldset>
        <br />
        <fieldset style="padding: 7px; border-radius: 5px; -moz-border-radius: 5px;">
            <legend><b>Éditions</b></legend>
            <asp:Panel runat="server" ID="pnlSaveDone" Visible="False">
                <div style="border: solid 1px black; color: green; padding: 10px 10px 10px 10px; background-color: white; margin-bottom: 5px;">
                    <asp:Label runat="server" ID="lblMessage" Text="Enregistrement effectué avec succès"></asp:Label>
                </div>
            </asp:Panel>
            <atmtech:Section runat="server" Libelle="Détails" EstOuvert="False" ID="sectionDetail">

                <div style="font-size: 12px; margin-bottom: 10px;">
                    <asp:PlaceHolder runat="server" ID="pnlControl"></asp:PlaceHolder>
                </div>
            </atmtech:Section>
            <div style="background-color: lightgray; border: solid 1px gray; padding: 5px 5px 5px 5px;">
                <asp:Button runat="server" ID="btnSave" Text="Enregistrer" OnClick="SaveClick" CausesValidation="False"
                    CssClass="button" Visible="False" />
                <asp:Button runat="server" ID="btnAnnuler" Text="Annuler" OnClick="CancelClick" CausesValidation="False"
                    CssClass="button" />
                <asp:Button runat="server" ID="btnAdd" OnClick="AddClick" Text="Ajouter" CausesValidation="False"
                    CssClass="button" />
                <asp:Button runat="server" ID="btnConfirm" OnClick="ConfirmOrderClick" Text="Confirmer la commande au consommateur"
                    CausesValidation="False" CssClass="button" Visible="False" />
                <asp:Panel runat="server" ID="pnlOrder" Visible="False">
                    <asp:Button runat="server" ID="btnDisplayOrder" OnClick="DisplayOrderClick" Text="Afficher la facture" CausesValidation="False" CssClass="button" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlAssociate" Visible="False">
                    <table>
                        <tr>
                            <td>Enterprise:
                            </td>
                            <td>
                                <atmtech:ComboBoxAvance runat="server" ID="cboEnterprise2" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnAssociateWithCustomer" OnClick="AssociateUserOpenWindowClick"
                                    Text="Associer cet utilisateur à cette entreprise" CausesValidation="False" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlCreateEnterpriseFrom" Visible="false">
                    <table>
                        <tr>
                            <td>Enterprise:
                            </td>
                            <td>
                                <atmtech:ComboBoxAvance runat="server" ID="cboEnterprise3" />
                            </td>
                            <td>Nouveau nom:<asp:TextBox runat="server" ID="txtNewName"></asp:TextBox></td>
                            <td>
                                <asp:Button runat="server" ID="btnCreateEnterpriseFrom" OnClick="btnCreateEnterpriseFromClick"
                                    Text="Créer une entreprise avec celle sélectionnée" CausesValidation="False" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlStockTemplate" Visible="False">
                    <table>
                        <tr>
                            <td>Modèle:</td>
                            <td>
                                <atmtech:ComboBoxAvance runat="server" ID="cboStockTemplate" />
                            </td>
                            <td>Produits sans stock:</td>
                            <td>
                                <atmtech:ComboBoxAvance runat="server" ID="cboProductWithoutStock" />
                            </td>

                            <td>Ne possède pas d'inventaire:
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkIsWithoutStock" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtQuantityStockTemplate" Text="0" Visible="False"></asp:TextBox>
                                <asp:Button runat="server" ID="btnApplyStockTemplate" OnClick="ApplyStockTemplateClick" CausesValidation="False" CssClass="button" Text="Appliquer le modèle" />
                            </td>
                        </tr>

                    </table>
                </asp:Panel>
            </div>
        </fieldset>
    </div>
</asp:Content>