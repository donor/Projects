using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Web.Profile;

namespace Rezerwacje.App_Code
{

    [DataObject(true)]
    public class MembershipUserAndProfileODS
    {

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(string userName, bool isApproved,
            string comment, DateTime lastLockoutDate, DateTime creationDate,
            string email, DateTime lastActivityDate, string providerName, bool isLockedOut,
            DateTime lastLoginDate, bool isOnline, string passwordQuestion,
            DateTime lastPasswordChangedDate, string password, string passwordAnswer
                 ,string nazwisko,string imie,string pESEL,string ulica,string nrDomu,string kodPocztowy,string miejscowosc
            )
        {

            MembershipCreateStatus status;
            Membership.CreateUser(userName, password, email, passwordQuestion, passwordAnswer, isApproved, out status);

            if (status != MembershipCreateStatus.Success)
            {
                throw new ApplicationException(status.ToString());
            }

            MembershipUser mu = Membership.GetUser(userName);
            mu.Comment = comment;
            Membership.UpdateUser(mu);
            ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
            pc.Nazwisko = nazwisko;
            pc.Imie = imie;
            pc.PESEL = pESEL;
            pc.Ulica = ulica;
            pc.NrDomu = nrDomu;
            pc.KodPocztowy = kodPocztowy;
            pc.Miejscowosc = miejscowosc;
            pc.Save();
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        static public void Delete(string UserName)
        {
            Membership.DeleteUser(UserName, true);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        static public void Delete(string UserName,string original_UserName)
        {
            string userNameForDelete = String.IsNullOrEmpty(UserName) ? original_UserName : UserName;
            Membership.DeleteUser(userNameForDelete, true);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Update(string UserName, string original_UserName, string email,bool isLockedOut,
             bool isApproved, string comment, DateTime lastActivityDate, DateTime lastLoginDate
             ,string nazwisko,string imie,string pESEL,string ulica,string nrDomu,string kodPocztowy,string miejscowosc
        )
        {
            string userNameForUpdate = String.IsNullOrEmpty(UserName) ? original_UserName : UserName;
            this.Update(userNameForUpdate, email, isLockedOut, isApproved, comment, lastActivityDate,lastLoginDate
                 ,nazwisko,imie,pESEL,ulica,nrDomu,kodPocztowy,miejscowosc
            );
        }



        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(string userName, string email,bool isLockedOut,
             bool isApproved, string comment, DateTime lastActivityDate, DateTime lastLoginDate
                  ,string nazwisko,string imie,string pESEL,string ulica,string nrDomu,string kodPocztowy,string miejscowosc
        )
        {
            bool dirtyFlagMu = false;

            MembershipUser mu = Membership.GetUser(userName);

            ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
            pc.Nazwisko = nazwisko;
            pc.Imie = imie;
            pc.PESEL = pESEL;
            pc.Ulica = ulica;
            pc.NrDomu = nrDomu;
            pc.KodPocztowy = kodPocztowy;
            pc.Miejscowosc = miejscowosc;
            pc.Save();
            


            if (mu.IsLockedOut && !isLockedOut)
            {
                mu.UnlockUser();
            }

            if ( string.IsNullOrEmpty(mu.Comment) || mu.Comment.CompareTo(comment) != 0)
            {
                dirtyFlagMu = true;
                mu.Comment = comment;
            }

            if (string.IsNullOrEmpty(mu.Email) || mu.Email.CompareTo(email) != 0)
            {
                dirtyFlagMu = true;
                mu.Email = email;
            }

            if (mu.IsApproved != isApproved)
            {
                dirtyFlagMu = true;
                mu.IsApproved = isApproved;
            }

            if (dirtyFlagMu == true)
            {
                Membership.UpdateUser(mu);
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MembershipUserWrapperForMP> GetMembers()
        {
            return GetMembers(true, true, null, null);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MembershipUserWrapperForMP> GetMembers(string sortData)
        {
            return GetMembers(true, true, null, sortData);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MembershipUserWrapperForMP> GetMembers(bool approvalStatus, string sortData)
        {
            if (approvalStatus == true)
            {
                return GetMembers(true, false, null, sortData);
            }
            else
            {
                return GetMembers(false, true, null, sortData);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MembershipUserWrapperForMP> GetMembers(bool returnAllApprovedUsers, bool returnAllNotApprovedUsers,
            string usernameToFind, string sortData)
        {

            List<MembershipUserWrapperForMP> memberList = new List<MembershipUserWrapperForMP>();

            if (usernameToFind != null)
            {
                MembershipUser mu = Membership.GetUser(usernameToFind);
                if (mu != null)
                {
                    MembershipUserWrapperForMP md = new MembershipUserWrapperForMP(mu);
                    ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
                    md.Nazwisko = pc.Nazwisko;
                    md.Imie = pc.Imie;
                    md.PESEL = pc.PESEL;
                    md.Ulica = pc.Ulica;
                    md.NrDomu = pc.NrDomu;
                    md.KodPocztowy = pc.KodPocztowy;
                    md.Miejscowosc = pc.Miejscowosc;

                    if (Roles.IsUserInRole(pc.UserName, "Pracownik"))
                    memberList.Add(md);
                }
            }
            else
            {
                MembershipUserCollection muc = Membership.GetAllUsers();
                foreach (MembershipUser mu in muc)
                {
                    if ((returnAllApprovedUsers == true && mu.IsApproved == true) ||
                         (returnAllNotApprovedUsers == true && mu.IsApproved == false))
                    {
                        MembershipUserWrapperForMP md = new MembershipUserWrapperForMP(mu);
                        ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
                        md.Nazwisko = pc.Nazwisko;
                        md.Imie = pc.Imie;
                        md.PESEL = pc.PESEL;
                        md.Ulica = pc.Ulica;
                        md.NrDomu = pc.NrDomu;
                        md.KodPocztowy = pc.KodPocztowy;
                        md.Miejscowosc = pc.Miejscowosc;
                        if (Roles.IsUserInRole(pc.UserName, "Pracownik"))
                        
                        memberList.Add(md);
                    }
                }

                if (sortData == null)
                {
                    sortData = "UserName";
                }
                if (sortData.Length == 0)
                {
                    sortData = "UserName";
                }

                string sortDataBase = sortData; 
                string descString = " DESC";
                if (sortData.EndsWith(descString))
                {
                    sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
                }

                Comparison<MembershipUserWrapperForMP> comparison = null;

                switch (sortDataBase)
                {

                    case "Nazwisko":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.Nazwisko == null || rhs.Nazwisko == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.Nazwisko.CompareTo(rhs.Nazwisko);
                            }
                          }
                        );
                        break;
                    case "Imie":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.Imie == null || rhs.Imie == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.Imie.CompareTo(rhs.Imie);
                            }
                          }
                        );
                        break;
                    case "PESEL":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.PESEL == null || rhs.PESEL == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.PESEL.CompareTo(rhs.PESEL);
                            }
                          }
                        );
                        break;
                    case "Ulica":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.Ulica == null || rhs.Ulica == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.Ulica.CompareTo(rhs.Ulica);
                            }
                          }
                        );
                        break;
                    case "NrDomu":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.NrDomu == null || rhs.NrDomu == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.NrDomu.CompareTo(rhs.NrDomu);
                            }
                          }
                        );
                        break;
                    case "KodPocztowy":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.KodPocztowy == null || rhs.KodPocztowy == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.KodPocztowy.CompareTo(rhs.KodPocztowy);
                            }
                          }
                        );
                        break;
                    case "Miejscowosc":
                       comparison = new Comparison<MembershipUserWrapperForMP>(
                          delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                          {
                             if (lhs.Miejscowosc == null || rhs.Miejscowosc == null)
                             {
                                return 1;
                             }
                             else
                             {
                               return lhs.Miejscowosc.CompareTo(rhs.Miejscowosc);
                            }
                          }
                        );
                        break;
                    case "UserName":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                            delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                            {
                                return lhs.UserName.CompareTo(rhs.UserName);
                            }
                            );
                        break;
                    case "Email":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 if (lhs.Email == null || rhs.Email == null)
                                 {
                                     return 0;
                                 }
                                 else
                                 {
                                     return lhs.Email.CompareTo(rhs.Email);
                                 }
                             }
                             );
                        break;
                    case "CreationDate":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.CreationDate.CompareTo(rhs.CreationDate);
                             }
                             );
                        break;
                    case "IsApproved":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.IsApproved.CompareTo(rhs.IsApproved);
                             }
                             );
                        break;
                    case "IsOnline":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.IsOnline.CompareTo(rhs.IsOnline);
                             }
                             );
                        break;
                    case "LastLoginDate":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.LastLoginDate.CompareTo(rhs.LastLoginDate);
                             }
                             );
                        break;
                    default:
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.UserName.CompareTo(rhs.UserName);
                             }
                             );
                        break;
                }

                if (sortData.EndsWith("DESC"))
                {
                    memberList.Sort(comparison);
                    memberList.Reverse();
                }
                else
                {
                    memberList.Sort(comparison);
                }

            }

            return memberList;

        }


    }

    public class MembershipUserWrapperForMP : MembershipUser
    {

        public MembershipUserWrapperForMP(MembershipUser mu)
            : base(mu.ProviderName, mu.UserName, mu.ProviderUserKey, mu.Email, mu.PasswordQuestion,
            mu.Comment, mu.IsApproved, mu.IsLockedOut, mu.CreationDate, mu.LastLoginDate, mu.LastActivityDate,
            mu.LastPasswordChangedDate, mu.LastLockoutDate)
        {
        }

        [DataObjectField(true)]
        public override string UserName
        {
            get { return base.UserName; }
        }

        public MembershipUserWrapperForMP() {}
        public MembershipUserWrapperForMP (string nazwisko,string imie,string pESEL,string ulica,string nrDomu,string kodPocztowy,string miejscowosc)
        {
          this.nazwisko = nazwisko;
          this.imie = imie;
          this.pESEL = pESEL;
          this.ulica = ulica;
          this.nrDomu = nrDomu;
          this.kodPocztowy = kodPocztowy;
          this.miejscowosc = miejscowosc;
        }

        private string nazwisko;
        [DataObjectField(false,false,false)]
        public string Nazwisko
        {
          get { return nazwisko; }
          set { nazwisko = value; }
        }

        private string imie;
        [DataObjectField(false,false,false)]
        public string Imie
        {
          get { return imie; }
          set { imie = value; }
        }

        private string pESEL;
        [DataObjectField(false,false,false)]
        public string PESEL
        {
          get { return pESEL; }
          set { pESEL = value; }
        }

        private string ulica;
        [DataObjectField(false,false,false)]
        public string Ulica
        {
          get { return ulica; }
          set { ulica = value; }
        }

        private string nrDomu;
        [DataObjectField(false,false,false)]
        public string NrDomu
        {
          get { return nrDomu; }
          set { nrDomu = value; }
        }

        private string kodPocztowy;
        [DataObjectField(false,false,false)]
        public string KodPocztowy
        {
          get { return kodPocztowy; }
          set { kodPocztowy = value; }
        }

        private string miejscowosc;
        [DataObjectField(false,false,false)]
        public string Miejscowosc
        {
          get { return miejscowosc; }
          set { miejscowosc = value; }
        }






    }


    [DataObject(true)]  
    public class RoleDataObjectForMP
    {

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        static public List<RoleDataForMP> GetRoles()
        {
            return GetRoles(null, false);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<RoleDataForMP> GetRoles(string userName, bool showOnlyAssignedRolls)
        {
            List<RoleDataForMP> roleList = new List<RoleDataForMP>();

            string[] roleListStr = Roles.GetAllRoles();
            foreach (string roleName in roleListStr)
            {
                bool userInRole = false;
                if (userName != null)
                {
                    userInRole = Roles.IsUserInRole(userName, roleName);
                }

                if (showOnlyAssignedRolls == false || userInRole == true)
                {
                    string[] usersInRole = Roles.GetUsersInRole(roleName);
                    RoleDataForMP rd = new RoleDataForMP();
                    rd.RoleName = roleName;
                    rd.UserName = userName;
                    rd.UserInRole = userInRole;
                    rd.NumberOfUsersInRole = usersInRole.Length;
                    roleList.Add(rd);
                }
            }

            return roleList;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        static public void Insert(string roleName)
        {
            if (Roles.RoleExists(roleName) == false)
            {
                Roles.CreateRole(roleName);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        static public void Delete(string roleName)
        {
            MembershipUserCollection muc = Membership.GetAllUsers();
            string[] allUserNames = new string[1];

            foreach (MembershipUser mu in muc)
            {
                if (Roles.IsUserInRole(mu.UserName, roleName) == true)
                {
                    allUserNames[0] = mu.UserName;
                    Roles.RemoveUsersFromRole(allUserNames, roleName);
                }
            }
            Roles.DeleteRole(roleName);
        }
    }

    public class RoleDataForMP
    {

        private int numberOfUsersInRole;
        public int NumberOfUsersInRole
        {
            get { return numberOfUsersInRole; }
            set { numberOfUsersInRole = value; }
        }

        private string roleName;
        [DataObjectField(true)]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private bool userInRole;
        public bool UserInRole
        {
            get { return userInRole; }
            set { userInRole = value; }
        }

    }

    [DataObject(true)]
    public class MembershipUserAndProfileODSKlient
    {

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(string userName, bool isApproved,
            string comment, DateTime lastLockoutDate, DateTime creationDate,
            string email, DateTime lastActivityDate, string providerName, bool isLockedOut,
            DateTime lastLoginDate, bool isOnline, string passwordQuestion,
            DateTime lastPasswordChangedDate, string password, string passwordAnswer
                 , string nazwisko, string imie, string pESEL, string ulica, string nrDomu, string kodPocztowy, string miejscowosc
            )
        {

            MembershipCreateStatus status;
            Membership.CreateUser(userName, password, email, passwordQuestion, passwordAnswer, isApproved, out status);

            if (status != MembershipCreateStatus.Success)
            {
                throw new ApplicationException(status.ToString());
            }

            MembershipUser mu = Membership.GetUser(userName);
            mu.Comment = comment;
            Membership.UpdateUser(mu);
            ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
            pc.Nazwisko = nazwisko;
            pc.Imie = imie;
            pc.PESEL = pESEL;
            pc.Ulica = ulica;
            pc.NrDomu = nrDomu;
            pc.KodPocztowy = kodPocztowy;
            pc.Miejscowosc = miejscowosc;
            pc.Save();
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        static public void Delete(string UserName)
        {
            Membership.DeleteUser(UserName, true);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        static public void Delete(string UserName, string original_UserName)
        {
            string userNameForDelete = String.IsNullOrEmpty(UserName) ? original_UserName : UserName;
            Membership.DeleteUser(userNameForDelete, true);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Update(string UserName, string original_UserName, string email, bool isLockedOut,
             bool isApproved, string comment, DateTime lastActivityDate, DateTime lastLoginDate
             , string nazwisko, string imie, string pESEL, string ulica, string nrDomu, string kodPocztowy, string miejscowosc
        )
        {
            string userNameForUpdate = String.IsNullOrEmpty(UserName) ? original_UserName : UserName;
            this.Update(userNameForUpdate, email, isLockedOut, isApproved, comment, lastActivityDate, lastLoginDate
                 , nazwisko, imie, pESEL, ulica, nrDomu, kodPocztowy, miejscowosc
            );
        }



        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(string userName, string email, bool isLockedOut,
             bool isApproved, string comment, DateTime lastActivityDate, DateTime lastLoginDate
                  , string nazwisko, string imie, string pESEL, string ulica, string nrDomu, string kodPocztowy, string miejscowosc
        )
        {
            bool dirtyFlagMu = false;

            MembershipUser mu = Membership.GetUser(userName);

            ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
            pc.Nazwisko = nazwisko;
            pc.Imie = imie;
            pc.PESEL = pESEL;
            pc.Ulica = ulica;
            pc.NrDomu = nrDomu;
            pc.KodPocztowy = kodPocztowy;
            pc.Miejscowosc = miejscowosc;
            pc.Save();



            if (mu.IsLockedOut && !isLockedOut)
            {
                mu.UnlockUser();
            }

            if (string.IsNullOrEmpty(mu.Comment) || mu.Comment.CompareTo(comment) != 0)
            {
                dirtyFlagMu = true;
                mu.Comment = comment;
            }

            if (string.IsNullOrEmpty(mu.Email) || mu.Email.CompareTo(email) != 0)
            {
                dirtyFlagMu = true;
                mu.Email = email;
            }

            if (mu.IsApproved != isApproved)
            {
                dirtyFlagMu = true;
                mu.IsApproved = isApproved;
            }

            if (dirtyFlagMu == true)
            {
                Membership.UpdateUser(mu);
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MembershipUserWrapperForMP> GetMembers()
        {
            return GetMembers(true, true, null, null);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MembershipUserWrapperForMP> GetMembers(string sortData)
        {
            return GetMembers(true, true, null, sortData);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MembershipUserWrapperForMP> GetMembers(bool approvalStatus, string sortData)
        {
            if (approvalStatus == true)
            {
                return GetMembers(true, false, null, sortData);
            }
            else
            {
                return GetMembers(false, true, null, sortData);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MembershipUserWrapperForMP> GetMembers(bool returnAllApprovedUsers, bool returnAllNotApprovedUsers,
            string usernameToFind, string sortData)
        {

            List<MembershipUserWrapperForMP> memberList = new List<MembershipUserWrapperForMP>();

            if (usernameToFind != null)
            {
                MembershipUser mu = Membership.GetUser(usernameToFind);
                if (mu != null)
                {
                    MembershipUserWrapperForMP md = new MembershipUserWrapperForMP(mu);
                    ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
                    md.Nazwisko = pc.Nazwisko;
                    md.Imie = pc.Imie;
                    md.PESEL = pc.PESEL;
                    md.Ulica = pc.Ulica;
                    md.NrDomu = pc.NrDomu;
                    md.KodPocztowy = pc.KodPocztowy;
                    md.Miejscowosc = pc.Miejscowosc;

                    if (Roles.IsUserInRole(pc.UserName, "Klient"))
                        memberList.Add(md);
                }
            }
            else
            {
                MembershipUserCollection muc = Membership.GetAllUsers();
                foreach (MembershipUser mu in muc)
                {
                    if ((returnAllApprovedUsers == true && mu.IsApproved == true) ||
                         (returnAllNotApprovedUsers == true && mu.IsApproved == false))
                    {
                        MembershipUserWrapperForMP md = new MembershipUserWrapperForMP(mu);
                        ProfileCommon pc = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
                        md.Nazwisko = pc.Nazwisko;
                        md.Imie = pc.Imie;
                        md.PESEL = pc.PESEL;
                        md.Ulica = pc.Ulica;
                        md.NrDomu = pc.NrDomu;
                        md.KodPocztowy = pc.KodPocztowy;
                        md.Miejscowosc = pc.Miejscowosc;
                        if (Roles.IsUserInRole(pc.UserName, "Klient"))

                            memberList.Add(md);
                    }
                }

                if (sortData == null)
                {
                    sortData = "UserName";
                }
                if (sortData.Length == 0)
                {
                    sortData = "UserName";
                }

                string sortDataBase = sortData; 
                string descString = " DESC";
                if (sortData.EndsWith(descString))
                {
                    sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
                }

                Comparison<MembershipUserWrapperForMP> comparison = null;

                switch (sortDataBase)
                {

                    case "Nazwisko":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.Nazwisko == null || rhs.Nazwisko == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.Nazwisko.CompareTo(rhs.Nazwisko);
                               }
                           }
                         );
                        break;
                    case "Imie":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.Imie == null || rhs.Imie == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.Imie.CompareTo(rhs.Imie);
                               }
                           }
                         );
                        break;
                    case "PESEL":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.PESEL == null || rhs.PESEL == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.PESEL.CompareTo(rhs.PESEL);
                               }
                           }
                         );
                        break;
                    case "Ulica":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.Ulica == null || rhs.Ulica == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.Ulica.CompareTo(rhs.Ulica);
                               }
                           }
                         );
                        break;
                    case "NrDomu":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.NrDomu == null || rhs.NrDomu == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.NrDomu.CompareTo(rhs.NrDomu);
                               }
                           }
                         );
                        break;
                    case "KodPocztowy":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.KodPocztowy == null || rhs.KodPocztowy == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.KodPocztowy.CompareTo(rhs.KodPocztowy);
                               }
                           }
                         );
                        break;
                    case "Miejscowosc":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                           delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                           {
                               if (lhs.Miejscowosc == null || rhs.Miejscowosc == null)
                               {
                                   return 1;
                               }
                               else
                               {
                                   return lhs.Miejscowosc.CompareTo(rhs.Miejscowosc);
                               }
                           }
                         );
                        break;
                    case "UserName":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                            delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                            {
                                return lhs.UserName.CompareTo(rhs.UserName);
                            }
                            );
                        break;
                    case "Email":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 if (lhs.Email == null || rhs.Email == null)
                                 {
                                     return 0;
                                 }
                                 else
                                 {
                                     return lhs.Email.CompareTo(rhs.Email);
                                 }
                             }
                             );
                        break;
                    case "CreationDate":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.CreationDate.CompareTo(rhs.CreationDate);
                             }
                             );
                        break;
                    case "IsApproved":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.IsApproved.CompareTo(rhs.IsApproved);
                             }
                             );
                        break;
                    case "IsOnline":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.IsOnline.CompareTo(rhs.IsOnline);
                             }
                             );
                        break;
                    case "LastLoginDate":
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.LastLoginDate.CompareTo(rhs.LastLoginDate);
                             }
                             );
                        break;
                    default:
                        comparison = new Comparison<MembershipUserWrapperForMP>(
                             delegate(MembershipUserWrapperForMP lhs, MembershipUserWrapperForMP rhs)
                             {
                                 return lhs.UserName.CompareTo(rhs.UserName);
                             }
                             );
                        break;
                }

                if (sortData.EndsWith("DESC"))
                {
                    memberList.Sort(comparison);
                    memberList.Reverse();
                }
                else
                {
                    memberList.Sort(comparison);
                }

            }

            return memberList;

        }


    }

    public class MembershipUserWrapperForMPKlient : MembershipUser
    {

        public MembershipUserWrapperForMPKlient(MembershipUser mu)
            : base(mu.ProviderName, mu.UserName, mu.ProviderUserKey, mu.Email, mu.PasswordQuestion,
            mu.Comment, mu.IsApproved, mu.IsLockedOut, mu.CreationDate, mu.LastLoginDate, mu.LastActivityDate,
            mu.LastPasswordChangedDate, mu.LastLockoutDate)
        {
        }

        [DataObjectField(true)]
        public override string UserName
        {
            get { return base.UserName; }
        }

        public MembershipUserWrapperForMPKlient() { }
        public MembershipUserWrapperForMPKlient(string nazwisko, string imie, string pESEL, string ulica, string nrDomu, string kodPocztowy, string miejscowosc)
        {
            this.nazwisko = nazwisko;
            this.imie = imie;
            this.pESEL = pESEL;
            this.ulica = ulica;
            this.nrDomu = nrDomu;
            this.kodPocztowy = kodPocztowy;
            this.miejscowosc = miejscowosc;
        }

        private string nazwisko;
        [DataObjectField(false, false, false)]
        public string Nazwisko
        {
            get { return nazwisko; }
            set { nazwisko = value; }
        }

        private string imie;
        [DataObjectField(false, false, false)]
        public string Imie
        {
            get { return imie; }
            set { imie = value; }
        }

        private string pESEL;
        [DataObjectField(false, false, false)]
        public string PESEL
        {
            get { return pESEL; }
            set { pESEL = value; }
        }

        private string ulica;
        [DataObjectField(false, false, false)]
        public string Ulica
        {
            get { return ulica; }
            set { ulica = value; }
        }

        private string nrDomu;
        [DataObjectField(false, false, false)]
        public string NrDomu
        {
            get { return nrDomu; }
            set { nrDomu = value; }
        }

        private string kodPocztowy;
        [DataObjectField(false, false, false)]
        public string KodPocztowy
        {
            get { return kodPocztowy; }
            set { kodPocztowy = value; }
        }

        private string miejscowosc;
        [DataObjectField(false, false, false)]
        public string Miejscowosc
        {
            get { return miejscowosc; }
            set { miejscowosc = value; }
        }






    }

}





