<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Rezerwacje.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Login ID="LoginUser" runat="server"  EnableViewState="false" RenderOuterTable="false" OnLoggedIn="LoginUser_LoggedIn"      >
        
        
        
           </asp:Login>
</asp:Content>
