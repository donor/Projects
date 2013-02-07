<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="Rezerwacje.Account.Register" %>
    <%@ Import Namespace="System.Net.Mail" %>



    <script runat="server">
    
        
        protected void CreateUserWizard1_CreatedUser1(object sender, EventArgs e)
        {
            TextBox imieTextBox = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxImie");
            TextBox nazwiskoTextBox=(TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("TextBoxNazwisko");
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

            Roles.AddUserToRole(userName, "Klient"); //dodawanie uzytkownika do roli klient
            Guid userId = (Guid)Membership.GetUser(userName).ProviderUserKey;



           
            StringBuilder emailMessage = new StringBuilder();
            emailMessage.Append("Dziekujemy za założenie konta w serwisie Rezerwacje");
            emailMessage.Append("<br />");
            emailMessage.Append("Twój login i hasło:  <br />");
            emailMessage.Append("Login: " + userName + " <br />");
            emailMessage.Append("Hasło: " + CreateUserWizard1.Password + " <br />");
            emailMessage.Append("Nacisnij ponizszy link w celu aktywacji konta <br />");
         
            emailMessage.Append(string.Format("<a href='http://rezerwacje.dyndns.org/rezerwacje/Account/ActivateUser.aspx?userName{0}&Id={1}'>Aktywuj {0} </a>", userName, userId.ToString()));
            MailMessage email = new MailMessage();
            email.From = new MailAddress("serwisrezerwacje@gmail.com");
            email.To.Add(new MailAddress(emailAddress));
            email.Subject = "Aktywuj swoje konto w serwisie Rezerwacie";
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;
                
           
            SmtpClient client = new SmtpClient();
            client.Send(email);
         
            Response.Redirect("~/Default.aspx", true);
        }
           
       
       
        
        </script>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style4
        {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>Wprowadź swoje dane w celu rejestracji</p>
    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" 
    DisableCreatedUser="True" oncreateduser="CreateUserWizard1_CreatedUser1">
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
    </asp:Content>
