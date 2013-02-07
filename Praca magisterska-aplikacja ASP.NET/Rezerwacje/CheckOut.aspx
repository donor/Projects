<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="Rezerwacje.CheckOut"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script runat="server"> 
    
   
    
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {
            width: 93%;
            height: 65px;
        }
        .style8
        {
            width: 63px;
        }
        .style9
        {
            width: 221px;
        }
        .style20
        {
            width: 74px;
        }
        .style22
        {
            width: 103px;
            height: 6px;
        }
        .style24
        {
            width: 63px;
            height: 21px;
        }
        .style25
        {
            width: 103px;
            height: 21px;
        }
        .style27
        {
            height: 21px;
        }
        .style28
        {
            width: 96px;
        }
        .style29
        {
            width: 96px;
            height: 21px;
        }
        .style30
        {
            width: 103px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="Panel1" runat="server">
  
    <div id="naglowek" runat="server">Rezerwacje</div>
    <table class="style4" runat="server" id="tblDane">
        <tr>
            <td align="left" class="style8" >
                Imie:</td>
            <td class="style30" id="tdImie" style=" width:100px">
                &nbsp;</td>
                <td  align="left" class="style28" id="tdU">
                Ulica:</td>
            <td ID="tdUlica" style=" width:100px"></td>
          
        </tr>
        <tr>
            <td align="left" class="style24">
                Nazwisko:</td>
            <td class="style25" id="tdNazwisko" runat="server" style=" width:100px">
                </td>
                 <td  align="left" class="style29" id="tdnr">
                 Nr domu:</td>
                 <td class="style27" id="tdNrDomu" style=" width:100px">
                </td>
        </tr>
        <tr>
            <td align="left" >
                Email:</td>
            <td class="style22"  id="tdEmail" runat="server" style=" width:100px">
                </td>
            <td  align="left" class="style28" >
                Kod pocztowy:</td>
            <td   id="tdKod" style=" width:100px">
                </td>
        </tr>
        <tr>
            <td class="style8" align="left">
                &nbsp;</td>
            <td class="style30">
                &nbsp;</td>
            <td class="style28" align="left">
                Miejscowosc:</td>
            <td id="tdMiejscowosc" style=" width:100px">
                </td>
        </tr>
        <tr>
            <td align="left" class="style8">
                &nbsp;</td>
            <td class="style30">
                &nbsp;</td>
            <td class="style28">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
        <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="238px" 
            DataSourceID="SqlDataSource2" AutoGenerateRows="False"            >
           
            <Fields>
                <asp:BoundField DataField="Expr1" HeaderText="Wartosc biletow w koszyku:" 
                    ConvertEmptyStringToNull="False" NullDisplayText="0,0000">
                <ItemStyle ForeColor="#FF3300" />
                </asp:BoundField>
            </Fields>
           
        </asp:DetailsView>

        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 

         SelectCommand= " SELECT        SUM(dbo.ZlecenieSzczegoly.Cena) AS Expr1
                          FROM            dbo.Zamowienia INNER JOIN
                            dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID INNER JOIN
                            dbo.Imprezy ON dbo.ZlecenieSzczegoly.ImprezaID = dbo.Imprezy.ImprezaID
                          WHERE        (dbo.Zamowienia.ZamowienieID =  @ZamowienieId)"     >
          

        <SelectParameters>
            <asp:SessionParameter Name="ZamowienieId"  SessionField="zamowienieId" Type="String"/>
          
      
        </SelectParameters>
        </asp:SqlDataSource>

    <asp:ListView ID="ListViewRezerwacje" runat="server" DataSourceID="SqlDataSource1" 
            DataKeyNames="SzczegolyZleceniaID" 
           onselectedindexchanged="ListViewRezerwacje_SelectedIndexChanged" 
         >
    <EmptyDataTemplate>
        <table id="Table1" runat="server">
            <tr>
                <td>BRAK REZERWACJI </td>
            </tr>
        </table>
</EmptyDataTemplate>

<EmptyItemTemplate>
    <td id="Td1" runat="server" />
</EmptyItemTemplate>
<GroupTemplate>
    <tr ID="itemPlaceholderContainer" runat="server">
        <td ID="itemPlaceholder" runat="server"></td>
    </tr>
</GroupTemplate>
<ItemTemplate>
    <td id="Td2" runat="server" >
        <table border="0" width="500">
            <tr>
            
                <td style="vertical-align: middle; text-align: right;"> 
                    
                        <image src='Images/KodyKreskowe/<%# Eval("Kod") %>.jpg' 
                            width="152" height="70" border="0" >
                    &nbsp;&nbsp</td>
                <td style="width: 350px; vertical-align: middle;">
              
                 <span class="ProductListItem">
                    <b>Nazwa: </b><%# Eval("Nazwa")%>
                </span><br />
                <span class="ProductListItem">
                    <b>Mjeisce: </b><%# Eval("Miejsce")%>
                </span><br />
                <span class="ProductListItem">
                    <b>Data: </b><%# String.Format("{0:d}",Eval("Data")) %>
                <span><br />
                <span class="ProductListItem">
                    <b>Bilet: </b><%# Eval("TypKlienta") %>
                <span><br />
                 <span class="ProductListItem">
                    <b>Cena: </b><%# Eval("Cena")%>
                </span><br />
                 
            </td>
             <td style="width: 10px;">&nbsp</td>
            <td align="right"  style="width:150px">
           
     <asp:ImageButton ID="SelectButton" runat="server" EnableViewState="True"  CommandName="Select"
        ImageAlign="Right" ImageUrl="~/Images/usun.jpg"  />
        
            </td>

        </tr>
    </table>
</td>
</ItemTemplate>
<LayoutTemplate>
    <table id="Table2" runat="server"  >
        <tr id="Tr1" runat="server">
            <td id="Td3" runat="server">
                <table ID="groupPlaceholderContainer" runat="server" >
                    <tr ID="groupPlaceholder" runat="server"></tr>
                </table>
            </td>
        </tr>
      
    </table>
    </LayoutTemplate>
    </asp:ListView>
     </asp:Panel>
    <asp:Label ID="Label1" runat="server" Text="Label">Musisz wydrukować rezerwacje w celu okazania ich przy wejściu na impreze, wybierz jedną z opcji poniżej</asp:Label>
        <table style="width: 100%;">
            <tr>
                <td class="style9">
                    &nbsp;
                    <asp:Button ID="btnAkceptuj" runat="server" onclick="btnAkceptuj_Click" 
                        Text="Wydrukuj Rezerwacje" 
                        ToolTip="Jeżeli masz dostęp do drukarki, wydrukuj teraz rezerwacje" />
                </td>
                <td>
                    &nbsp;
                    <asp:Button ID="btnWysliMail" runat="server" onclick="btnWysliMail_Click" 
                        Text="Wyślij rezerwacje na maila" 
                        ToolTip="Możesz wysłać rezerwacje na swój email adres, dzięki czemu będziesz miał do nich dostęp elektroniczny" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            
        </table>

        
    
    
       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 

         SelectCommand= " SELECT dbo.ZlecenieSzczegoly.SzczegolyZleceniaID, dbo.ZlecenieSzczegoly.ImprezaID, dbo.ZlecenieSzczegoly.Cena, dbo.ZlecenieSzczegoly.Kod, dbo.Imprezy.Nazwa, dbo.Imprezy.Data, dbo.Imprezy.Miejsce, dbo.ZlecenieSzczegoly.TypKlienta 
         FROM dbo.Zamowienia INNER JOIN dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID INNER JOIN dbo.Imprezy ON dbo.ZlecenieSzczegoly.ImprezaID = dbo.Imprezy.ImprezaID 
         WHERE (dbo.Zamowienia.ZamowienieID = @ZamowienieId)"     >
          

        <SelectParameters>
            <asp:SessionParameter Name="ZamowienieId"  SessionField="zamowienieId" Type="String"/>
          
      
        </SelectParameters>
        </asp:SqlDataSource>
      
       
</asp:Content>
