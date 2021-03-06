﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="AddProductToBasket.aspx.cs" Inherits="ATMTECH.ShoppingCart.Commerce.AddProductToBasket" %>

<%@ Register Src="UserControls/ListeCouleur.ascx" TagName="ListeCouleur" TagPrefix="uc2" %>
<%@ Register Src="UserControls/SlideShowFile.ascx" TagName="SlideShowFile" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:PlaceHolder ID="MetaPlaceHolder" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePannel">
        <ContentTemplate>
            <div id="fb-root"></div>
            <script>(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.3&appId=286270354802156";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>

            <div class="imageProduit">
                <asp:Image runat="server" ID="imgProduit" Style="width: 190px; height: 300px;" />
                <%--<uc3:SlideShowFile ID="ListeFichier" runat="server" />--%>
            </div>
            <div class="descriptionProduit">
                <div class="titreDescriptionProduit">
                    <asp:Label runat="server" ID="lblIdentProduit"></asp:Label>
                    -
            <asp:Label runat="server" ID="lblNomProduit"></asp:Label>
                </div>
                <div class="prixDescriptionProduit">
                    <asp:Label runat="server" ID="lblPrixUnitaire" CssClass="prixPaye"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblPrixOriginal" CssClass="prixRaye"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblVousEpargnez" Text="Vous épargnez" Visible="False" CssClass="prixEpargner"></asp:Label>
                    <asp:Label runat="server" ID="lblPrixEpargner" Visible="False" CssClass="prixEpargner"></asp:Label>
                </div>
                <div class="quantiteDescriptionProduit">
                    <div>
                        <div class="Table">
                            <div class="Heading">
                                <div class="Cell">
                                    <asp:Label runat="server" ID="lblCaracteristiqueAddProduct" Text="Choisir les caractéristiques"></asp:Label>
                                </div>
                                <div class="Cell">
                                    <asp:Label runat="server" ID="lblQuantiteAddProduct" Text="Quantité"></asp:Label>
                                </div>
                            </div>

                            <div class="Row" style="">
                                <div class="Cell" style="padding-bottom: 10px; width: 300px; text-align: left;">
                                    <atmtech:ComboBox runat="server" ID="ddlStock" Visible="False" />

                                    <asp:Label runat="server" ID="lblCouleur" Text="Couleur"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlCouleur" AutoPostBack="True" OnSelectedIndexChanged="ddlCouleurSelectedIndexChanged" CssClass="dropDownList" Width="100%" />
                                    <asp:Label runat="server" ID="lblTaille" Text="Taille"></asp:Label>
                                    <br />
                                    <asp:DropDownList runat="server" ID="ddlTaille" AutoPostBack="True" OnSelectedIndexChanged="ddlTailleSelectedIndexChanged" CssClass="dropDownList" Width="100%" />
                                </div>
                                <div class="Cell" style="">
                                    <br />
                                    <asp:Button runat="server" ID="btnVousDevezEtreConnectePourAjouterQuantite" Text="Identifier vous" OnClick="btnVousDevezEtreConnectePourAjouterQuantiteClick" CssClass="boutonLien"></asp:Button>
                                    <atmtech:Numeric runat="server" NoDecimal="True" ID="txtQuantite" CssClass="textBox" Width="50px" />
                                </div>
                            </div>
                        </div>
                        <br />

                        <asp:DataList runat="server" ID="dataListListeImagesCouleur" RepeatDirection="Horizontal" RepeatColumns="10" OnItemCommand="dataListListeImagesCouleurItemCommand">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imageCouleur" ImageUrl='<%# Eval("ImageCouleur") %>' AlternateText='<%# Eval("NomCouleurFr") %>' ToolTip='<%# Eval("NomCouleurFr") %>' CommandArgument='<%# Eval("Image") %>' CssClass="thumbnailCouleur" on CommandName="ChangerImage" />
                            </ItemTemplate>
                        </asp:DataList>

                        <%--<uc2:ListeCouleur ID="ListeCouleur" runat="server" />--%>
                        <br />

                        <asp:Image runat="server" ID="imgMarque" Visible="false" CssClass="imageMarqueDansAjoutPanier" />
                        <br />

                        <asp:Button runat="server" ID="btnConsulterLaCharte" OnClick="btnConsulterLaCharteClick" CssClass="boutonActionRond"
                            Text="Consulter la charte des grandeurs"></asp:Button>
                        <br />
                        <br />
                        <asp:Button runat="server" ID="btnAjouterLigneCommande" OnClick="btnAjouterLigneCommandeClick" CssClass="boutonActionRond"
                            Text="Ajouter au panier"></asp:Button>
                    </div>

                </div>
                <div class="detailDescriptionProduit">
                    <div style="float: left; padding-right: 10px;">
                        <asp:PlaceHolder runat="server" ID="placeHolderTwitter"></asp:PlaceHolder>
                        <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } }(document, 'script', 'twitter-wjs');</script>
                    </div>
                    <div style="float: left;">
                        <asp:PlaceHolder runat="server" ID="placeHolderFacebook"></asp:PlaceHolder>
                    </div>
                    <div style="clear: both;"></div>
                    <div style="padding-bottom: 10px;">
                        <asp:Label runat="server" ID="lblDetail" Text="Détails"></asp:Label>
                    </div>
                    <div>
                        <asp:Label runat="server" ID="lblDescription"></asp:Label>
                    </div>
                </div>
            </div>
            <div style="clear: both;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
