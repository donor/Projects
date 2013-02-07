<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Klienci.aspx.cs" Inherits="Rezerwacje.Pracownicy.Klienci" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"  TagPrefix="asp" %>
<%@ Import Namespace="System.Web.Profile" %>
<%@ Import Namespace="System.Net.Mail" %>





<script  runat="server">
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        Button1.Visible = false;
        CreateUserWizard1.Visible = true;
    }

    protected void CreateUserWizard1_CancelButtonClick(object sender, EventArgs e)
    {
        Button1.Visible = true;
        CreateUserWizard1.Visible = false;
    }
    
  protected void CreateUserWizard1_CreatedUser1(object sender, EventArgs e)
    {
        TextBox imieTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxImie");
        TextBox nazwiskoTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxNazwisko");
        TextBox peselTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxPESEL");
        TextBox ulicaTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxUlica");
        TextBox nrDomuTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxNrdomu");
        TextBox kodTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxKod");
        TextBox miejscowoscTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxMiejscowosc");

        ProfileCommon pc = new ProfileCommon();
        pc.Initialize(CreateUserWizard1.UserName.ToString(), true);
        pc.Imie = imieTextBox.Text;
        pc.Nazwisko = nazwiskoTextBox.Text;
        pc.PESEL = peselTextBox.Text;
        pc.Ulica = ulicaTextBox.Text;
        pc.NrDomu = nrDomuTextBox.Text;
        pc.KodPocztowy = kodTextBox.Text;
        pc.Miejscowosc = miejscowoscTextBox.Text;
        pc.Save();



        string userName = CreateUserWizard1.UserName;
        string emailAddress = CreateUserWizard1.Email;

        Roles.AddUserToRole(userName, "Klient"); //dodawanie uzytkownika do roli pracownik
        Guid userId = (Guid)Membership.GetUser(userName).ProviderUserKey;


      
        MembershipUser user = Membership.GetUser(userId);
        // Activate the user
        user.IsApproved = false;
        // Update the user activation
        Membership.UpdateUser(user);
        
        
        // Now lets create an email message
        StringBuilder emailMessage = new StringBuilder();
        emailMessage.Append("Witamy w serwisie Rezerwacje");
        emailMessage.Append("<br />");
        emailMessage.Append("Twój login i hasło:  <br />");
        emailMessage.Append("Login: "+userName +" <br />");
        emailMessage.Append("Hasło: " + CreateUserWizard1.Password + " <br />");
        //emailMessage.Append("Konto jest już aktywne" +"<br />");
        emailMessage.Append("Nacisnij ponizszy link w celu aktywacji konta <br />");
        //emailMessage.Append(string.Format("<a href='http://localhost/rezerwacje/Account/ActivateUser.aspx?userName{0}&Id={1}'>Aktywuj {0} </a>", userName, userId.ToString()));
        emailMessage.Append(string.Format("<a href='http://rezerwacje.dyndns.org/rezerwacje/Account/ActivateUser.aspx?userName{0}&Id={1}'>Aktywuj {0} </a>", userName, userId.ToString()));
        MailMessage email = new MailMessage();
        email.From = new MailAddress("serwisrezerwacje@gmail.com");
        email.To.Add(new MailAddress(emailAddress));
        email.Subject = "Witamy w Rezerwacjach";
        email.Body = emailMessage.ToString();
        email.IsBodyHtml = true;

        // Send the email
        SmtpClient client = new SmtpClient();
        client.Send(email);
       
        
        CreateUserWizard1.Visible = false;
        GridView1.DataBind();
        Button1.Visible = true;
        Response.Redirect("~/Pracownicy/Klienci.aspx", true);
      
    }
    
   
    
</script>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {
            width: 468px;
        }
        .style5
        {
            width: 548px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="toolkitScriptMaster" runat="server">
</asp:ToolkitScriptManager>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
        TRWA DODAWANIE KLIENTA
    </ProgressTemplate>
    </asp:UpdateProgress>

     <asp:Button ID="Button1" runat="server" Text="Dodaj Klienta" 
        onclick="Button1_Click"  />
     <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" 
    DisableCreatedUser="True" oncreateduser="CreateUserWizard1_CreatedUser1" 
        Visible="False" DisplayCancelButton="True" 
        oncancelbuttonclick="CreateUserWizard1_CancelButtonClick" >
        <WizardSteps>

            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td align="center" colspan="2">
                                Wypełnij dane dla nowego konta</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Login:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                    ControlToValidate="UserName" ErrorMessage="Login jest wymagany." 
                                    ToolTip="Login jest wymagany." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Hasło:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Hasło jest wymagane." 
                                    ToolTip="Hasło jest wymagane." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" 
                                    AssociatedControlID="ConfirmPassword">Potwierdź hasło:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
                                    ControlToValidate="ConfirmPassword" 
                                    ErrorMessage="Potwiedzenie hasła jest wymagane." 
                                    ToolTip="Potwiedzenie hasła jest wymagane." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>

                        <tr>
                            <td align="right">
                               <asp:Label ID="LabelImie" runat="server" AssociatedControlID="TextBoxImie">Imie:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxImie" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorImie" runat="server" 
                                    ControlToValidate="TextBoxImie" ErrorMessage="Imie jest wymagane." 
                                    ToolTip="Imie jest wymagane." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                
                            </td>
                        </tr>

                        <tr>
                            <td align="right">
                               <asp:Label ID="LabelNazwisko" runat="server" AssociatedControlID="TextBoxNazwisko">Nazwisko:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxNazwisko" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorNazwisko" runat="server" 
                                    ControlToValidate="TextBoxNazwisko" ErrorMessage="Nazwisko jest wymagane." 
                                    ToolTip="Nazwisko jest wymagane." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>

                       <tr>
                            <td align="right" class="style4">
                               <asp:Label ID="Label1" runat="server" AssociatedControlID="TextBoxPESEL">PESEL:</asp:Label>
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="TextBoxPESEL" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPESEL" runat="server" ControlToValidate="TextBoxPESEL" ErrorMessage="PESEL musi mieć 11 cyfr." 
                                    ToolTip="PESEL musi mieć 11 cyfr." ValidationGroup="CreateUserWizard1"  ValidationExpression="\d{11}">*</asp:RegularExpressionValidator>                               
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
                            </td>
                            <td align="right">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="TextBoxNrdomu">Nrdomu:</asp:Label>
                            </td>
                             <td>
                                <asp:TextBox ID="TextBoxNrdomu" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorNrdomy" runat="server" 
                                    ControlToValidate="TextBoxNrdomu" ErrorMessage="Nr domu jest wymagana." 
                                    ToolTip="Nr domu jest wymagany." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
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
                               <asp:RequiredFieldValidator ID="RequiredFieldValidatorKod" runat="server" 
                                    ControlToValidate="TextBoxKod" ErrorMessage="Kod jest wymagana." 
                                    ToolTip="Kod jest wymagany." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>

                            <td align="right">
                                <asp:Label ID="Label4" runat="server" AssociatedControlID="TextBoxMiejscowosc">Miejscowosc:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxMiejscowosc" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="TextBoxMiejscowosc" ErrorMessage="Miejscowosc jest wymagana." 
                                    ToolTip="Miejscowosc jest wymagany." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>



                        <tr>
                            <td align="right">
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail"  ValidationGroup="CreateUserWizard1"
                                    runat="server"  ControlToValidate="Email"      ErrorMessage="Błąd w adresie e-mail"   ToolTip="Błąd w adresie e-mail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>                               
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ControlToValidate="Email" ErrorMessage="Pole z adresem e-mail jest wymagana">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Pytanie bezpieczeństwa:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Question" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" 
                                    ControlToValidate="Question" ErrorMessage="Pytanie bezpieczenstwa jest wymagane." 
                                    ToolTip="Pytanie bezpieczenstwa jest wymagane." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Odpowiedź bezpiezeństwa:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" 
                                    ControlToValidate="Answer" ErrorMessage="Odpowiedż jest wymagana." 
                                    ToolTip="Odpowiedż jest wymagana." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:CompareValidator ID="PasswordCompare" runat="server" 
                                    ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                    Display="Dynamic" 
                                    ErrorMessage="Hasło i potwierdzenie hasła muszą być takie same." 
                                    ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="color:Red;">
                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
               </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server" >
            </asp:CompleteWizardStep>
        </WizardSteps>

    </asp:CreateUserWizard>

<div style="height: 555px; overflow: auto">

          <asp:GridView   ID="GridView1" runat="server"  PageSize="8"
        DataSourceID="ObjectDataSource1" DataKeyNames="UserName"
     AutoGenerateColumns="False" AllowSorting="True"  Width="95%" AllowPaging="True" 
              BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
              CellPadding="3" HorizontalAlign="Center" >
        <Columns>
            <asp:TemplateField>
              <ItemTemplate>
                <asp:LinkButton ID="delete" runat="server" CommandName="Delete" Text="Usuń" />
                  <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="delete" ConfirmText="Czy napewno chcesz usunąć tego klienta?">
                  </asp:ConfirmButtonExtender>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField  ShowEditButton="True" ShowSelectButton="True" SelectText="Sprawdź bilety" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="UserName" HeaderText="Nazwa użytkownika" 
                ReadOnly="True" SortExpression="UserName" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField  DataField="Imie" HeaderText="Imie" ReadOnly="false" SortExpression="Imie" ><ItemStyle HorizontalAlign="Center" /> </asp:BoundField>
            <asp:BoundField DataField="Nazwisko" HeaderText="Nazwisko" ReadOnly="false" SortExpression="Nazwisko" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField> 
            <asp:BoundField ReadOnly="False" HeaderText="PESEL" DataField="PESEL" SortExpression="PESEL" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:BoundField  HeaderText="Ulica" DataField="Ulica" SortExpression="Ulica"> <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:BoundField  HeaderText="Nr domu" DataField="NrDomu" SortExpression="NrDomu">  <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:BoundField  HeaderText="Kod pocztowy" DataField="KodPocztowy" SortExpression="KodPocztowy"> <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:BoundField  HeaderText="Miejscowosc " DataField="Miejscowosc" SortExpression="Miejscowosc"> <ItemStyle HorizontalAlign="Center" />       </asp:BoundField> 
            <asp:BoundField DataField="Comment" HeaderText="Komentarz" SortExpression="Comment" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:BoundField DataField="CreationDate" HeaderText="Data utworzenia" ReadOnly="True"
                                    SortExpression="CreationDate" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:CheckBoxField DataField="IsApproved" HeaderText="Jest zatwierdzony" SortExpression="IsApproved" > <ItemStyle HorizontalAlign="Center" />       </asp:CheckBoxField>
            <asp:BoundField DataField="LastLoginDate" HeaderText="Data ostatniego logowania" SortExpression="LastLoginDate" Visible="True" ReadOnly="True" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
            <asp:CheckBoxField DataField="IsOnline" Visible="True" HeaderText="Jest  online" ReadOnly="True"
                                    SortExpression="IsOnline" > <ItemStyle HorizontalAlign="Center" />       </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Jest zablokowany" ReadOnly="True"
                                    SortExpression="IsLockedOut" Visible="True" > <ItemStyle HorizontalAlign="Center" />       </asp:CheckBoxField>
            <asp:BoundField DataField="LastActivityDate" HeaderText="Ostatnia aktywnosc" SortExpression="LastActivityDate"
                                    Visible="True" ReadOnly="True" > <ItemStyle HorizontalAlign="Center" />       </asp:BoundField>
        </Columns>
              <EmptyDataTemplate>
                  Brak Klientów
              </EmptyDataTemplate>
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Center" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    </asp:GridView>
        
      </div>
 
 

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DeleteMethod="Delete" InsertMethod="Insert" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetMembers" 
        TypeName="Rezerwacje.App_Code.MembershipUserAndProfileODSKlient" 
        UpdateMethod="Update" SortParameterName="SortData">
        <DeleteParameters>
            <asp:Parameter Name="UserName" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="userName" Type="String" />
            <asp:Parameter Name="isApproved" Type="Boolean" />
            <asp:Parameter Name="comment" Type="String" />
            <asp:Parameter Name="lastLockoutDate" Type="DateTime" />
            <asp:Parameter Name="creationDate" Type="DateTime" />
            <asp:Parameter Name="email" Type="String" />
            <asp:Parameter Name="lastActivityDate" Type="DateTime" />
            <asp:Parameter Name="providerName" Type="String" />
            <asp:Parameter Name="isLockedOut" Type="Boolean" />
            <asp:Parameter Name="lastLoginDate" Type="DateTime" />
            <asp:Parameter Name="isOnline" Type="Boolean" />
            <asp:Parameter Name="passwordQuestion" Type="String" />
            <asp:Parameter Name="lastPasswordChangedDate" Type="DateTime" />
            <asp:Parameter Name="password" Type="String" />
            <asp:Parameter Name="passwordAnswer" Type="String" />
            <asp:Parameter Name="nazwisko" Type="String" />
            <asp:Parameter Name="imie" Type="String" />
            <asp:Parameter Name="pESEL" Type="String" />
            <asp:Parameter Name="ulica" Type="String" />
            <asp:Parameter Name="nrDomu" Type="String" />
            <asp:Parameter Name="kodPocztowy" Type="String" />
            <asp:Parameter Name="miejscowosc" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Name="sortData" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="userName" Type="String" />
            <asp:Parameter Name="email" Type="String" />
            <asp:Parameter Name="isLockedOut" Type="Boolean" />
            <asp:Parameter Name="isApproved" Type="Boolean" />
            <asp:Parameter Name="comment" Type="String" />
            <asp:Parameter Name="lastActivityDate" Type="DateTime" />
            <asp:Parameter Name="lastLoginDate" Type="DateTime" />
            <asp:Parameter Name="nazwisko" Type="String" />
            <asp:Parameter Name="imie" Type="String" />
            <asp:Parameter Name="pESEL" Type="String" />
            <asp:Parameter Name="ulica" Type="String" />
            <asp:Parameter Name="nrDomu" Type="String" />
            <asp:Parameter Name="kodPocztowy" Type="String" />
            <asp:Parameter Name="miejscowosc" Type="String" />
        </UpdateParameters>
    </asp:ObjectDataSource>      
    
    
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
    SelectCommand="SELECT dbo.ZlecenieSzczegoly.Kod, dbo.ZlecenieSzczegoly.TypKlienta, dbo.ZlecenieSzczegoly.Zaplacono, dbo.ZlecenieSzczegoly.Cena, dbo.Imprezy.Nazwa, dbo.Imprezy.Data, dbo.Imprezy.Miejsce, dbo.Zamowienia.DataZamowienia
                    FROM dbo.Zamowienia INNER JOIN
                         dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID INNER JOIN
                         dbo.Imprezy ON dbo.ZlecenieSzczegoly.ImprezaID = dbo.Imprezy.ImprezaID
                    WHERE (dbo.Zamowienia.Klient = @UserName)">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="UserName" PropertyName="SelectedValue" Type="String" >

        </asp:ControlParameter >
    
    </SelectParameters>
        

</asp:SqlDataSource>
    <table style="width: 100%; height: 171px;">
        <tr>
            <td class="style5">
                <asp:GridView ID="GridView2" runat="server" DataSourceID="SqlDataSource1" 
        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False">
    <Columns>
        <asp:ImageField  
         DataImageUrlField='Kod'
            
            DataImageUrlFormatString="~/Images/KodyKreskowe/{0}.jpg" 
            HeaderText="Kod Kreskowy">
        </asp:ImageField>
     <%--   <asp:BoundField DataField="Kod" HeaderText="Kod Kreskowy" ReadOnly="True" 
            SortExpression="Kod" />--%>
        <asp:BoundField DataField="Nazwa" HeaderText="Nazwa Imprezy" ReadOnly="True" 
            SortExpression="Nazwa" />
        <asp:BoundField DataField="Miejsce" HeaderText="Miejsce" ReadOnly="True" 
            SortExpression="Miejsce" />
        <asp:BoundField DataField="Data" DataFormatString="{0:d}" 
            HeaderText="Data Imprezy" ReadOnly="True" SortExpression="Data" />
        <asp:BoundField DataField="Cena" HeaderText="Cena biletu" ReadOnly="True" 
            SortExpression="Cena" />
        <asp:BoundField DataField="TypKlienta" HeaderText="Typ biletu" 
            ReadOnly="True" SortExpression="TypKlienta" />
      
            <asp:TemplateField HeaderText="Zapłacono" ItemStyle-HorizontalAlign="Center" SortExpression="Zaplacono">
                    <ItemTemplate>
                        <asp:Label ID="chkStatus" runat="server"
                            
                           
                            Text='<%# Eval("Zaplacono").ToString().Equals("True") ? " Tak " : " Nie " %>' />
                    </ItemTemplate>                   
                </asp:TemplateField>

    </Columns>
   <%-- <EmptyDataTemplate>
        Brak biletów dla tego klienta
    </EmptyDataTemplate>--%>
                    <EmptyDataTemplate>
                        Ten klient nie zarezerwował jeszcze żadnego biletu
                    </EmptyDataTemplate>
        </asp:GridView>
            </td>
            <td>
                <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="233px" 
                    AutoGenerateRows="False" DataSourceID="SqlDataSource2">
                    <Fields>
                        <asp:BoundField DataField="Expr1" HeaderText="Wartość biletów klienta:" 
                            ReadOnly="True" SortExpression="Expr1" HeaderStyle-Height="20" >
<HeaderStyle Height="20px"></HeaderStyle>
                        </asp:BoundField>
                    </Fields>
                </asp:DetailsView> <br />
                <asp:DetailsView ID="DetailsView2" runat="server" Height="50px" Width="233px" 
                    AutoGenerateRows="False" DataSourceID="SqlDataSource3">
                    <Fields>
                        <asp:BoundField DataField="Expr1" 
                            HeaderText="Wartość zapłaconych biletów tego klienta:" ReadOnly="True"  HeaderStyle-Height="20"> 
                            <HeaderStyle Height="20px"></HeaderStyle>
                            </asp:BoundField>
                    </Fields>
                </asp:DetailsView>

            </td>
           
        </tr>
       
    </table>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" SelectCommand="SELECT        SUM(dbo.ZlecenieSzczegoly.Cena) AS Expr1
                            FROM            dbo.Zamowienia INNER JOIN
                            dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID
                            WHERE        (dbo.Zamowienia.Klient = @UserName)">
     <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="UserName" PropertyName="SelectedValue" Type="String" >

        </asp:ControlParameter >
    
    </SelectParameters>           
                            
</asp:SqlDataSource>
                                                                                           

    <asp:SqlDataSource ID="SqlDataSource3" runat="server"  ConnectionString="<%$ ConnectionStrings:BazaRezerwacjeConnectionString %>" 
    SelectCommand="SELECT        SUM(dbo.ZlecenieSzczegoly.Cena) AS Expr1
                            FROM            dbo.Zamowienia INNER JOIN
                            dbo.ZlecenieSzczegoly ON dbo.Zamowienia.ZamowienieID = dbo.ZlecenieSzczegoly.ZamowienieID
                            WHERE        (dbo.Zamowienia.Klient = @UserName) and (dbo.ZlecenieSzczegoly.Zaplacono='True')">
     <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="UserName" PropertyName="SelectedValue" Type="String" >

        </asp:ControlParameter >
    
    </SelectParameters>      
    
    </asp:SqlDataSource>
   


</asp:Content>
