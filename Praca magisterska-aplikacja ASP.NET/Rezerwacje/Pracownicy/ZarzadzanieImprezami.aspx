<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ZarzadzanieImprezami.aspx.cs" Inherits="Rezerwacje.Pracownicy.ZarzadzanieImprezami" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"  TagPrefix="asp" %>
<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"  TagPrefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {
            width: 269px;
        }
        .style5
        {
            width: 200px;
        }
        .style6
        {
            width: 243px;
        }
        .style7
        {
            width: 194px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <asp:ToolkitScriptManager ID="toolkitScriptMaster" runat="server">
</asp:ToolkitScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="2000">
        <ProgressTemplate>
            <asp:Image ID="Image1" runat="server"  ImageUrl="~/Images/progress.gif" />
    </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server"
     TargetControlID="textBoxData" PopupControlID="UpdatePanel1" OffsetY="25" >
    </asp:PopupControlExtender>
    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="textBoxGodzina" MaskType="Time" Mask="99:99"
      AcceptAMPM="false" UserTimeFormat="TwentyFourHour" AutoComplete="false" MessageValidatorTip="true" ErrorTooltipEnabled="True" InputDirection="LeftToRight">
    </asp:MaskedEditExtender>
     <asp:Button ID="Button1" runat="server" Text="Wprowadz dane nowej imprezy" 
        onclick="Button1_Click" />
    <asp:Panel ID="Panel1" runat="server" Visible="False">
        <table id="Table1" style="width: 100%;" runat="server">
            <tr>
                <td align="right" class="style7">
                   Nazwa:
                </td>
                <td id="tdNazwa" class="style6">
                    <asp:TextBox ID="textBoxNazwa" runat="server"></asp:TextBox>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="textBoxNazwa"></asp:RequiredFieldValidator>--%>
                </td>
                <td class="style5" align="right">
                   
                </td>
                <td>
                  
                </td>
            </tr>
            <tr>
                <td class="style7" align="right">
                    Data:
                </td>
                <td class="style6">
                    <asp:TextBox ID="textBoxData" runat="server"></asp:TextBox>
             <%--        <asp:RequiredFieldValidator ID="RequiredFieldValidatorData" runat="server"  ControlToValidate="textBoxData" ErrorMessage="Data imprezy jest wymagana"></asp:RequiredFieldValidator>--%>
                </td>
                <td class="style5" align="right" >
                    Godzina:
                </td>
                <td>
                    <asp:TextBox ID="textBoxGodzina" runat="server"></asp:TextBox>
                  <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidatorGodzina" runat="server"  ControlToValidate="textBoxGodzina" ErrorMessage="Godzina imprezy jest wymagana"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
              <tr>
                            <td align="right">
                                <asp:Label ID="Label" runat="server" AssociatedControlID="TextBoxUlica">Ulica:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxUlica" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorUlica" runat="server" 
                                    ControlToValidate="TextBoxUlica" ErrorMessage="Ulica jest wymagana." 
                                    ToolTip="Ulica jest wymagany." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorUl" runat="server"  ControlToValidate="TextBoxUlica" ErrorMessage="Ulica jest wymagana"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="TextBoxNrdomu">Nrdomu:</asp:Label>
                            </td>
                             <td>
                                <asp:TextBox ID="TextBoxNrdomu" runat="server"></asp:TextBox>
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidatorNrdomy" runat="server" 
                                    ControlToValidate="TextBoxNrdomu" ErrorMessage="Nr domu jest wymagana." 
                                    ToolTip="Nr domu jest wymagany." >*</asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>

                         <tr>
                            <td align="right">
                                <asp:Label ID="Label3" runat="server" AssociatedControlID="TextBoxKod">Kod pocztowy:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxKod" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorKod1" 
                                    runat="server" ErrorMessage="Kod ma postać dd-ddd"  ToolTip="Kod ma postać dd-ddd"
                                    ControlToValidate="TextBoxKod" ValidationExpression="\d{2}(-\d{3})?">*</asp:RegularExpressionValidator>
                             
                            </td>

                            <td align="right">
                                <asp:Label ID="Label4" runat="server" AssociatedControlID="TextBoxMiejscowosc">Miejscowosc:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxMiejscowosc" runat="server"></asp:TextBox>
                                  <%--      <asp:RequiredFieldValidator ID="RequiredFieldValidator1msc" runat="server" 
                                    ControlToValidate="TextBoxMiejscowosc" ErrorMessage="Miejscowosc jest wymagana." 
                                    ToolTip="Miejscowosc jest wymagany." >*</asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
            <tr>
                <td class="style7" align="right">
                    Avatar:
                </td>
                <td class="style6">
                    <asp:FileUpload ID="FileUpload1" runat="server"> </asp:FileUpload>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Plik musi być typu gif"
                     ControlToValidate="FileUpload1" ValidationExpression="([^\s]+(\.(?i)(gif))$)">
                     </asp:RegularExpressionValidator>
                </td>
                <td class="style5" align="right" >
                    Szczegóły:
                </td>
                <td>
                    <asp:TextBox ID="TextBoxSzczegoly" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td align="right">
             Liczba Biletów:
            </td>
            <td>
                <asp:TextBox ID="TextBoxLiczbaBiletow" runat="server"></asp:TextBox>
                  <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ControlToValidate="TextBoxLiczbaBiletow" ErrorMessage="Liczba biletów jest wymagana"></asp:RequiredFieldValidator>--%>
                    <asp:RangeValidator ID="RangeValidatorLB" runat="server"  ControlToValidate="TextBoxLiczbaBiletow" ErrorMessage="Błąd w cenie" MinimumValue="0" Type="Double"></asp:RangeValidator>
            </td>
            <td  align="right">
                Cena Podstawowa: 
            </td>
            <td >
                <asp:TextBox ID="TextBoxCenaP" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator2" runat="server"  ControlToValidate="TextBoxCenaP" ErrorMessage="Błąd w cenie" MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ControlToValidate="TextBoxCenaP" ErrorMessage="Cena Podstawowa biletów jest wymagana"></asp:RequiredFieldValidator>--%>
            </td>
            </tr>
            <tr>
            <td align="right">
           Cena studencka:
            </td>
            <td>
             <asp:TextBox ID="TextBoxCenaS" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidatorS" runat="server"  ControlToValidate="TextBoxCenaS" ErrorMessage="Błąd w cenie" MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorS" runat="server"  ControlToValidate="TextBoxCenaS" ErrorMessage="Cena studencka biletów jest wymagana"></asp:RequiredFieldValidator>--%>
            </td>
            <td align="right">
            Cena ulgowa:
            </td>
            <td>
             <asp:TextBox ID="TextBoxCenaU" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidatorU" runat="server"  ControlToValidate="TextBoxCenaU" ErrorMessage="Błąd w cenie" MinimumValue="0" Type="Double"></asp:RangeValidator>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorU" runat="server"  ControlToValidate="TextBoxCenaU" ErrorMessage="Cena ulgowa biletów jest wymagana"></asp:RequiredFieldValidator>--%>
            </td>
            </tr>
            <tr>
            <td align="center">
             <asp:Button ID="Button2" runat="server" Text="Dodaj Impreze" 
            onclick="Button2_Click" />
            </td>
            <td align="center">
                 <asp:Button ID="Button3" runat="server" Text="Anuluj" 
                    onclick="Button3_Click"  />
            </td>
            </tr>
        </table>
       
        <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
         ControlToValidate="textBoxGodzina"  EmptyValueMessage="Wprowadż poprawnie godzine imprezy"
         InvalidValueBlurredMessage="Wprowadż poprawnie godzine imprezy" MaximumValue="23:55" IsValidEmpty="true" ValidationExpression="^([0-9][0-9])([:])([0-9])(([0])|([5]))$"
          ValidationGroup="CheckWorkTimeValidation" Display="Static"></asp:MaskedEditValidator>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" 
                    BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
                    Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                    onselectionchanged="Calendar1_SelectionChanged" Width="200px">
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
             </ContentTemplate>
             <%--<Triggers>
             <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
             </Triggers>--%>
        </asp:UpdatePanel>
        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Button2" ConfirmText="Czy na pewno chcesz dodać tą impreze">
        </asp:ConfirmButtonExtender>

       
    </asp:Panel>

     <div style="height: 515px; overflow: auto">
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
        DataKeyNames="ImprezaID" DataSourceID="SqlDataSource1" ForeColor="Black" 
        GridLines="Vertical" onrowdeleting="GridView1_RowDeleting"  
             AutoGenerateEditButton="True" AutoGenerateSelectButton="true" 
             style="margin-right: 0px">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:TemplateField>
              <ItemTemplate>
                <asp:LinkButton ID="delete" runat="server" CommandName="Delete" Text="Usuń" />
                  <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="delete" ConfirmText="Czy napewno chcesz usunąć tą impreze?">
                  </asp:ConfirmButtonExtender>
               </ItemTemplate>
            </asp:TemplateField>

          <%--  <asp:BoundField DataField="Obrazek" HeaderText="Obrazek" 
                SortExpression="Obrazek" />--%>
<%--            <asp:BoundField DataField="ImprezaID" HeaderText="ImprezaID" 
                InsertVisible="False" ReadOnly="True" SortExpression="Impreza ID" />--%>
          
            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa" SortExpression="Nazwa" />
            <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data" />
            <asp:BoundField DataField="Miejsce" HeaderText="Miejsce" 
                SortExpression="Miejsce" />
           <%-- <asp:BoundField DataField="CenaPodstawowa" HeaderText="CenaPodstawowa" 
                SortExpression="CenaPodstawowa" />--%>
            <asp:BoundField DataField="Szczegoly" HeaderText="Szczegoly" 
                SortExpression="Szczegoly" />
           <%-- <asp:ImageField DataImageUrlField='Obrazek' ReadOnly="True" HeaderText="Avatar" 
                DataImageUrlFormatString="~/Images/ListaImprez/{0}" 
                NullImageUrl="~/Images/ListaImprez/brak_zdj.gif">
            </asp:ImageField>--%>
              <asp:BoundField DataField="LiczbaBiletow" HeaderText="Liczba Biletow" 
                SortExpression="LiczbaBiletow" />
            <asp:BoundField DataField="SprzedaneBilety" HeaderText="Sprzedane bilety" ReadOnly="True" SortExpression="SprzedaneBilety"/>



          <%--  <asp:CommandField ShowDeleteButton="True" />--%>
            <asp:BoundField DataField="CenaPodstawowa" HeaderText="Cena Podstawowa" />
            <asp:BoundField DataField="CenaStudent" HeaderText="Cena dla studentów" />
            <asp:BoundField DataField="CenaUlgowa" HeaderText="Cena ulgowa" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>
    </div>

   
   
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"  
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" DataSourceMode="DataSet" 
        SelectCommand="SELECT * FROM [wiev_imprezy]" 
          UpdateCommand="UPDATE [Imprezy] SET  [Nazwa]=@Nazwa, [Data]=@Data,
                         [Miejsce]=@Miejsce, [CenaPodstawowa]=@CenaPodstawowa, [CenaStudent]=@CenaStudent,
                         [CenaUlgowa]=@CenaUlgowa, [LiczbaBiletow]=@LiczbaBiletow, 
                         [Szczegoly]=@Szczegoly                            
                         WHERE [ImprezaID]=@ImprezaID" >
        <UpdateParameters>
            <%--<asp:Parameter Type="Int32" Name="ImprezaID" />--%>
            <asp:Parameter Type="String" Name="Nazwa" />
            <asp:Parameter Type="DateTime" Name="Data" />
            <asp:Parameter Type="String" Name="Miejsce" />
            <asp:Parameter Type="Decimal" Name="CenaPodstawowa" />
            <asp:Parameter Type="Decimal" Name="CenaStudent" />
            <asp:Parameter Type="Decimal" Name="CenaUlgowa" />
            <asp:Parameter Type="Int32" Name="LiczbaBiletow" />
           <%-- <asp:Parameter Type="String" Name="Obrazek" />--%>
            <asp:Parameter Type="String" Name="Szczegoly" />
           <%-- <asp:Parameter Type="Int32" Name="SprzedaneBilety" />--%>
        </UpdateParameters>
        
        </asp:SqlDataSource>
       
       <asp:SqlDataSource ID="SqlDataSource2" runat="server"  ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
      SelectCommand="SELECT       SUM(Cena) AS Expr1

                     FROM   dbo.ZlecenieSzczegoly WHERE ImprezaID=@ImprezaID" >
     <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="ImprezaID" PropertyName="SelectedValue" Type="Int32" >

        </asp:ControlParameter >
    
    </SelectParameters>  
     </asp:SqlDataSource>

    <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="393px" 
        HorizontalAlign="Center" DataSourceID="SqlDataSource2" 
        AutoGenerateRows="False" >
        <Fields>
            <asp:BoundField DataField="Expr1" 
                HeaderText="Wartość ze sprzedaży biletów dla wskazanej imprezy:" 
                ReadOnly="True" HeaderStyle-Height="25" NullDisplayText="0,0000">
<HeaderStyle Height="25px"></HeaderStyle>

            <ItemStyle ForeColor="#FF3300" Height="20px" HorizontalAlign="Center" />
            </asp:BoundField>
        </Fields>
    </asp:DetailsView>
    
</asp:Content>
