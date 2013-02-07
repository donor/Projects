<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ZarzadzaniePracownikami.aspx.cs" Inherits="Rezerwacje.Admin.ZarzadzaniePracownikami" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"  TagPrefix="asp" %>
<%@ Import Namespace="System.Web.Profile" %>
<%@ Import Namespace="System.Net.Mail" %>

<script runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

   

    protected void Button1_Click(object sender, EventArgs e)
    {
        Button1.Visible = false;
        CreateUserWizard1.Visible = true;
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

        Roles.AddUserToRole(userName, "Pracownik"); //dodawanie uzytkownika do roli pracownik
        Guid userId = (Guid)Membership.GetUser(userName).ProviderUserKey;


      
        MembershipUser user = Membership.GetUser(userId);
      
        Membership.UpdateUser(user);
        
        
       
        StringBuilder emailMessage = new StringBuilder();
        emailMessage.Append("Witamy wśród personelu serwisu Rezerwacje");
        emailMessage.Append("<br />");
        emailMessage.Append("Twój login i hasło:  <br />");
        emailMessage.Append("Login: "+userName +" <br />");
        emailMessage.Append("Hasło: " + CreateUserWizard1.Password + " <br />");

        emailMessage.Append("Nacisnij ponizszy link w celu aktywacji konta <br />");
        emailMessage.Append(string.Format("<a href='http://rezerwacje.dyndns.org/rezerwacje/Account/ActivateUser.aspx?userName{0}&Id={1}'>Aktywuj {0} </a>", userName, userId.ToString()));
        MailMessage email = new MailMessage();
        email.From = new MailAddress("serwisrezerwacje@gmail.com");
        email.To.Add(new MailAddress(emailAddress));
        email.Subject = "Witamy w Rezerwacjach";
        email.Body = emailMessage.ToString();
        email.IsBodyHtml = true;

       
        SmtpClient client = new SmtpClient();
        client.Send(email);
       
        
        CreateUserWizard1.Visible = false;
       GridView1.DataBind();
        Button1.Visible = true;
        Response.Redirect("~/Admin/ZarzadzaniePracownikami.aspx", true);
        
    }
    protected void CreateUserWizard1_CancelButtonClick(object sender, EventArgs e)
    {
        Button1.Visible = true;
        CreateUserWizard1.Visible = false;
    }
    
    </script>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {
            width: 1289px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
    <asp:ToolkitScriptManager ID="toolkitScriptMaster" runat="server">
</asp:ToolkitScriptManager>


    <asp:Button ID="Button1" runat="server" Text="Dodaj Pracownika" 
        onclick="Button1_Click" />
     <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" 
    DisableCreatedUser="True" oncreateduser="CreateUserWizard1_CreatedUser1" 
        Visible="False" DisplayCancelButton="True" 
        oncancelbuttonclick="CreateUserWizard1_CancelButtonClick">
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
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
            </asp:CompleteWizardStep>
        </WizardSteps>

    </asp:CreateUserWizard>

      <div style="height: 515px; overflow: auto">

          <asp:GridView   ID="GridView1" runat="server"  PageSize="8"
        DataSourceID="ObjectDataSource1" DataKeyNames="UserName"
     AutoGenerateColumns="False" AllowSorting="True"  Width="95%"
         ForeColor="#333333" AllowPaging="True" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField>
              <ItemTemplate>
                <asp:LinkButton ID="delete" runat="server" CommandName="Delete" Text="Usuń" />
                  <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="delete" ConfirmText="Czy napewno chcesz usunąć tego pracownika?">
                  </asp:ConfirmButtonExtender>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField  ShowEditButton="True" />
           
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
        <EditRowStyle BackColor="#999999" />
              <EmptyDataTemplate>
                  Brak Pracowników
              </EmptyDataTemplate>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
        
      </div>
 


  
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DeleteMethod="Delete" InsertMethod="Insert" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetMembers" 
        TypeName="Rezerwacje.App_Code.MembershipUserAndProfileODS" 
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

</asp:Content>
