<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Wplaty.aspx.cs" Inherits="Rezerwacje.Pracownicy.Wplaty" %>
<%@ Import Namespace="System.Web.Profile" %>
<%@ Import Namespace="Rezerwacje.Data_Access"%>
<script runat="server">
   
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        tbCena.Visible = false;
        DetailsView1.Visible = false;
        PanelProfil.Visible = false;

        if (IsPostBack)
        {

            try
            {
                DetailsView1.DataBind();   
                ProfileCommon userProfile = Profile.GetProfile(DetailsView1.DataKey[0].ToString());
                tdImie.InnerText = (string)userProfile.GetPropertyValue("Imie");
                tdEmail.InnerText = Membership.GetUser().Email;
                tdNazwisko.InnerText = (string)userProfile.GetPropertyValue("Nazwisko");
                tdUlica.InnerText = (string)userProfile.GetPropertyValue("Ulica");
                tdNrDomu.InnerText = (string)userProfile.GetPropertyValue("NrDomu");
                tdKod.InnerText = (string)userProfile.GetPropertyValue("KodPocztowy");
                tdMiejscowosc.InnerText = (string)userProfile.GetPropertyValue("Miejscowosc");

                td1.InnerText = (string)userProfile.GetPropertyValue("Imie");
                td7.InnerText = Membership.GetUser().Email;
                td4.InnerText = (string)userProfile.GetPropertyValue("Nazwisko");
                td3.InnerText = (string)userProfile.GetPropertyValue("Ulica");
                td6.InnerText = (string)userProfile.GetPropertyValue("NrDomu");
                td8.InnerText = (string)userProfile.GetPropertyValue("KodPocztowy");
                td9.InnerText = (string)userProfile.GetPropertyValue("Miejscowosc");
                
                PanelProfil.Visible = true;
            }
            catch { }
        }
    }

    
    
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
  <asp:Label ID="Label1" runat="server" Text="Id Zamowienie (pierwsza część liczby w kodach kreskowych rezerwacji):"></asp:Label> 
    <asp:TextBox ID="tbIdZamowienia" runat="server"  Width="60px"  AutoPostBack="true"
        ontextchanged="tbIdZamowienia_TextChanged">  </asp:TextBox>
    <asp:Button ID="btnIdZamowienia" runat="server" Text="Pokaż bilety" 
        onclick="btnIdZamowienia_Click" />
    <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px" 
        AutoGenerateRows="False" DataSourceID="SqlDataSource3"   DataKeyNames="Klient">
        <Fields>
            <asp:BoundField DataField="Klient" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
        SelectCommand="SELECT [Klient] FROM [Zamowienia] WHERE        (dbo.Zamowienia.ZamowienieID = @ZamowienieId)">
    <SelectParameters>
        <asp:ControlParameter ControlID="tbIdZamowienia" Name="ZamowienieId" PropertyName="Text" Type="String" >

        </asp:ControlParameter >
        </SelectParameters>
    
    </asp:SqlDataSource>


       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 

         SelectCommand= " SELECT        dbo.ZlecenieSzczegoly.SzczegolyZleceniaID, dbo.ZlecenieSzczegoly.ImprezaID, dbo.ZlecenieSzczegoly.Cena, dbo.ZlecenieSzczegoly.Kod, 
                         dbo.ZlecenieSzczegoly.Zaplacono, dbo.Imprezy.Nazwa, dbo.Imprezy.Data, dbo.Imprezy.Miejsce, dbo.ZlecenieSzczegoly.TypKlienta, 
                         dbo.Zamowienia.DataZamowienia
FROM            dbo.Zamowienia INNER JOIN
                         dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID INNER JOIN
                         dbo.Imprezy ON dbo.ZlecenieSzczegoly.ImprezaID = dbo.Imprezy.ImprezaID
WHERE        (dbo.Zamowienia.ZamowienieID = @ZamowienieId)"    
 UpdateCommand="Update [ZlecenieSzczegoly] Set [Zaplacono]=@Zaplacono " 
>
          


        <SelectParameters>
        <asp:ControlParameter ControlID="tbIdZamowienia" Name="ZamowienieId" PropertyName="Text" Type="String" >

        </asp:ControlParameter >
    
    </SelectParameters>
    <UpdateParameters>
    
    <asp:Parameter Type="Boolean" Name="Zaplacono" />
    </UpdateParameters>
  </asp:SqlDataSource>
    <asp:Panel ID="PanelProfil" runat="server">

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
    </asp:Panel>

    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
        AutoGenerateColumns="False" DataKeyNames="SzczegolyZleceniaID"    >
       
        <Columns>
        <asp:ImageField DataImageUrlField="Kod" 
                DataImageUrlFormatString="~/Images/KodyKreskowe/{0}.jpg" 
                HeaderText="Kod Kreskowy" ReadOnly="True">
            </asp:ImageField>

          
            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa Imprezy" 
                SortExpression="Nazwa" ReadOnly="True" />
            <asp:BoundField DataField="Cena" HeaderText="Cena" SortExpression="Cena" ReadOnly="True" />
          
            <asp:BoundField DataField="Data" HeaderText="Data Imprezy" SortExpression="Data" 
                DataFormatString="{0:d/M/yyyy HH:mm}"                  />
            <asp:BoundField DataField="Miejsce" HeaderText="Miejsce" 
                SortExpression="Miejsce" ReadOnly="True" />
            <asp:BoundField DataField="TypKlienta" HeaderText="Typ Klienta" 
                SortExpression="TypKlienta" ReadOnly="True" />
                 <asp:BoundField DataField="DataZamowienia" HeaderText="Data Zamowienia" SortExpression="DataZamowienia" 
                DataFormatString="{0:d/M/yyyy HH:mm}" ReadOnly="True" />
           <asp:TemplateField ItemStyle-HorizontalAlign="Center">
             <ItemTemplate  >
              <asp:CheckBox AutoPostBack="true" ID="chk" runat="server"   Checked='<%# Eval("Zaplacono")%>' 
        oncheckedchanged="chk_CheckedChanged" />
            </ItemTemplate>
               
            <HeaderTemplate>Wpłacić
    </HeaderTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>

            </asp:TemplateField>
            
        </Columns>


        <EmptyDataTemplate>
            Brak biletów dla tego zamowienia
        </EmptyDataTemplate>




    </asp:GridView>
   <%-- <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Larger"   
        ForeColor="#FF3300"></asp:Label>--%>
        
        <br />
    <asp:Button ID="Button2" runat="server" Text="Policz Cene" 
        onclick="Button2_Click" />
    <asp:TextBox ID="tbCena" runat="server" AutoPostBack="true" Enabled="False" 
        Font-Bold="True" Font-Size="Larger" ForeColor="#FF3300" ></asp:TextBox>

        <br />
        
        <br />

    <asp:Button ID="Button1" runat="server" Text="Wydrukuj Potwierdzenie Zapłaty" 
        onclick="Button1_Click" />


    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="Label3" runat="server" Text="        REZERWACJE"></asp:Label> <br/>
       

        <div id="Div1" runat="server"></div>
    <table class="style4" runat="server" id="Table1">
        <tr>
            <td align="left" class="style8" >
                Imie:</td>
            <td class="style30" id="td1" style=" width:100px">
                &nbsp;</td>
                <td  align="left" class="style28" id="td2">
                Ulica:</td>
            <td ID="td3" style=" width:100px"></td>
          
        </tr>
        <tr>
            <td align="left" class="style24">
                Nazwisko:</td>
            <td class="style25" id="td4" runat="server" style=" width:100px">
                </td>
                 <td  align="left" class="style29" id="td5">
                 Nr domu:</td>
                 <td class="style27" id="td6" style=" width:100px">
                </td>
        </tr>
        <tr>
            <td align="left" >
                Email:</td>
            <td class="style22"  id="td7" runat="server" style=" width:100px">
                </td>
            <td  align="left" class="style28" >
                Kod pocztowy:</td>
            <td   id="td8" style=" width:100px">
                </td>
        </tr>
        <tr>
            <td class="style8" align="left">
                &nbsp;</td>
            <td class="style30">
                &nbsp;</td>
            <td class="style28" align="left">
                Miejscowosc:</td>
            <td id="td9" style=" width:100px">
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
    
     <asp:Label ID="Label4" runat="server" Text="Potwierdzenie opłaty za poniższe bilety:"></asp:Label>
     <br />
      <asp:GridView ID="GridView2" runat="server" DataSourceID="SqlDataSource2" 
        AutoGenerateColumns="False" DataKeyNames="SzczegolyZleceniaID" 
       >
        <Columns>
          <asp:BoundField DataField="Kod" HeaderText="Numer biletu" />
        <%--<asp:ImageField DataImageUrlField="Kod" 
                DataImageUrlFormatString="~/Images/KodyKreskowe/{0}.jpg" 
                HeaderText="Numer Biletu" ReadOnly="True">
            </asp:ImageField>--%>


          
            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa Imprezy" 
                SortExpression="Nazwa" ReadOnly="True" />
            <asp:BoundField DataField="Cena" HeaderText="Cena" SortExpression="Cena" ReadOnly="True" />
          
            <asp:BoundField DataField="Data" HeaderText="Data Imprezy" SortExpression="Data" 
                DataFormatString="{0:d/M/yyyy HH:mm}"                  />
            <asp:BoundField DataField="Miejsce" HeaderText="Miejsce" 
                SortExpression="Miejsce" ReadOnly="True" />
            <asp:BoundField DataField="TypKlienta" HeaderText="Typ Klienta" 
                SortExpression="TypKlienta" ReadOnly="True" />
                 <asp:BoundField DataField="DataZamowienia" HeaderText="Data Zamowienia" SortExpression="DataZamowienia" 
                DataFormatString="{0:d/M/yyyy HH:mm}" ReadOnly="True" />
           <asp:TemplateField ItemStyle-HorizontalAlign="Center">
             <ItemTemplate  >
                 <asp:Label ID="Label2" runat="server" Text="Label">Zapłacono gotówką</asp:Label>
            </ItemTemplate>
            <HeaderTemplate>Forma Płatności
    </HeaderTemplate>

               <ItemStyle HorizontalAlign="Center" />

            </asp:TemplateField>
            
          
            
        </Columns>


        <EmptyDataTemplate>
            Brak biletów dla tego zamowienia
        </EmptyDataTemplate>


    </asp:GridView>
        

   <asp:TextBox ID="tbCena2" runat="server" AutoPostBack="true" Enabled="False" 
        Font-Bold="True" Font-Size="Larger" ForeColor="#FF3300" ></asp:TextBox>
        </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"    ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
     SelectCommand=" SELECT dbo.ZlecenieSzczegoly.SzczegolyZleceniaID, dbo.ZlecenieSzczegoly.ImprezaID, dbo.ZlecenieSzczegoly.Cena, dbo.ZlecenieSzczegoly.Kod, 
                         dbo.ZlecenieSzczegoly.Zaplacono, dbo.Imprezy.Nazwa, dbo.Imprezy.Data, dbo.Imprezy.Miejsce, dbo.ZlecenieSzczegoly.TypKlienta, 
                         dbo.Zamowienia.DataZamowienia
                    FROM            dbo.Zamowienia INNER JOIN
                         dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID INNER JOIN
                         dbo.Imprezy ON dbo.ZlecenieSzczegoly.ImprezaID = dbo.Imprezy.ImprezaID
                    WHERE        (dbo.Zamowienia.ZamowienieID = @ZamowienieId) and ( dbo.ZlecenieSzczegoly.Zaplacono='True')"    
 UpdateCommand="Update [ZlecenieSzczegoly] Set [Zaplacono]=@Zaplacono " 
>
          


        <SelectParameters>
        <asp:ControlParameter ControlID="tbIdZamowienia" Name="ZamowienieId" PropertyName="Text" Type="String" >

        </asp:ControlParameter >
    
    </SelectParameters>
    <UpdateParameters>
    
    <asp:Parameter Type="Boolean" Name="Zaplacono" />
    </UpdateParameters>
        
        </asp:SqlDataSource>
</asp:Content>
