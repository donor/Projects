<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaImprez.aspx.cs" Inherits="Rezerwacje.ListaImprez" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"  TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<asp:ToolkitScriptManager ID="toolkitScriptMaster" runat="server">
</asp:ToolkitScriptManager>
   

    <asp:ListView ID="ListView_Products" runat="server"  DataSourceID="SqlDataSource1" GroupItemCount="3"      
           Enabled="True" DataKeyNames="ImprezaId" >

    <EmptyDataTemplate>
        <table id="Table1" runat="server">
            <tr>
                <td>BRAK IMPREZ W OFERCIE</td>
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
    <td id="Td2" runat="server"  >
        <table border="0" width="300">
            <tr>
                <td style="width: 25px;">&nbsp</td>
                <td style="vertical-align: middle; text-align: right;"> 
                   <image src='Images/ListaImprez/<%# Eval("Obrazek") %>' width="100" height="75" border="0" >
                </td>
                <td style="width: 250px; vertical-align: middle;">
                   <b>Nazwa: </b><%# Eval("Nazwa")%>
                </span><br />
                <span class="ProductListItem">
                    <b>Miejsce: </b><%# Eval("Miejsce")%>
                </span><br />
                <span class="ProductListItem">
                    <b>Data: </b><%# String.Format("{0:d/M/yyyy HH:mm}", Eval("Data"))%>
                </span><br />
                 <span class="ProductListItem">
                    <b>Ilość wolnych biletów: </b><%# Eval("Expr1")%>
                </span><br />
                   <asp:LinkButton ID="SelectButton" runat="server" Text="Bilety" CommandName="Select" />          
                </a>
                </span><br />
                <span class="ProductListItem">
                  <b>Szczegóły: </b><%# Eval("Szczegoly")%>
                  </span><br />
            </td>
        </tr>
        <tr >           
        </tr>
    </table>
</td>
</ItemTemplate>

<SelectedItemTemplate>
     <td id="Td2" runat="server"  style="background-color: yellow; font-weight: bold;">
        <table border="0" width="300">
            <tr>
                <td style="width: 25px;">&nbsp</td>
                <td style="vertical-align: middle; text-align: right;"> 
                    <image src='Images/ListaImprez/<%# Eval("Obrazek") %>' width="100" height="75" border="0" >                   
                </td>
                <td style="width: 250px; vertical-align: middle;">
                   <b>Nazwa: </b><%# Eval("Nazwa")%>
                </span><br />
                <span class="ProductListItem">
                    <b>Miejsce: </b><%# Eval("Miejsce")%>
                </span><br />
                <span class="ProductListItem">
                    <b>Data: </b><%# String.Format("{0:d/M/yyyy HH:mm}", Eval("Data"))%>
                </span><br />
                <span class="ProductListItem">
                    <b>Ilość wolnych biletów: </b><%# Eval("Expr1")%>
                </span><br />
                     <asp:LinkButton ID="SelectButton" runat="server" Text="Bilety" CommandName="Select" />          
                
                </span><br />
                <span class="ProductListItem">
                  <b>Szczegóły: </b><%# Eval("Szczegoly")%>
                  </span><br />
            </td>
        </tr>
        <tr >           
        </tr>
    </table>
</td>
</SelectedItemTemplate>

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
    <table width="100%" style="text-align:center">
      <asp:DataPager runat="server"  ID="DataPager" PageSize="9">
        <Fields>
          <asp:NumericPagerField ButtonCount="10"
               PreviousPageText="<--"
               NextPageText="-->" />
        </Fields>
    </asp:DataPager>
    </table>
    </LayoutTemplate>


</asp:ListView>
    <%--<asp:Label ID="Label1" runat="server" Text="Label">W celu przejrzenia swojego koszyka i potwierdzenia rezerwacji musisz być zalogowany w serwisie</asp:Label>--%>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
        SelectCommand="SELECT   TOP (100) PERCENT ImprezaID, Nazwa, Miejsce, Data, Obrazek, Szczegoly, LiczbaBiletow - SprzedaneBilety AS Expr1
                        FROM            dbo.Imprezy
                        WHERE        (Data >= @Param1) ORDER BY Data">
  <SelectParameters> 
  <asp:ControlParameter  Name="Param1" PropertyName="Text"  ControlID="tbData" Type="DateTime" />
  </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="lblMsg" runat="server" ForeColor="#CC3300"></asp:Label>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource2"         >
        <AlternatingItemTemplate>
            <span style="background-color: #FFF8DC;">CenaPodstawowa:
            <asp:Label ID="CenaPodstawowaLabel" runat="server" 
                Text='<%# Eval("CenaPodstawowa") %>' />
            <br />
            CenaStudent:
            <asp:Label ID="CenaStudentLabel" runat="server" 
                Text='<%# Eval("CenaStudent") %>' />
            <br />
            CenaUlgowa:
            <asp:Label ID="CenaUlgowaLabel" runat="server" 
                Text='<%# Eval("CenaUlgowa") %>' />
            <br />
<br /></span>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <span style="background-color: #008A8C;color: #FFFFFF;">CenaPodstawowa:
            <asp:TextBox ID="CenaPodstawowaTextBox" runat="server" 
                Text='<%# Bind("CenaPodstawowa") %>' />
            <br />
            CenaStudent:
            <asp:TextBox ID="CenaStudentTextBox" runat="server" 
                Text='<%# Bind("CenaStudent") %>' />
            <br />
            CenaUlgowa:
            <asp:TextBox ID="CenaUlgowaTextBox" runat="server" 
                Text='<%# Bind("CenaUlgowa") %>' />
            <br />
            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                Text="Update" />
            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                Text="Cancel" />
            <br /><br /></span>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <%--<span>Brak biletów.</span>--%>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <span style="">CenaPodstawowa:
            <asp:TextBox ID="CenaPodstawowaTextBox" runat="server" 
                Text='<%# Bind("CenaPodstawowa") %>' />
            <br />CenaStudent:
            <asp:TextBox ID="CenaStudentTextBox" runat="server" 
                Text='<%# Bind("CenaStudent") %>' />
            <br />CenaUlgowa:
            <asp:TextBox ID="CenaUlgowaTextBox" runat="server" 
                Text='<%# Bind("CenaUlgowa") %>' />
            <br />
            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                Text="Insert" />
            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                Text="Clear" />
            <br /><br /></span>
        </InsertItemTemplate>
        <ItemTemplate>
        
            <table  runat="server"    style="width: 60%;" frame="border">
                <tr align="center" bgcolor="#FFFF99" >
                    <td align="left" style=" width:150px">
                       <span><b>Kategoria cenowa<b/></span><br/>
                    </td>
                    <td style=" width:70px">
                        <span><b>Cena<b/></span><br/>
                    </td>
                    <td style=" width:70px">
                       <span><b>Ilosc<b/></span><br />
                    </td>
                </tr>
                <tr align="center">
                    <td align="left" style=" width:150px">
                            <span>Cana podstawowa</span><br/>
                    </td>
                    <td>
                         <asp:Label ID="CenaPodstawowaLabel" runat="server" Text='<%# Eval("CenaPodstawowa") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="tbCenaP" runat="server" Width="50" BackColor="#CCCCCC"></asp:TextBox >
                        <asp:RangeValidator ID="RangeValidator1"  runat="server" ErrorMessage="Błąd przy wprowadzaniu ilości biletów" ControlToValidate="tbCenaP" MinimumValue="0"  MaximumValue="1000000000" Type="Integer"
                         Display="None" ></asp:RangeValidator>
                          <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RangeValidator1">
                           </asp:ValidatorCalloutExtender>
                    </td>
                </tr>
                <tr align="center">
                    <td align="left" style=" width:150px">
                         <span>Cana dla studentów</span><br/>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server"  Text='<%# Eval("CenaStudent") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="tbCenaS" runat="server" Width="50" BackColor="#CCCCCC"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2"  runat="server" ErrorMessage="Błąd przy wprowadzaniu ilości biletów" ControlToValidate="tbCenaS" MinimumValue="0"  MaximumValue="1000000000" Type="Integer" Display="None"></asp:RangeValidator>
                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RangeValidator2">
                           </asp:ValidatorCalloutExtender>

                    </td>
                </tr>
                <tr align="center">
                    <td align="left" style=" width:150px">
                         <span>Cana ulgowa</span><br/>
                    </td>
                    <td align="center" style=" width:150px">
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("CenaUlgowa") %>' />
                    </td>
                    <td >
                        <asp:TextBox ID="tbCenaU" runat="server" Width="50" BackColor="#CCCCCC"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator3"  runat="server" ErrorMessage="Błąd przy wprowadzaniu ilości biletów" ControlToValidate="tbCenaU" MinimumValue="0"  MaximumValue="1000000000" Type="Integer" Display="None"></asp:RangeValidator>
                         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RangeValidator3">
                           </asp:ValidatorCalloutExtender>


                    </td>
                </tr>
            
            </table>
            <table style="width: 60%;" frame="border">
                <tr bgcolor="silver"  >
                <td align="left">
                    <asp:LinkButton ID="lbKoszyk" runat="server" onclick="lbKoszyk_Click">Przejdź do koszyka</asp:LinkButton>
                      
                </td>
                <td align="right">
                 <asp:LinkButton ID="lbDodaj" runat="server" onclick="lbDodaj_Click" >Dodaj do koszyka</asp:LinkButton>
                </td>
                
                </tr>

            </table>
           

        </ItemTemplate>
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" 
                style="font-family: Verdana, Arial, Helvetica, sans-serif;">
                <span runat="server" id="itemPlaceholder" />
            </div>
            <div style="text-align: center;background-color: #CCCCCC;font-family: Verdana, Arial, Helvetica, sans-serif;color: #000000;">
            </div>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <span style="background-color: #008A8C;font-weight: bold;color: #FFFFFF;">
            Cena Podstawowa
            <asp:Label ID="CenaPodstawowaLabel" runat="server" 
                Text='<%# Eval("CenaPodstawowa") %>' />
            <br />
            Cena dla studentów
            <asp:Label ID="CenaStudentLabel" runat="server" 
                Text='<%# Eval("CenaStudent") %>' />
            <br />
            Cena ulgowa
            <asp:Label ID="CenaUlgowaLabel" runat="server" 
                Text='<%# Eval("CenaUlgowa") %>' />
            <br />
<br /></span>
        </SelectedItemTemplate>
    </asp:ListView>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
        SelectCommand="SELECT [CenaPodstawowa], [CenaStudent], [CenaUlgowa] FROM [Imprezy] Where [ImprezaID]=@ImprezaID">
        <SelectParameters>
           <asp:ControlParameter ControlID="ListView_Products" Name="ImprezaID" PropertyName="SelectedValue" Type="Int32" >

        </asp:ControlParameter >
        </SelectParameters>
    </asp:SqlDataSource>

          
    <asp:TextBox ID="tbData" runat="server"></asp:TextBox>          
   
    </asp:Content>
