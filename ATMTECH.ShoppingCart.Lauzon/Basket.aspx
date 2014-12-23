﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Basket.aspx.cs" Inherits="ATMTECH.ShoppingCart.Lauzon.Basket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div style="width: 950px; padding: 10px 10px 10px 10px; color: black; border: solid 1px gray; background-color: white; margin-bottom: 10px;">

        <asp:Panel runat="server" ID="pnlAskShipping" Visible="False" CssClass="askShippingBox">
            <asp:Label runat="server" ID="lblAskShipping" Font-Bold="True"></asp:Label>
        </asp:Panel>

        <div class="title">
            <strong>
                <asp:Label runat="server" ID="lblTitleBasket" Text="Votre panier" Visible="True"></asp:Label></strong>
        </div>
        <asp:Panel runat="server" ID="pnlBasketNotEmpty" Visible="True">
            <asp:Label runat="server" ID="lblOrderNumber" Text="Numéro de commande:" Visible="False"></asp:Label>
            <asp:Label runat="server" ID="lblOrderId" Visible="False"></asp:Label>
            <asp:GridView runat="server" ID="grvBasket" AutoGenerateColumns="false" CssClass="basketGrid"
                ShowHeader="True" ShowFooter="False" OnRowCommand="GrvBasketCommand">
                <HeaderStyle CssClass="basketGridHeader" />
                <Columns>
                    <asp:TemplateField HeaderText="Article(s)">
                        <ItemTemplate>
                            <div style="float: left;">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# "ThumbNail.aspx?width=109&height=104&directory=images/product/&filename=" +Eval("Stock.Product.PrincipalFileUrlWithoutDirectory")  %>'></asp:Image>
                            </div>
                            <div style="float: left; padding-left: 10px;">
                                <asp:Label runat="server" ID="lblIdent" Text='<%#Eval("Stock.Product.Ident")%>'></asp:Label>
                                <br />
                                <asp:Label runat="server" ID="lblProductName" Text='<%# Session["currentLanguage"].ToString().Equals("fr") ?  Eval("Stock.Product.NameFrench") : Eval("Stock.Product.NameEnglish")%>'
                                    CssClass="basketProductName"></asp:Label>
                                <br />
                                <asp:Label runat="server" ID="lblStockFeature" Text='<%# Session["currentLanguage"].ToString().Equals("fr") ?  Eval("Stock.FeatureFrench") : Eval("Stock.FeatureEnglish")  %>'></asp:Label>

                                <asp:Label runat="server" ID="lblAdjustPrice" Text='<%# "( +" + Eval("Stock.AdjustPrice","{0:c}") + ")"  %>' Visible='<%#  Convert.ToDecimal(Eval("Stock.AdjustPrice")) > 0 %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantité">
                        <ItemTemplate>
                            <center>
                                <asp:TextBox runat="server" ID="txtQuantity" Text='<%#Eval("Quantity")%>'
                                    Width="50px"></asp:TextBox>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Prix">
                        <ItemTemplate>
                            <center>
                                <asp:Label runat="server" ID="lblUnitPrice" Text='<%# Eval("Stock.StockPrice","{0:c}")  %>'></asp:Label></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Retirer">
                        <ItemTemplate>
                            <center>
                                <asp:ImageButton runat="server" ID="lnkDelete" CommandName="DeleteOrderLine" ImageUrl="~/Images/WebSite/Retirer.png"
                                    CommandArgument='<%# Eval("Id") %>' /></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="padding-bottom: 10px;">
                <asp:Button runat="server" ID="btnRecalculateBasket" Text="Recalculer" OnClick="RecalculerClick" />
            </div>

            <div style="float: left; width: 50%;">
                <asp:Label runat="server" ID="lblNotes"></asp:Label>
                <br />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtProject" Width="400px" Height="100px"></asp:TextBox>

            </div>


            <div style="float: left; width: 49%;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSubTotalLabel" Text="Sous-total:"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblSubTotal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblShippingTotalLabel" Text="Livraison:"></asp:Label>
                            <asp:Label runat="server" ID="lblShippingWeight" Text=""></asp:Label>
                            <asp:Label runat="server" ID="lblShippingNotIncluded" Visible="False" Text="Envoi non inclus." ForeColor="red" Font-Size="10px"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblShippingTotal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSubTotalTaxesRegionLabel" Text="TVQ:"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblSubTotalTaxesRegion" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblSubTotalTaxesCountryLabel" Text="TPS:"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblSubTotalTaxesCountry" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblGrandTotalLabel" Text="Grand total:"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblGrandTotal" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear: both;">
            </div>

            <div style="float: left; width: 49%;">
                <asp:Panel runat="server" ID="pnlOrderInformation1" Visible="False">
                    <div class="title">
                        <strong>
                            <asp:Label runat="server" ID="lblOrderInformation1" Text="Information 1" />
                        </strong>
                    </div>
                    <asp:DropDownList runat="server" ID="ddlOrderInformation1" Width="100%" />
                </asp:Panel>
                <br />
            </div>
            <div style="margin-left: 10px; float: left; width: 49%;">
                <asp:Panel runat="server" ID="pnlOrderInformation2" Visible="False">
                    <div class="title">
                        <strong>
                            <asp:Label runat="server" ID="lblOrderInformation2" Text="Information 2" />
                        </strong>
                    </div>
                    <asp:DropDownList runat="server" ID="ddlOrderInformation2" Width="100%" />
                </asp:Panel>
                <br />
            </div>

            <div style="clear: both;">
            </div>

            <asp:Label runat="server" ID="lblAdressInformationBasket" Visible="False"></asp:Label>
            <div class="title">
                <strong>
                    <asp:Label runat="server" ID="lblShippingLabel" Text="Adresse d'envoi" />
                </strong>
            </div>
            <asp:DropDownList runat="server" ID="ddlShipping" AutoPostBack="True" />
            <asp:Button runat="server" ID="btnModifyShippingAddress" OnClick="btnModifyShippingAddressClick"
                Text="Modifier" />
            <asp:Label runat="server" ID="lblNoShippingAddress"  Visible="False" ForeColor="red"></asp:Label>
            <br />
            <br />
            <div class="title">
                <strong>
                    <asp:Label runat="server" ID="lblBillingLabel" Text="Adresse de facturation" />
                </strong>
            </div>
            <asp:DropDownList runat="server" ID="ddlBilling" AutoPostBack="True" />
            <asp:Button runat="server" ID="btnModifyBillingAddress" OnClick="btnModifyBillingAddressClick"
                Text="Modifier" />
            <asp:Label runat="server" ID="lblNoBillingAddress" Visible="False" ForeColor="red"></asp:Label>

            <hr />
            <asp:Button runat="server" ID="btnContinueShopping" Text="Continuer le magasinage"
                OnClick="ContinueShoppingClick" CausesValidation="False" />
            <asp:Button runat="server" ID="btnFinalizeOrder" Text="Finaliser la commande" OnClick="FinalizeOrderClick"
                ValidationGroup="FinalizeOrder" />
            <asp:Button runat="server" ID="btnFinalizeOrderPaypal" Text="Finaliser la commande avec Paypal"
                OnClick="FinalizeOrderPaypal" ValidationGroup="FinalizeOrder" />
            <asp:Button runat="server" ID="btnAskShipping" Text="Demande de quotation pour envoi" OnClick="btnAskShippingClick" Visible="False" />
            <asp:Label runat="server" ID="lblAskShippingInformation" Text="Assurez-vous que votre commande est complète puisque vous ne pourrez pas commander d'articles supplémentaires lorsqu'une demande de soumission de frais de livraison est en cours." Visible="False"></asp:Label><br />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBasketEmpty" Visible="False">
            <asp:Label runat="server" ID="lblBasketIsEmpty" Text="Votre panier est vide"></asp:Label>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlOrderFinalized" Visible="False">
            <asp:Label runat="server" ID="lblOrderIsFinalized" Text="Merci de votre commande"></asp:Label><br />
            <hr />
            <asp:Button runat="server" ID="btnPrintOrder" Text="Imprimer le détail de votre commande"
                OnClick="PrintOrderClick" />
        </asp:Panel>
    </div>
</asp:Content>
